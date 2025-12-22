using AutoMapper;
using SimpleECommerce.Core.DTOs.Common;
using SimpleECommerce.Core.DTOs.Product;
using SimpleECommerce.Core.Entities;
using SimpleECommerce.Core.Interfaces.Repositories;
using SimpleECommerce.Core.Interfaces.Services;

namespace SimpleECommerce.Business.Services;

public class ProductService : IProductService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public ProductService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ApiResponse<PaginatedResult<ProductDto>>> GetAllAsync(int pageNumber = 1, int pageSize = 10, int? categoryId = null, string? searchTerm = null)
    {
        var paginatedProducts = await _unitOfWork.Products.GetPaginatedAsync(pageNumber, pageSize, categoryId, searchTerm);

        var result = new PaginatedResult<ProductDto>
        {
            Items = _mapper.Map<List<ProductDto>>(paginatedProducts.Items),
            TotalCount = paginatedProducts.TotalCount,
            PageNumber = paginatedProducts.PageNumber,
            PageSize = paginatedProducts.PageSize
        };

        return ApiResponse<PaginatedResult<ProductDto>>.SuccessResult(result);
    }

    public async Task<ApiResponse<ProductDto>> GetByIdAsync(int id)
    {
        var product = await _unitOfWork.Products.GetByIdWithCategoryAsync(id);

        if (product == null)
        {
            return ApiResponse<ProductDto>.FailResult("Product not found");
        }

        return ApiResponse<ProductDto>.SuccessResult(_mapper.Map<ProductDto>(product));
    }

    public async Task<ApiResponse<ProductDto>> CreateAsync(CreateProductDto dto)
    {
        var category = await _unitOfWork.Categories.GetByIdAsync(dto.CategoryId);

        if (category == null)
        {
            return ApiResponse<ProductDto>.FailResult("Category not found");
        }

        var product = _mapper.Map<Product>(dto);

        await _unitOfWork.Products.AddAsync(product);
        await _unitOfWork.SaveChangesAsync();

        var createdProduct = await _unitOfWork.Products.GetByIdWithCategoryAsync(product.Id);

        return ApiResponse<ProductDto>.SuccessResult(_mapper.Map<ProductDto>(createdProduct), "Product created successfully");
    }

    public async Task<ApiResponse<ProductDto>> UpdateAsync(int id, UpdateProductDto dto)
    {
        var product = await _unitOfWork.Products.GetByIdAsync(id);

        if (product == null)
        {
            return ApiResponse<ProductDto>.FailResult("Product not found");
        }

        var category = await _unitOfWork.Categories.GetByIdAsync(dto.CategoryId);

        if (category == null)
        {
            return ApiResponse<ProductDto>.FailResult("Category not found");
        }

        _mapper.Map(dto, product);

        await _unitOfWork.Products.UpdateAsync(product);
        await _unitOfWork.SaveChangesAsync();

        var updatedProduct = await _unitOfWork.Products.GetByIdWithCategoryAsync(product.Id);

        return ApiResponse<ProductDto>.SuccessResult(_mapper.Map<ProductDto>(updatedProduct), "Product updated successfully");
    }

    public async Task<ApiResponse> DeleteAsync(int id)
    {
        var product = await _unitOfWork.Products.GetByIdAsync(id);

        if (product == null)
        {
            return ApiResponse.FailResult("Product not found");
        }

        await _unitOfWork.Products.SoftDeleteAsync(product);
        await _unitOfWork.SaveChangesAsync();

        return ApiResponse.SuccessResult("Product deleted successfully");
    }
}
