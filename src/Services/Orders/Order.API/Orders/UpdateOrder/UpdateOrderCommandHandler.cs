using BuildingBlocks.CQRS;
using BuildingBlocks.Messaging.Events;
using FluentValidation;
using Mapster;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Order.API.Data;
using Order.API.Dtos;

namespace Order.API.Orders.UpdateOrder
{
    public record UpdateOrderCommand(OrderUpdateRequest OrderUpdateRequest) : ICommand<OrderResponse>;

    public class UpdateOrderCommandValidator : AbstractValidator<UpdateOrderCommand>
    {
        public UpdateOrderCommandValidator()
        {
            RuleFor(x => x.OrderUpdateRequest).NotNull();
            RuleFor(x => x.OrderUpdateRequest.OrderId).NotEmpty();
            RuleFor(x => x.OrderUpdateRequest.OrderStatus).IsInEnum();
        }
    }
    public class UpdateOrderCommandHandler(OrderDbContext db , IPublishEndpoint publish)
        : ICommandHandler<UpdateOrderCommand, OrderResponse>
    {
        public async Task<OrderResponse> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
        {
            if (request is null)
                throw new ArgumentNullException(nameof(request));

            var order = await db.Orders
              .Include(o => o.OrderItems)
              .FirstOrDefaultAsync(o => o.Id == request.OrderUpdateRequest.OrderId, cancellationToken);

            if (order == null)
                throw new KeyNotFoundException("Order not found");

            order.OrderStatus = request.OrderUpdateRequest.OrderStatus;

            db.Orders.Update(order);

            await db.SaveChangesAsync();

            await publish.Publish(
                new OrderCompletedEvent(
                    order.Id,
                    order.UserId,
                    order.OrderStatus.ToString(),
                    order.OrderItems.Select(oi=>oi.CourseId).ToList(),
                    order.CreatedAt
                ),
                cancellationToken
            );

            return order.Adapt<OrderResponse>();
        }
    }
}
