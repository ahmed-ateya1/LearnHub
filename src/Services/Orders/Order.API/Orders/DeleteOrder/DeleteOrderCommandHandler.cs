using BuildingBlocks.CQRS;
using Order.API.Data;

namespace Order.API.Orders.DeleteOrder
{
    public record DeleteOrderCommand(Guid OrderId) : ICommand<bool>;
    public class DeleteOrderCommandHandler(OrderDbContext db)
        : ICommandHandler<DeleteOrderCommand, bool>
    {
        public async Task<bool> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
        {
            var order = await db.Orders.FindAsync(request.OrderId);
            if (order == null)
            {
                return false;
            }
            db.Orders.Remove(order);
            await db.SaveChangesAsync();
            return true;
        }
    }
}
