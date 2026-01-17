using Core.Dtos.Category;
using Core.Results;

namespace Core.IServices;

public interface ICategoryService
{
    Task<DataResponse<List<CategoryDto>>> GetAllCategoriesAsync();
    Task<DataResponse<CategoryDto>> GetCategoryByIdAsync(int categoryId);
    Task<DataResponse<CategoryDto>> CreateCategoryAsync(CategoryCreateDto categoryCreateDto);
    Task<Response> UpdateCategoryAsync(CategoryUpdateDto categoryUpdateDto);
    Task<Response> DeleteCategoryAsync(int categoryId);
}
