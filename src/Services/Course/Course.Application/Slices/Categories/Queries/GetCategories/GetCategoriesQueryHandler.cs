using Course.Application.Dtos.CategoryDto;

namespace Course.Application.Slices.Categories.Queries.GetCategories
{
    public record GetCategoriesQuery : IQuery<IEnumerable<CategoryResponse>>;
    public class GetCategoriesQueryHandler (ICategoryService categoryService)
        : IQueryHandler<GetCategoriesQuery, IEnumerable<CategoryResponse>>
    {
        public async Task<IEnumerable<CategoryResponse>> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
        {
            return await categoryService.GetAllCategoriesAsync();
        }
    }
}
