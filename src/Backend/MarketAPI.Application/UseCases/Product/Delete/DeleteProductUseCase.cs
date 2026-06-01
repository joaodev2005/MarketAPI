using MarketAPI.Domain.Repositories;
using MarketAPI.Domain.Repositories.Product;
using MarketAPI.Exception.ExceptionsBase;

namespace MarketAPI.Application.UseCases.Product.Delete;

public class DeleteProductUseCase : IDeleteProductUseCase
{
    private readonly IProductRepository _productRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteProductUseCase(IProductRepository productRepository, IUnitOfWork unitOfWork)
    {
        _productRepository = productRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Execute(Guid id)
    {
        var product = await _productRepository.GetByIdAsync(id);
        if (product is null)
            throw new NotFoundException("Product not found");

        await _productRepository.DeleteAsync(product);
        await _unitOfWork.Commit();
    }
}