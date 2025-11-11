using Magma3.Application.DTOs.Clientes;
using Magma3.Infraestructure.Interfaces;
using MediatR;

namespace Magma3.Application.Clientes.Queries.GetClientes
{
    public class GetClientesQuery : AuthorizationBaseRequest, IRequest<List<ClienteDto>>
    {
    }

    public class GetClientesQueryHandler : IRequestHandler<GetClientesQuery, List<ClienteDto>>
    {
        private readonly IClienteRepository _repository;

        public GetClientesQueryHandler(IClienteRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<ClienteDto>> Handle(GetClientesQuery request, CancellationToken cancellationToken)
        {
            var clientes = await _repository.GetAllAsync();

            return clientes.Select(ClienteDto.New).ToList();
        }
    }
}
