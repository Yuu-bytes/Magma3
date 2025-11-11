using Magma3.Domain;
using Magma3.Infraestructure.Interfaces;
using MediatR;

namespace Magma3.Application.Products.Commands.CreateProduct
{
    public class CreateProductCommand : AuthorizationBaseRequest, IRequest<Product>
    {
        public string? Name { get; set; }
        public decimal Price { get; set; }
    }

    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Product>
    {
        private readonly IProductRepository _productRepository;

        public CreateProductCommandHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<Product> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var product = new Product
            {
                Name = request.Name,
                Price = request.Price
            };

            return await _productRepository.CreateAsync(product);
        }
    }
}
