using Carter;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Order.API.Dtos;

namespace Order.API.Orders.UpdateOrder
{
    public class UpdateOrderEndpoints : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPut("/orders", async ([FromBody] OrderUpdateRequest request, ISender sender) =>
            {
                var result = await sender.Send(new UpdateOrderCommand(request));
                if (result == null)
                {
                    return Results.NotFound();
                }
                return Results.Ok(result);
            })
             .WithDescription("Update Order")
             .WithTags("Orders")
             .Produces<OrderResponse>(StatusCodes.Status200OK)
             .Produces(StatusCodes.Status404NotFound)
             .ProducesValidationProblem();

        }
    }
}
