# CRUD de Clientes com MongoDB

## ?? Visão Geral

Este módulo implementa um CRUD completo para a entidade **Cliente** utilizando **MongoDB** como banco de dados.

## ??? Estrutura

### Entidade Cliente
```csharp
public class Cliente
{
    public int Id { get; set; }
  public string? Nome { get; set; }
    public string? Email { get; set; }
    public string? Telefone { get; set; }
    public string? Cpf { get; set; }
    public DateTime DataCadastro { get; set; }
    public bool Ativo { get; set; }
}
```

### Repositório MongoDB

O repositório `MongoDbClienteRepository` implementa:
- ? Criação automática de índices (Id e Email único)
- ? Geração automática de IDs sequenciais
- ? Data de cadastro automática
- ? Operações assíncronas completas

## ?? Configuração

### 1. Configurar Connection String

No seu `appsettings.json` ou `Program.cs`:

```json
{
  "MongoDB": {
    "ConnectionString": "mongodb://localhost:27017",
    "DatabaseName": "magma3db"
  }
}
```

### 2. Registrar o Repositório

No `Program.cs` ou arquivo de configuração de DI:

```csharp
// Registrar o repositório de clientes
builder.Services.AddSingleton<IClienteRepository>(sp =>
{
 var configuration = sp.GetRequiredService<IConfiguration>();
    var connectionString = configuration["MongoDB:ConnectionString"] ?? "mongodb://localhost:27017";
    var databaseName = configuration["MongoDB:DatabaseName"] ?? "magma3db";
    return new MongoDbClienteRepository(connectionString, databaseName);
});
```

## ?? Endpoints da API

### GET /api/clientes
Lista todos os clientes

**Resposta:**
```json
[
  {
    "id": 1,
    "nome": "João Silva",
    "email": "joao@example.com",
    "telefone": "(11) 98888-7777",
    "cpf": "123.456.789-00",
    "dataCadastro": "2025-01-15T10:30:00Z",
    "ativo": true
  }
]
```

### GET /api/clientes/{id}
Busca um cliente específico

**Resposta:**
```json
{
  "id": 1,
  "nome": "João Silva",
  "email": "joao@example.com",
  "telefone": "(11) 98888-7777",
  "cpf": "123.456.789-00",
  "dataCadastro": "2025-01-15T10:30:00Z",
  "ativo": true
}
```

### POST /api/clientes
Cria um novo cliente

**Request Body:**
```json
{
  "nome": "Maria Santos",
  "email": "maria@example.com",
  "telefone": "(11) 97777-6666",
  "cpf": "987.654.321-00",
  "ativo": true
}
```

**Resposta:** `201 Created`
```json
{
  "id": 2,
  "nome": "Maria Santos",
  "email": "maria@example.com",
  "telefone": "(11) 97777-6666",
  "cpf": "987.654.321-00",
  "dataCadastro": "2025-01-15T11:00:00Z",
  "ativo": true
}
```

### PUT /api/clientes/{id}
Atualiza um cliente existente

**Request Body:**
```json
{
  "id": 1,
  "nome": "João Silva Santos",
  "email": "joao.silva@example.com",
  "telefone": "(11) 98888-7777",
  "cpf": "123.456.789-00",
  "ativo": true
}
```

**Resposta:** `200 OK`

### DELETE /api/clientes/{id}
Remove um cliente

**Resposta:** `204 No Content`

## ?? Exemplos de Uso

### Usando cURL

```bash
# Listar todos
curl -X GET http://localhost:5000/api/clientes

# Buscar por ID
curl -X GET http://localhost:5000/api/clientes/1

# Criar novo
curl -X POST http://localhost:5000/api/clientes \
  -H "Content-Type: application/json" \
  -d '{
    "nome": "Pedro Costa",
    "email": "pedro@example.com",
    "telefone": "(21) 96666-5555",
    "cpf": "111.222.333-44",
    "ativo": true
  }'

# Atualizar
curl -X PUT http://localhost:5000/api/clientes/1 \
  -H "Content-Type: application/json" \
  -d '{
    "id": 1,
    "nome": "Pedro Costa Silva",
    "email": "pedro.costa@example.com",
  "ativo": false
  }'

# Deletar
curl -X DELETE http://localhost:5000/api/clientes/1
```

### Usando C# Client

```csharp
using var client = new HttpClient { BaseAddress = new Uri("http://localhost:5000") };

// Criar cliente
var createCommand = new CreateClienteCommand
{
    Nome = "Ana Paula",
    Email = "ana@example.com",
 Telefone = "(31) 95555-4444",
    Cpf = "222.333.444-55",
    Ativo = true
};

var response = await client.PostAsJsonAsync("/api/clientes", createCommand);
var cliente = await response.Content.ReadFromJsonAsync<ClienteDto>();

// Listar todos
var clientes = await client.GetFromJsonAsync<List<ClienteDto>>("/api/clientes");

// Buscar por ID
var cliente = await client.GetFromJsonAsync<ClienteDto>($"/api/clientes/{id}");

// Atualizar
var updateCommand = new UpdateClienteCommand
{
    Id = cliente.Id,
    Nome = "Ana Paula Silva",
    Ativo = false
};
await client.PutAsJsonAsync($"/api/clientes/{cliente.Id}", updateCommand);

// Deletar
await client.DeleteAsync($"/api/clientes/{cliente.Id}");
```

## ?? Queries Diretas no MongoDB

```javascript
// Conectar ao MongoDB
mongosh "mongodb://localhost:27017/magma3db"

// Listar todos os clientes
db.clientes.find().pretty()

// Buscar por nome
db.clientes.find({ nome: /Silva/i }).pretty()

// Buscar clientes ativos
db.clientes.find({ ativo: true }).pretty()

// Contar clientes
db.clientes.countDocuments()

// Buscar por email
db.clientes.find({ email: "joao@example.com" }).pretty()
```

## ?? Características Técnicas

### Índices MongoDB
- **Id**: Índice ascendente para buscas rápidas
- **Email**: Índice único e esparso (permite nulls, mas emails devem ser únicos)

### Validações
- Email único no banco de dados
- ID gerado automaticamente de forma sequencial
- Data de cadastro definida automaticamente em UTC

### Padrões Utilizados
- ? Clean Architecture
- ? CQRS (Command Query Responsibility Segregation)
- ? MediatR para mediação de comandos e queries
- ? Repository Pattern
- ? Async/Await em todas as operações

## ?? Dependências

- **MongoDB.Driver** 3.5.0
- **MediatR**
- **ASP.NET Core 8.0**

## ?? Próximos Passos

1. Adicionar validações com FluentValidation
2. Implementar paginação nas queries
3. Adicionar filtros e ordenação
4. Implementar soft delete
5. Adicionar logging e tratamento de erros
6. Criar testes unitários e de integração
