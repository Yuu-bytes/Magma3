using Magma3.Infraestructure.Interfaces;
using MediatR;
using System.Text.Json.Serialization;

namespace Magma3.Application.Clientes.Commands.DeleteCliente
{
    public class DeleteClienteCommand : AuthorizationBaseRequest, IRequest<bool>
    {
        [JsonIgnore]
        public int Id { get; set; }
    }

    public class DeleteClienteCommandHandler : IRequestHandler<DeleteClienteCommand, bool>
    {
        private readonly IClienteRepository _repository;

        public DeleteClienteCommandHandler(IClienteRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(DeleteClienteCommand request, CancellationToken cancellationToken)
        {
            return await _repository.DeleteAsync(request.Id);
        }
    }
}
