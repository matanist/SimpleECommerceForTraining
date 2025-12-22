using AutoMapper;
using SimpleECommerce.Core.DTOs.Category;
using SimpleECommerce.Core.DTOs.Common;
using SimpleECommerce.Core.Entities;
using SimpleECommerce.Core.Interfaces.Repositories;
using SimpleECommerce.Core.Interfaces.Services;

namespace SimpleECommerce.Business.Services;

public class CategoryService : ICategoryService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CategoryService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ApiResponse<IEnumerable<CategoryDto>>> GetAllAsync()
    {
        var categories = await _unitOfWork.Categories.GetAllWithProductCountAsync();
        var categoryDtos = _mapper.Map<IEnumerable<CategoryDto>>(categories);

        return ApiResponse<IEnumerable<CategoryDto>>.SuccessResult(categoryDtos);
    }

    public async Task<ApiResponse<CategoryDto>> GetByIdAsync(int id)
    {
        var category = await _unitOfWork.Categories.GetByIdWithProductsAsync(id);

        if (category == null)
        {
            return ApiResponse<CategoryDto>.FailResult("Category not found");
        }

        return ApiResponse<CategoryDto>.SuccessResult(_mapper.Map<CategoryDto>(category));
    }

    public async Task<ApiResponse<CategoryDto>> CreateAsync(CreateCategoryDto dto)
    {
        if (await _unitOfWork.Categories.AnyAsync(c => c.Name == dto.Name))
        {
            return ApiResponse<CategoryDto>.FailResult("Category with this name already exists");
        }

        var category = _mapper.Map<Category>(dto);

        await _unitOfWork.Categories.AddAsync(category);
        await _unitOfWork.SaveChangesAsync();

        return ApiResponse<CategoryDto>.SuccessResult(_mapper.Map<CategoryDto>(category), "Category created successfully");
    }

    public async Task<ApiResponse<CategoryDto>> UpdateAsync(int id, UpdateCategoryDto dto)
    {
        var category = await _unitOfWork.Categories.GetByIdAsync(id);

        if (category == null)
        {
            return ApiResponse<CategoryDto>.FailResult("Category not found");
        }

        if (await _unitOfWork.Categories.AnyAsync(c => c.Name == dto.Name && c.Id != id))
        {
            return ApiResponse<CategoryDto>.FailResult("Category with this name already exists");
        }

        _mapper.Map(dto, category);

        await _unitOfWork.Categories.UpdateAsync(category);
        await _unitOfWork.SaveChangesAsync();

        return ApiResponse<CategoryDto>.SuccessResult(_mapper.Map<CategoryDto>(category), "Category updated successfully");
    }

    public async Task<ApiResponse> DeleteAsync(int id)
    {
        var category = await _unitOfWork.Categories.GetByIdWithProductsAsync(id);

        if (category == null)
        {
            return ApiResponse.FailResult("Category not found");
        }

        if (category.Products.Any())
        {
            return ApiResponse.FailResult("Cannot delete category with existing products");
        }

        await _unitOfWork.Categories.SoftDeleteAsync(category);
        await _unitOfWork.SaveChangesAsync();

        return ApiResponse.SuccessResult("Category deleted successfully");
    }
}
