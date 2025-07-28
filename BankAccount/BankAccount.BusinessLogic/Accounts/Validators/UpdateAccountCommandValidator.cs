using BankAccount.BusinessLogic.Accounts.Commands;
using BankAccount.Domain.Enums;
using FluentValidation;

namespace BankAccount.BusinessLogic.Accounts.Validators
{
    public class UpdateAccountCommandValidator : AbstractValidator<UpdateAccountCommand>
    {
        public UpdateAccountCommandValidator()
        {
            RuleFor(x => x.AccountId)
                .NotEmpty().WithMessage("Необходимо указать идентификатор счёта");

            RuleFor(x => x.OwnerId)
                 .NotEmpty().WithMessage("Необходимо указать идентификатор владельца.");

            RuleFor(x => x.Type)
                .IsInEnum().WithMessage("Указан недопустимый тип счёта.");

            RuleFor(x => x.Currency)
                .NotEmpty().WithMessage("Необходимо указать валюту.")
                .Length(3).WithMessage("Валюта должна быть в формате ISO 4217.");

            RuleFor(x => x.InitialBalance)
                .GreaterThanOrEqualTo(0).WithMessage("Начальный баланс должен быть больше или равен нулю.");

            When(x => x.Type == AccountType.Deposit || x.Type == AccountType.Credit, () =>
            {
                RuleFor(x => x.InterestRate)
                    .NotNull().WithMessage("Для счетов типа Deposit и Credit необходимо указать процентную ставку.")
                    .GreaterThanOrEqualTo(0).WithMessage("Процентная ставка должна быть больше или равна нулю.");
            });

            When(x => x.Type == AccountType.Checking, () =>
            {
                RuleFor(x => x.InterestRate)
                    .Null().WithMessage("Для счетов типа Checking процентная ставка не должна быть указана.");
            });

            RuleFor(x => x.OpenDate)
                .NotNull().WithMessage("Необходимо указать дату открытия счета.")
                .GreaterThan(x => x.CloseDate)
                .When(x => x.CloseDate.HasValue).WithMessage("Дата закрытия должна быть позже даты открытия.");
        }
    }
}
