using Magma3.Infraestructure.Interfaces;
using MediatR;

namespace Magma3.Application.Products.Commands.DeleteProduct
{
    public class DeleteProductCommand : AuthorizationBaseRequest, IRequest<bool>
    {
        public int Id { get; set; }
    }

    public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, bool>
    {
        private readonly IProductRepository _productRepository;

        public DeleteProductCommandHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<bool> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            return await _productRepository.DeleteAsync(request.Id);
        }
    }
}
