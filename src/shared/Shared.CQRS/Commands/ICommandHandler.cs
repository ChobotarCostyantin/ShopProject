using MediatR;
using Shared.ErrorHandling;

namespace Shared.CQRS.Commands;

public interface ICommandHandler<in TCommand, TResponse> : IRequestHandler<TCommand, TResponse> where TCommand : ICommand<TResponse>;

public interface ICommandHandler<in TCommand> : IRequestHandler<TCommand, Result<Unit>> where TCommand : ICommand;