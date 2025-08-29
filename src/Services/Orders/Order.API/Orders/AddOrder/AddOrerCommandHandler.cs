using BuildingBlocks.CQRS;
using FluentValidation;
using Mapster;
using Order.API.Data;
using Order.API.Dtos;

namespace Order.API.Orders.AddOrder
{
    public record AddOrderCommand(OrderAddRequest OrderAddRequest) : ICommand<OrderResponse>;

    public class AddOrderCommandValidator : AbstractValidator<AddOrderCommand>
    {
        public AddOrderCommandValidator()
        {
            RuleFor(x => x.OrderAddRequest).NotNull();
            RuleFor(x => x.OrderAddRequest.UserId).NotEmpty();
            RuleFor(x => x.OrderAddRequest.Items).NotEmpty();
        }
    }
    public class AddOrerCommandHandler(OrderDbContext db)
     : ICommandHandler<AddOrderCommand, OrderResponse>
    {
        public async Task<OrderResponse> Handle(AddOrderCommand request, CancellationToken cancellationToken)
        {
            if (request is null)
                throw new ArgumentNullException(nameof(request));

            var order = new Models.Order
            {
                UserId = request.OrderAddRequest.UserId,
                Id = Guid.NewGuid()
            };

            order.OrderItems = request.OrderAddRequest.Items.Select(i => new Models.OrderItems
            {
                Id = Guid.NewGuid(),
                Price = i.Price,
                OrderId = order.Id,
                CourseId = i.CourseId
                  
            }).ToList();

            order.TotalPrice = order.OrderItems.Sum(i => i.Price);

            await db.Orders.AddAsync(order, cancellationToken);
            await db.SaveChangesAsync(cancellationToken);

            return order.Adapt<OrderResponse>();
        }
    }

}
