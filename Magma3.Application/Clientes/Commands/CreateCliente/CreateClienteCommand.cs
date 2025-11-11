using Magma3.Application.DTOs.Clientes;
using Magma3.Domain;
using Magma3.Infraestructure.Interfaces;
using MediatR;

namespace Magma3.Application.Clientes.Commands.CreateCliente
{
    public class CreateClienteCommand : AuthorizationBaseRequest, IRequest<ClienteDto>
    {
        public string? Nome { get; set; }
        public string? Email { get; set; }
        public string? Telefone { get; set; }
        public string? Cpf { get; set; }
        public bool Ativo { get; set; } = true;
    }

    public class CreateClienteCommandHandler : IRequestHandler<CreateClienteCommand, ClienteDto>
    {
        private readonly IClienteRepository _repository;

        public CreateClienteCommandHandler(IClienteRepository repository)
        {
            _repository = repository;
        }

        public async Task<ClienteDto> Handle(CreateClienteCommand request, CancellationToken cancellationToken)
        {
            var cliente = new Cliente
            {
                Nome = request.Nome,
                Email = request.Email,
                Telefone = request.Telefone,
                Cpf = request.Cpf,
                DataCadastro = DateTime.UtcNow,
                Ativo = request.Ativo
            };

            var createdCliente = await _repository.CreateAsync(cliente);

            return ClienteDto.New(createdCliente);
        }
    }
}
