using Carter;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Order.API.Dtos;

namespace Order.API.Orders.AddOrder
{
    public class AddOrderEndpoints : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/api/orders", async ([FromBody] OrderAddRequest request, ISender sender) =>
            {
                var result = await sender.Send(new AddOrderCommand(request));
                return Results.Created($"/orders/{result.Id}", result);
            })
             .WithDescription("Add Order")
             .WithTags("Orders")
             .Produces<OrderResponse>(StatusCodes.Status201Created)
             .ProducesValidationProblem();

        }
    }
}
