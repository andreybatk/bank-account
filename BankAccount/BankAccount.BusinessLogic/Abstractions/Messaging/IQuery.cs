using MediatR;

namespace BankAccount.BusinessLogic.Abstractions.Messaging;

public interface IQuery<out TResponse> : IRequest<TResponse>;