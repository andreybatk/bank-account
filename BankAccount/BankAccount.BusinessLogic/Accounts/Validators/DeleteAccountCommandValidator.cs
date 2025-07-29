using BankAccount.BusinessLogic.Accounts.Commands;
using FluentValidation;

namespace BankAccount.BusinessLogic.Accounts.Validators;

public class DeleteAccountCommandValidator : AbstractValidator<DeleteAccountCommand>
{
    public DeleteAccountCommandValidator()
    {
        RuleFor(x => x.AccountId)
            .NotEmpty().WithMessage("Необходимо указать идентификатор счёта.");
    }
}