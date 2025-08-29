using BuildingBlocks.CQRS;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Order.API.Data;
using Order.API.Dtos;

namespace Order.API.Orders.GetOrders
{
    public record GetOrdersQuery() : IQuery<IEnumerable<OrderResponse>>;
    public class GetOrdersQueryHandler(OrderDbContext db)
        : IQueryHandler<GetOrdersQuery, IEnumerable<OrderResponse>>
    {
        public async Task<IEnumerable<OrderResponse>> Handle(GetOrdersQuery request, CancellationToken cancellationToken)
        {
            var orders = await db.Orders
                .Include(x=>x.OrderItems)
                .ToListAsync(cancellationToken);

            if (orders == null|| !orders.Any())
            {
                return Enumerable.Empty<OrderResponse>();
            }

            return orders.Adapt<IEnumerable<OrderResponse>>();
        }
    }
}
