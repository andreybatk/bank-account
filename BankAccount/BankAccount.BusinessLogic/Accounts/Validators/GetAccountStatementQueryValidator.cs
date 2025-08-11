// ReSharper disable UnusedType.Global Используется в контейнере зависимостей
using BankAccount.BusinessLogic.Accounts.Queries;
using FluentValidation;

namespace BankAccount.BusinessLogic.Accounts.Validators;

public class GetAccountStatementQueryValidator : AbstractValidator<GetAccountStatementQuery>
{
    public GetAccountStatementQueryValidator()
    {
        RuleFor(x => x.AccountId)
            .NotEmpty().WithMessage("Необходимо указать идентификатор счёта");

        RuleFor(x => x.OwnerId)
            .NotEmpty().WithMessage("Необходимо указать идентификатор владельца.");
    }
}