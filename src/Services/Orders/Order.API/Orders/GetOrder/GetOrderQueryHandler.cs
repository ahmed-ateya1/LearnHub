using BuildingBlocks.CQRS;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Order.API.Data;
using Order.API.Dtos;

namespace Order.API.Orders.GetOrder
{
    public record GetOrderQuery(Guid OrderId) :IQuery<OrderResponse>;
    public class GetOrderQueryHandler(OrderDbContext db)
        : IQueryHandler<GetOrderQuery, OrderResponse>
    {
        public async Task<OrderResponse> Handle(GetOrderQuery request, CancellationToken cancellationToken)
        {
            var order = await db.Orders
               .Include(o => o.OrderItems) 
               .FirstOrDefaultAsync(o => o.Id == request.OrderId, cancellationToken);
            if (order == null)
            {
                return null;
            }
            return order.Adapt<OrderResponse>();
        }
    }
}
