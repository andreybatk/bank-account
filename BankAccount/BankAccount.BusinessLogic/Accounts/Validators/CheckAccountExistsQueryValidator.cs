using BankAccount.BusinessLogic.Accounts.Queries;
using FluentValidation;

namespace BankAccount.BusinessLogic.Accounts.Validators;

public class CheckAccountExistsQueryValidator : AbstractValidator<CheckAccountExistsQuery>
{
    public CheckAccountExistsQueryValidator()
    {
        RuleFor(x => x.OwnerId)
            .NotEmpty().WithMessage("Необходимо указать идентификатор владельца.");
    }
}