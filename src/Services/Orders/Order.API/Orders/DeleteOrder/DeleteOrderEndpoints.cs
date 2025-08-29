using Carter;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Order.API.Orders.DeleteOrder
{
    public class DeleteOrderEndpoints : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapDelete("/orders/{orderId:guid}", async (Guid orderId, ISender sender) =>
            {
                var result = await sender.Send(new DeleteOrderCommand(orderId));
                if (!result)
                {
                    return Results.NotFound();
                }
                return Results.NoContent();
            }).WithDescription("Delete Order")
              .WithTags("Orders")
              .Produces(StatusCodes.Status204NoContent)
              .Produces(StatusCodes.Status404NotFound);
        }
    }
}
