using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BankAccount.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddAccrueInterestProcedure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
            CREATE OR REPLACE PROCEDURE accrue_interest(account_id UUID)
            LANGUAGE plpgsql
            AS $$
            DECLARE
                v_balance NUMERIC(18,2);
                v_interest_rate NUMERIC(5,2);
                v_currency CHAR(3);
                v_interest_amount NUMERIC(18,2);
                v_account_type INT;
            BEGIN
                SELECT ""Balance"", ""InterestRate"", ""Currency"", ""Type""
                INTO v_balance, v_interest_rate, v_currency, v_account_type
                FROM public.""Accounts""
                WHERE ""Id"" = account_id
                FOR UPDATE;

                IF NOT FOUND THEN
                    RAISE EXCEPTION 'Счёт % не найден', account_id;
                END IF;

                IF v_account_type <> 1 THEN
                    RAISE NOTICE 'Счёт % не депозитный.', account_id;
                    RETURN;
                END IF;

                IF v_interest_rate IS NULL OR v_interest_rate <= 0 THEN
                    RAISE NOTICE 'Счёт % не имеет процентной ставки.', account_id;
                    RETURN;
                END IF;

                v_interest_amount := ROUND(v_balance * (v_interest_rate / 100) / 12, 2);

                UPDATE public.""Accounts""
                SET ""Balance"" = ""Balance"" + v_interest_amount
                WHERE ""Id"" = account_id;

                INSERT INTO public.""Transactions"" (
                    ""Id"", ""AccountId"", ""CounterpartyAccountId"", ""Amount"", ""Currency"", ""Type"", ""Description"", ""CreatedAt""
                ) VALUES (
                    gen_random_uuid(),
                    account_id,
                    NULL,
                    v_interest_amount,
                    v_currency,
                    1,
                    'Interest accrual',
                    NOW()
                );

                RAISE NOTICE 'Начислено % процентов на счёт %', v_interest_amount, account_id;
            END;
            $$;

        ");
        }
        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS accrue_interest(UUID);");
        }
    }
}
