using MediatR;

namespace BuildingBlocks.CQRS
{
    public interface IQueryHandler<in TQuery> : IRequestHandler<TQuery, Unit>
       where TQuery : IQuery
    {
    }
    public interface IQueryHandler<in TQuery, TResponse> : IRequestHandler<TQuery, TResponse>
        where TQuery : IQuery<TResponse>
        where TResponse : notnull
    {
    }
}
