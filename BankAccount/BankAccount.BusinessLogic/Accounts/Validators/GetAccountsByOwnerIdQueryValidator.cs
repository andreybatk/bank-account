using BankAccount.BusinessLogic.Accounts.Queries;
using FluentValidation;

namespace BankAccount.BusinessLogic.Accounts.Validators
{
    public class GetAccountsByOwnerIdQueryValidator : AbstractValidator<GetAccountsByOwnerIdQuery>
    {
        public GetAccountsByOwnerIdQueryValidator()
        {
            RuleFor(x => x.OwnerId)
                .NotEmpty().WithMessage("Необходимо указать идентификатор владельца.");
        }
    }
}