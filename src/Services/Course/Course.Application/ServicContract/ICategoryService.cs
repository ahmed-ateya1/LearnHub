using Course.Application.Dtos.CategoryDto;

namespace Course.Application.ServicContract
{
    public interface ICategoryService
    {
        Task<CategoryResponse> CreateCategoryAsync(CategoryAddRequest categoryAddRequest);
        Task<CategoryResponse> UpdateCategoryAsync(CategoryUpdateRequest categoryUpdateRequest);
        Task<bool> DeleteCategoryAsync(Guid categoryId);
        Task<CategoryResponse> GetCategoryByAsync(Expression<Func<Category, bool>> filter);
        Task<IEnumerable<CategoryResponse>> GetAllCategoriesAsync(Expression<Func<Category,bool>>? filter=null);
    }
}
