namespace ProductCatalog.Application.Abstractions;

public class CommandDispatcher(IServiceProvider serviceProvider) : ICommandDispatcher
{
    public Task<TResult> Dispatch<TCommand, TResult>(TCommand command, CancellationToken cancellationToken = default)
        where TCommand : ICommand<TResult>
    {
        var handler = serviceProvider.GetService(typeof(ICommandHandler<TCommand, TResult>)) as ICommandHandler<TCommand, TResult>
            ?? throw new InvalidOperationException($"No handler registered for command type {typeof(TCommand).Name}.");
        return handler.Handle(command, cancellationToken);
    }
}

public class QueryDispatcher(IServiceProvider serviceProvider) : IQueryDispatcher
{
    public Task<TResult> Dispatch<TQuery, TResult>(TQuery query, CancellationToken cancellationToken = default)
        where TQuery : IQuery<TResult>
    {
        var handler = serviceProvider.GetService(typeof(IQueryHandler<TQuery, TResult>)) as IQueryHandler<TQuery, TResult>
            ?? throw new InvalidOperationException($"No handler registered for query type {typeof(TQuery).Name}.");
        return handler.Handle(query, cancellationToken);
    }
}
