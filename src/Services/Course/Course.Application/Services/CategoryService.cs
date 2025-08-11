using Course.Application.Dtos.CategoryDto;

namespace Course.Application.Services
{
    public class CategoryService(
        IUnitOfWork unitOfWork,
        ILogger<CategoryService> logger) : ICategoryService
    {
        private async Task ExecuteWithTransactionAsync(Func<Task> action)
        {
            using var transaction = await unitOfWork.BeginTransactionAsync();
            try
            {
                await action();
                await unitOfWork.CommitTransactionAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while executing transaction");
                await unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }

        private async Task<IEnumerable<CategoryResponse>> PrepareResponseAsync(IEnumerable<Category> categories)
        {
            var responses = categories.Adapt<List<CategoryResponse>>();

            foreach (var response in responses)
            {
                var category = categories.First(c => c.Id == response.Id);
                if (category.SubCategories?.Any() == true)
                {
                    response.SubCategories = category.SubCategories.Adapt<List<CategoryResponse>>();
                }
            }

            return responses;
        }

        public async Task<CategoryResponse> CreateCategoryAsync(CategoryAddRequest categoryAddRequest)
        {
            if (categoryAddRequest == null)
                throw new ArgumentNullException(nameof(categoryAddRequest));

            logger.LogInformation("Creating category with name: {Name}", categoryAddRequest.Name);

            if (categoryAddRequest.BaseCategoryId.HasValue)
            {
                var baseCategory = await unitOfWork.Repository<Category>()
                    .GetByAsync(c => c.Id == categoryAddRequest.BaseCategoryId.Value)
                    ?? throw new CategoryNotFoundException("Base category not found");

                logger.LogInformation("Found base category: {BaseCategoryName}", baseCategory.Name);
            }

            var category = categoryAddRequest.Adapt<Category>();

            logger.LogInformation("Mapped category from request: {Category}", category);

            await ExecuteWithTransactionAsync(async () =>
            {
                await unitOfWork.Repository<Category>().CreateAsync(category);
            });

            logger.LogInformation("Category created successfully with ID: {CategoryId}", category.Id);

            var categoryResponse = category.Adapt<CategoryResponse>();
            return categoryResponse;
        }

        public async Task<CategoryResponse> UpdateCategoryAsync(CategoryUpdateRequest categoryUpdateRequest)
        {
            if (categoryUpdateRequest == null)
                throw new ArgumentNullException(nameof(categoryUpdateRequest));

            logger.LogInformation("Updating category with ID: {CategoryId}", categoryUpdateRequest.Id);

            var category = await unitOfWork.Repository<Category>()
                .GetByAsync(x => x.Id == categoryUpdateRequest.Id, includeProperties: "SubCategories")
                ?? throw new CategoryNotFoundException("Category not found");

            if (categoryUpdateRequest.BaseCategoryId.HasValue &&
                categoryUpdateRequest.BaseCategoryId.Value != categoryUpdateRequest.Id)
            {
                var baseCategory = await unitOfWork.Repository<Category>()
                    .GetByAsync(c => c.Id == categoryUpdateRequest.BaseCategoryId.Value)
                    ?? throw new CategoryNotFoundException("Base category not found");

                logger.LogInformation("Found base category: {BaseCategoryName}", baseCategory.Name);
            }

            categoryUpdateRequest.Adapt(category);

            await ExecuteWithTransactionAsync(async () =>
            {
                logger.LogInformation("Updating category details: {Category}", category);
                await unitOfWork.Repository<Category>().UpdateAsync(category);
            });

            var response = category.Adapt<CategoryResponse>();
            logger.LogInformation("Category updated successfully: {CategoryId}", response.Id);
            return response;
        }

        public async Task<bool> DeleteCategoryAsync(Guid categoryId)
        {
            logger.LogInformation("Deleting category with ID: {CategoryId}", categoryId);

            var category = await unitOfWork.Repository<Category>()
                .GetByAsync(c => c.Id == categoryId, includeProperties: "SubCategories,Courses")
                ?? throw new CategoryNotFoundException("Category not found");

            if (category.SubCategories?.Any() == true)
                throw new InvalidOperationException("Cannot delete category that has subcategories");

            if (category.Courses?.Any() == true)
                throw new InvalidOperationException("Cannot delete category that has courses");

            logger.LogInformation("Found category to delete: {Category}", category);

            await ExecuteWithTransactionAsync(async () =>
            {
                await unitOfWork.Repository<Category>().DeleteAsync(category);
            });

            logger.LogInformation("Category deleted successfully with ID: {CategoryId}", categoryId);
            return true;
        }

        public async Task<CategoryResponse> GetCategoryByAsync(Expression<Func<Category, bool>> filter)
        {
            logger.LogInformation("Fetching category with filter: {Filter}", filter);

            var category = await unitOfWork.Repository<Category>()
                .GetByAsync(filter, includeProperties: "SubCategories,BaseCategory")
                ?? throw new CategoryNotFoundException("Category not found");

            var response = category.Adapt<CategoryResponse>();
            logger.LogInformation("Category fetched successfully: {CategoryId}", response.Id);

            return response;
        }

        public async Task<IEnumerable<CategoryResponse>> GetAllCategoriesAsync(Expression<Func<Category, bool>>? filter = null)
        {
            var categories = await unitOfWork.Repository<Category>()
                .GetAllAsync(filter, includeProperties: "SubCategories,BaseCategory");

            logger.LogInformation("Retrieved {Count} categories from the database", categories?.Count() ?? 0);

            if (categories == null || !categories.Any())
                return Enumerable.Empty<CategoryResponse>();

            return await PrepareResponseAsync(categories);
        }
    }
}