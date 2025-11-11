using Magma3.Application.DTOs.Products;
using Magma3.Infraestructure.Interfaces;
using MediatR;

namespace Magma3.Application.Products.Queries.GetProducts
{
    public class GetProductsQuery : AuthorizationBaseRequest, IRequest<List<ProductDto>>
    {
    }

    public class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, List<ProductDto>>
    {
        private readonly IProductRepository _productRepository;

        public GetProductsQueryHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<List<ProductDto>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
        {
            var products = await _productRepository.GetAllAsync();
            return products.Select(ProductDto.New).ToList();
        }
    }
}
