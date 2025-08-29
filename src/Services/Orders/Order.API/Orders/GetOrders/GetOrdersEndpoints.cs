using Carter;
using MediatR;
using Order.API.Dtos;

namespace Order.API.Orders.GetOrders
{
    public class GetOrdersEndpoints : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/orders", async (ISender sender) =>
            {
                var result = await sender.Send(new GetOrdersQuery());
                if (result == null || !result.Any())
                {
                    return Results.NotFound();
                }
                return Results.Ok(result);
            }).WithDescription("Get Orders")
              .WithTags("Orders")
              .Produces<OrderResponse>(StatusCodes.Status200OK)
              .Produces(StatusCodes.Status404NotFound);
        }
    }
}
