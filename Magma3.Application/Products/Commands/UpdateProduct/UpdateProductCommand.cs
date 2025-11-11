using Magma3.Domain;
using Magma3.Infraestructure.Interfaces;
using MediatR;
using System.Text.Json.Serialization;

namespace Magma3.Application.Products.Commands.UpdateProduct
{
    public class UpdateProductCommand : AuthorizationBaseRequest, IRequest<Product>
    {
        [JsonIgnore]
        public int Id { get; set; }
        public string? Name { get; set; }
        public decimal? Price { get; set; }
    }

    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, Product>
    {
        private readonly IProductRepository _productRepository;

        public UpdateProductCommandHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<Product> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetByIdAsync(request.Id);

            if (product is null)
            {
                throw new InvalidOperationException($"Product with ID {request.Id} not found");
            }

            product.Update(request.Name, request.Price);

            return await _productRepository.UpdateAsync(product);
        }
    }
}
