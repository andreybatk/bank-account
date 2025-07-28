using BankAccount.BusinessLogic.Abstractions.Messaging;
using BankAccount.BusinessLogic.Accounts.Commands;
using BankAccount.Domain.Exceptions;
using BankAccount.Domain.Interfaces;

namespace BankAccount.BusinessLogic.Accounts.Handlers
{
    public class UpdateAccountCommandHandler : ICommandHandler<UpdateAccountCommand, Guid>
    {
        private readonly IAccountService _accountService;
        private readonly IClientVerificationService _clientVerificationService;
        private readonly ICurrencyService _currencyService;

        public UpdateAccountCommandHandler(
            IAccountService accountService,
            IClientVerificationService clientVerificationService,
            ICurrencyService currencyService)
        {
            _accountService = accountService;
            _clientVerificationService = clientVerificationService;
            _currencyService = currencyService;
        }

        public async Task<Guid> Handle(UpdateAccountCommand request, CancellationToken cancellationToken)
        {
            var errors = new Dictionary<string, string[]>();

            bool clientExists = await _clientVerificationService.ClientExistsAsync(request.OwnerId);
            if (!clientExists)
                errors.Add(nameof(request.OwnerId), new[] { "Клиент с таким OwnerId не найден." });

            bool currencySupported = await _currencyService.IsCurrencySupportedAsync(request.Currency);
            if (!currencySupported)
                errors.Add(nameof(request.Currency), new[] { $"Валюта '{request.Currency}' не поддерживается." });

            if (errors.Any())
                throw new ValidationException(errors);

            return await _accountService.UpdateAccountAsync(request);
        }
    }

}
