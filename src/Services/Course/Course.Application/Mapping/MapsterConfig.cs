using Mapster;

namespace Course.Application.Mapping
{
    public static class MapsterConfig
    {
        public static void Configure()
        {
            TypeAdapterConfig<Domain.Models.Course, CourseResponse>
                .NewConfig()
                .Map(dest => dest.AverageRating, src => src.Reviews.Average(x => x.Rating))
                .Map(dest => dest.ReviewCount, src => src.Reviews.Count())
                .Map(dest => dest.CategoryName, src => src.Category.Name);



        }
    }
}
