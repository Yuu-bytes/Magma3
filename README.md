# Magma3

Sistema de gerenciamento e monitoramento de ativos desenvolvido com .NET 8 e MongoDB.

## ?? Sobre o Projeto

Magma3 é uma aplicação que integra com a API do Magma-3 para gerenciar ativos, computadores e clientes. O projeto segue os princípios de Clean Architecture com separação em camadas (Domain, Application, Infrastructure e Presentation).

## ??? Arquitetura

O projeto está organizado em 5 projetos principais:

- **Magma3**: API principal (Web API)
- **Magma3.Application**: Camada de aplicação (Use Cases, CQRS com MediatR)
- **Magma3.Domain**: Camada de domínio (Entidades, Interfaces)
- **Magma3.Infrastructure**: Camada de infraestrutura (Repositórios, MongoDB)
- **Magma3.WebClient**: Cliente HTTP para integração com API externa

## ?? Tecnologias

- **.NET 8**
- **MongoDB** - Banco de dados NoSQL
- **MediatR** - Implementação de CQRS e Mediator Pattern
- **Swagger/OpenAPI** - Documentação da API
- **Docker & Docker Compose** - Containerização
- **Rate Limiting** - Controle de taxa de requisições

## ?? Pré-requisitos

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Docker](https://www.docker.com/get-started) e [Docker Compose](https://docs.docker.com/compose/install/)
- Visual Studio 2022 ou VS Code

## ?? Configuração

### 1. Clone o repositório

```bash
git clone https://github.com/Yuu-bytes/Magma3.git
cd Magma3
```

### 2. Configure o MongoDB com Docker

Inicie o MongoDB e o Mongo Express:

```bash
docker-compose up -d
```

Isso irá:
- Criar o container MongoDB na porta **27017**
- Criar o Mongo Express (interface web) na porta **8081**
- Configurar autenticação com as credenciais definidas

**Credenciais do MongoDB:**
- Usuário: `admin`
- Senha: `admin123`
- Database: `magma3db`

**Acessar Mongo Express:**
- URL: http://localhost:8081

### 3. Configure o appsettings.json

Ajuste as configurações em `Magma3/appsettings.json`:

```json
{
  "MongoDB": {
    "ConnectionString": "mongodb://admin:admin123@localhost:27017",
    "DatabaseName": "magma3db"
  },
  "Magma3Config": {
    "BaseUrl": "https://api.magma-3.com",
    "Enterprise": "sua-empresa",
    "Login": "seu-login",
    "Password": "sua-senha"
  },
  "RateLimitConfig": {
    "PermitLimit": 100,
    "WindowInSeconds": 30,
    "QueueLimit": 10
  }
}
```

### 4. Execute a aplicação

```bash
cd Magma3
dotnet run
```

Ou use o Visual Studio (F5).

A API estará disponível em:
- **HTTPS**: https://localhost:7000
- **HTTP**: http://localhost:5000
- **Swagger**: https://localhost:7000/swagger

## ?? Endpoints Principais

### Assets

- `GET /api/Assets/ComputersOff` - Lista computadores desligados
- `GET /api/Assets/GetAtivos?page={page}` - Lista ativos paginados

### Clientes

- `GET /api/Clientes` - Lista clientes

### Products

- `GET /api/Products` - Lista produtos

## ?? Autenticação

A API usa autenticação via **ApiKey** no header:

```
ApiKey: 9da0c852-1a10-4399-ac98-969577625c1f
```

Configure sua ApiKey no Swagger UI para testar os endpoints.

## ? Rate Limiting

A API implementa rate limiting com as seguintes configurações padrão:

- **Limite**: 100 requisições
- **Janela**: 30 segundos
- **Fila**: 10 requisições

Quando o limite é excedido, a API retorna status **429 Too Many Requests**.

## ??? Estrutura de Pastas

```
Magma3/
??? Magma3/  # API Web
?   ??? Controllers/       # Controllers da API
?   ??? Properties/
?   ??? appsettings.json
??? Magma3.Application/     # Camada de Aplicação
?   ??? Assets/
?   ?   ??? Queries/       # Queries CQRS
?   ??? Clientes/
?   ??? Products/
??? Magma3.Domain/    # Camada de Domínio
?   ??? Entities/               # Entidades do domínio
??? Magma3.Infrastructure/  # Camada de Infraestrutura
?   ??? Persistence/  # Repositórios MongoDB
?   ??? DependencyInjection.cs
??? Magma3.WebClient/        # Cliente HTTP
?   ??? WebClient/
??? docker-compose.yml              # Configuração Docker
??? README.md
```

## ?? Comandos Docker Úteis

### Verificar status dos containers
```bash
docker-compose ps
```

### Ver logs dos containers
```bash
docker-compose logs -f
```

### Parar os containers
```bash
docker-compose down
```

### Parar e remover volumes (apaga dados)
```bash
docker-compose down -v
```

### Reiniciar os containers
```bash
docker-compose restart
```

## ?? Testes

```bash
dotnet test
```

## ?? Documentação da API

Após iniciar a aplicação, acesse a documentação Swagger em:

```
https://localhost:7000/swagger
```

## ?? Contribuindo

1. Fork o projeto
2. Crie uma branch para sua feature (`git checkout -b feature/AmazingFeature`)
3. Commit suas mudanças (`git commit -m 'Add some AmazingFeature'`)
4. Push para a branch (`git push origin feature/AmazingFeature`)
5. Abra um Pull Request

## ?? Licença

Este projeto está sob a licença MIT. Veja o arquivo `LICENSE` para mais detalhes.

## ????? Autor

- GitHub: [@Yuu-bytes](https://github.com/Yuu-bytes)

## ?? Links Úteis

- [Documentação .NET 8](https://learn.microsoft.com/dotnet/core/whats-new/dotnet-8)
- [MongoDB Documentation](https://docs.mongodb.com/)
- [MediatR](https://github.com/jbogard/MediatR)
- [Swagger/OpenAPI](https://swagger.io/)

---

? Se este projeto foi útil para você, considere dar uma estrela no repositório!
