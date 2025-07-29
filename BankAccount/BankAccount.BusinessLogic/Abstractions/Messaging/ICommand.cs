using MediatR;

namespace BankAccount.BusinessLogic.Abstractions.Messaging;

public interface ICommand<out TResponse> : IRequest<TResponse>;