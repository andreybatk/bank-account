// ReSharper disable UnusedType.Global Используется в контейнере зависимостей
using BankAccount.BusinessLogic.AccountTransactions.Commands;
using BankAccount.Domain.Enums;
using FluentValidation;

namespace BankAccount.BusinessLogic.AccountTransactions.Validators;

public class CreateTransactionCommandValidator : AbstractValidator<CreateTransactionCommand>
{
    public CreateTransactionCommandValidator()
    {
        RuleFor(x => x.AccountId)
            .NotEmpty().WithMessage("Необходимо указать идентификатор счёта.");

        RuleFor(x => x.CounterpartyAccountId)
            .NotEqual(x => x.AccountId)
            .When(x => x.CounterpartyAccountId.HasValue)
            .WithMessage("Счёт получателя не может совпадать со счетом отправителя.");

        RuleFor(x => x.Amount)
            .GreaterThan(0).WithMessage("Сумма транзакции должна быть больше нуля.");

        RuleFor(x => x.Currency)
            .NotEmpty().WithMessage("Необходимо указать валюту транзакции.")
            .Length(3).WithMessage("Валюта должна быть в формате ISO 4217.");

        RuleFor(x => x.Type)
            .IsInEnum().WithMessage("Указан недопустимый тип транзакции.");

        When(x => x.Type == TransactionType.Debit, () =>
        {
            RuleFor(x => x.CounterpartyAccountId)
                .NotNull().WithMessage("Для дебетовой транзакции необходимо указать счёт получателя.");
        });
    }
}