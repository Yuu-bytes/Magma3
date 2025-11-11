namespace Magma3.Domain
{
    public class Cliente
    {
        public int Id { get; set; }
        public string? Nome { get; set; }
        public string? Email { get; set; }
        public string? Telefone { get; set; }
        public string? Cpf { get; set; }
        public DateTime DataCadastro { get; set; }
        public bool Ativo { get; set; }

        public void Update(string? nome, string? email, string? telefone, string? cpf, bool? ativo)
        {
            if (nome != null)
            {
                Nome = nome;
            }

            if (email != null)
            {
                Email = email;
            }

            if (telefone != null)
            {
                Telefone = telefone;
            }

            if (cpf != null)
            {
                Cpf = cpf;
            }

            if (ativo.HasValue)
            {
                Ativo = ativo.Value;
            }
        }
    }
}
