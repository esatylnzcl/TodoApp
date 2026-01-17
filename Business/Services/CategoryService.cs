using Core.Dtos.Category;
using Core.IRepositories;
using Core.IServices;
using Core.Results;
using Entity.Concrete;

namespace Business.Services;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _categoryRepository;
    
    public CategoryService(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<DataResponse<List<CategoryDto>>> GetAllCategoriesAsync()
    {
        var categories = await _categoryRepository.GetAllAsync();
        
        var categoriesunDeleted = categories.Where(c => !c.IsDeleted).ToList();
        
        var categoryDtos = categoriesunDeleted.Select(c => new CategoryDto
        {
            Id = c.Id,
            Name = c.Name,
            Description = c.Description,
            CreatedAt = c.CreatedAt
        }).ToList();
        
        return DataResponse<List<CategoryDto>>.SuccessDataResponse(categoryDtos, "Categories fetched");
    }

    public async Task<DataResponse<CategoryDto>> GetCategoryByIdAsync(int categoryId)
    {
        var category = await _categoryRepository.GetByIdAsync(categoryId);

        if (category == null || category.IsDeleted)
            return DataResponse<CategoryDto>.ErrorDateResponse(null, "Category not found");

        var dto = new CategoryDto
        {
            Id = category.Id,
            Name = category.Name,
            Description = category.Description,
            CreatedAt = category.CreatedAt
        };

        return DataResponse<CategoryDto>.SuccessDataResponse(dto);
    }

    public async Task<DataResponse<CategoryDto>> CreateCategoryAsync(CategoryCreateDto categoryCreateDto)
    {
        var newCategory = new Category
        {
            Name = categoryCreateDto.Name,
            Description = categoryCreateDto.Description
        };

        await _categoryRepository.AddAsync(newCategory);
        await _categoryRepository.SaveAsync();

        var responseDto = new CategoryDto 
        { 
            Id = newCategory.Id, 
            Name = newCategory.Name,
            Description = newCategory.Description,
            CreatedAt = newCategory.CreatedAt
        };
        
        return DataResponse<CategoryDto>.SuccessDataResponse(responseDto, "Category created");
    }

    public async Task<Response> UpdateCategoryAsync(CategoryUpdateDto categoryUpdateDto)
    {
        var category = await _categoryRepository.GetByIdAsync(categoryUpdateDto.Id);

        if (category == null || category.IsDeleted)
            return Response.ErrorResponse("Category to update couldnt be found");

        category.Name = categoryUpdateDto.Name;
        category.Description = categoryUpdateDto.Description;

        _categoryRepository.Update(category);
        await _categoryRepository.SaveAsync();

        return Response.SuccessResponse("Category updated");
    }

    public async Task<Response> DeleteCategoryAsync(int categoryId)
    {
        var category = await _categoryRepository.GetByIdAsync(categoryId);

        if (category == null)
            return Response.ErrorResponse("Category to delete could not be found");

        _categoryRepository.SoftDelete(category);
        await _categoryRepository.SaveAsync();

        return Response.SuccessResponse("Category deleted");
    }
}
