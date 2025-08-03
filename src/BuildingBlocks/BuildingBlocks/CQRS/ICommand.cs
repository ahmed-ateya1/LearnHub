using MediatR;

namespace BuildingBlocks.CQRS
{
    public interface ICommand: IRequest<Unit>
    {

    }
    public interface ICommand<TResponse>: IRequest<TResponse>
        where TResponse : notnull
    {
    }

}
