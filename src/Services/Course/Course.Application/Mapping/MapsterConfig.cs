using Mapster;

namespace Course.Application.Mapping
{
    public static class MapsterConfig
    {
        public static void Configure()
        {
            TypeAdapterConfig<Domain.Models.Course, CourseResponse>
                .NewConfig()
                .Map(dest => dest.AverageRating,
                     src => src.Reviews != null && src.Reviews.Any()
                     ? src.Reviews.Average(x => x.Rating)
                     : 0)
                .Map(dest => dest.ReviewCount,
                     src => src.Reviews != null ? src.Reviews.Count() : 0)
                .Map(dest => dest.CategoryName,
                      src => src.Category != null ? src.Category.Name : string.Empty);

            TypeAdapterConfig<CourseAddRequest, Domain.Models.Course>
                .NewConfig()
                .Map(dest => dest.Id, src => Guid.NewGuid());
        }
    }
}
