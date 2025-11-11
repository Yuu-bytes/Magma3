using Magma3.Domain;

namespace Magma3.Application.DTOs.Clientes
{
    public class ClienteDto
    {
        public int Id { get; set; }
        public string? Nome { get; set; }
        public string? Email { get; set; }
        public string? Telefone { get; set; }
        public string? Cpf { get; set; }
        public DateTime DataCadastro { get; set; }
        public bool Ativo { get; set; }

        public static ClienteDto New(Cliente cliente) =>
            new ClienteDto
            {
                Id = cliente.Id,
                Nome = cliente.Nome,
                Email = cliente.Email,
                Telefone = cliente.Telefone,
                Cpf = cliente.Cpf,
                DataCadastro = cliente.DataCadastro,
                Ativo = cliente.Ativo
            };
    }
}
