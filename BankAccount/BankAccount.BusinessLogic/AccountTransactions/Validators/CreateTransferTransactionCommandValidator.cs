// ReSharper disable UnusedType.Global Используется в контейнере зависимостей
using BankAccount.BusinessLogic.AccountTransactions.Commands;
using FluentValidation;

namespace BankAccount.BusinessLogic.AccountTransactions.Validators;

public class CreateTransferTransactionCommandValidator : AbstractValidator<CreateTransferTransactionCommand>
{
    public CreateTransferTransactionCommandValidator()
    {
        RuleFor(x => x.AccountIdFrom)
            .NotEmpty().WithMessage("Необходимо указать идентификатор счёта отправителя.");

        RuleFor(x => x.AccountIdTo)
            .NotEmpty().WithMessage("Необходимо указать идентификатор счёта получателя.");

        RuleFor(x => x.AccountIdFrom)
            .NotEqual(x => x.AccountIdTo)
            .WithMessage("Счёт получателя не может совпадать со счетом отправителя.");

        RuleFor(x => x.Amount)
            .GreaterThan(0).WithMessage("Сумма транзакции должна быть больше нуля.");

        RuleFor(x => x.Currency)
            .NotEmpty().WithMessage("Необходимо указать валюту транзакции.")
            .Length(3).WithMessage("Валюта должна быть в формате ISO 4217.");
    }
}
