using Course.Application.Dtos.CategoryDto;

namespace Course.Application.Categories.Queries.GetBaseCategories
{
    public record GetBaseCategoriesQuery : IQuery<IEnumerable<CategoryResponse>>;
    internal class GetBaseCategoriesQueryHandler (ICategoryService categoryService)
        : IQueryHandler<GetBaseCategoriesQuery, IEnumerable<CategoryResponse>>
    {
        public async Task<IEnumerable<CategoryResponse>> Handle(GetBaseCategoriesQuery request, CancellationToken cancellationToken)
        {
            return await categoryService.GetAllCategoriesAsync(x => x.BaseCategoryId == null);
        }
    }
}
