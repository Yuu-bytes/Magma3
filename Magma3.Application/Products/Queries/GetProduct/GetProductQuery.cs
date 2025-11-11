using Magma3.Application.DTOs.Products;
using Magma3.Infraestructure.Interfaces;
using MediatR;
using System.Text.Json.Serialization;

namespace Magma3.Application.Products.Queries.GetProduct
{
    public class GetProductQuery : AuthorizationBaseRequest, IRequest<ProductDto?>
    {
        [JsonIgnore]
        public int Id { get; set; }
    }

    public class GetProductQueryHandler : IRequestHandler<GetProductQuery, ProductDto?>
    {
        private readonly IProductRepository _productRepository;

        public GetProductQueryHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<ProductDto?> Handle(GetProductQuery request, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetByIdAsync(request.Id);
            
            if (product == null)
            {
                return null;
            }

            return ProductDto.New(product);
        }
    }
}
