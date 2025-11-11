using Magma3.Application.DTOs.Clientes;
using Magma3.Infraestructure.Interfaces;
using MediatR;
using System.Text.Json.Serialization;

namespace Magma3.Application.Clientes.Commands.UpdateCliente
{
    public class UpdateClienteCommand : AuthorizationBaseRequest, IRequest<ClienteDto>
    {
        [JsonIgnore]
        public int Id { get; set; }
        public string? Nome { get; set; }
        public string? Email { get; set; }
        public string? Telefone { get; set; }
        public string? Cpf { get; set; }
        public bool? Ativo { get; set; }
    }

    public class UpdateClienteCommandHandler : IRequestHandler<UpdateClienteCommand, ClienteDto>
    {
        private readonly IClienteRepository _repository;

        public UpdateClienteCommandHandler(IClienteRepository repository)
        {
            _repository = repository;
        }

        public async Task<ClienteDto> Handle(UpdateClienteCommand request, CancellationToken cancellationToken)
        {
            var cliente = await _repository.GetByIdAsync(request.Id);
            if (cliente == null)
            {
                throw new InvalidOperationException($"Cliente with ID {request.Id} not found");
            }

            cliente.Update(request.Nome, request.Email, request.Telefone, request.Cpf, request.Ativo);

            var updatedCliente = await _repository.UpdateAsync(cliente);

            return new ClienteDto
            {
                Id = updatedCliente.Id,
                Nome = updatedCliente.Nome,
                Email = updatedCliente.Email,
                Telefone = updatedCliente.Telefone,
                Cpf = updatedCliente.Cpf,
                DataCadastro = updatedCliente.DataCadastro,
                Ativo = updatedCliente.Ativo
            };
        }
    }
}
