using Magma3.Application.DTOs.Clientes;
using Magma3.Infraestructure.Interfaces;
using MediatR;

namespace Magma3.Application.Clientes.Queries.GetCliente
{
    public class GetClienteQuery : AuthorizationBaseRequest, IRequest<ClienteDto?>
    {
        public int Id { get; set; }
    }

    public class GetClienteQueryHandler : IRequestHandler<GetClienteQuery, ClienteDto?>
    {
        private readonly IClienteRepository _repository;

        public GetClienteQueryHandler(IClienteRepository repository)
        {
            _repository = repository;
        }

        public async Task<ClienteDto?> Handle(GetClienteQuery request, CancellationToken cancellationToken)
        {
            var cliente = await _repository.GetByIdAsync(request.Id);
            if (cliente == null)
            {
                return null;
            }

            return ClienteDto.New(cliente);
        }
    }
}
