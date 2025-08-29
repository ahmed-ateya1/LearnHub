using Mapster;
using Order.API.Dtos;

namespace Order.API.Mapping
{
    public static class OrderMapping
    {
        public static void Configure()
        {
            TypeAdapterConfig<Models.Order, OrderResponse>
                .NewConfig()
                .Map(dest => dest.Items, src => src.OrderItems);
        }
    }
}
