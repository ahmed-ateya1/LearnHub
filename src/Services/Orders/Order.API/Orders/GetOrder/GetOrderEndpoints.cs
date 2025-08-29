using Carter;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Order.API.Dtos;

namespace Order.API.Orders.GetOrder
{
    public class GetOrderEndpoints : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/orders/{orderId:guid}", async (Guid orderId, ISender sender) =>
            {
                var result = await sender.Send(new GetOrderQuery(orderId));
                if (result == null)
                {
                    return Results.NotFound();
                }
                return Results.Ok(result);
            }).WithDescription("Get Order by Id")
              .WithTags("Orders")
              .Produces<OrderResponse>(StatusCodes.Status200OK)
              .Produces(StatusCodes.Status404NotFound);
        }
    }
}
