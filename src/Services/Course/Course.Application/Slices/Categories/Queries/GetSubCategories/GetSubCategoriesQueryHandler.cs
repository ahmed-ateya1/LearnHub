using Course.Application.Dtos.CategoryDto;

namespace Course.Application.Slices.Categories.Queries.GetSubCategories
{
    public record GetSubCategoriesQuery(Guid CategoryId) : IQuery<IEnumerable<CategoryResponse>>;
    internal class GetSubCategoriesQueryHandler(ICategoryService categoryService)
        : IQueryHandler<GetSubCategoriesQuery, IEnumerable<CategoryResponse>>
    {
        public async Task<IEnumerable<CategoryResponse>> Handle(GetSubCategoriesQuery request, CancellationToken cancellationToken)
        {
            return await categoryService.GetAllCategoriesAsync(x=>x.BaseCategoryId == request.CategoryId);
        }
    }
}
