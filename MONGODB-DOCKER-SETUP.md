# Instruções para usar MongoDB com Docker

## Iniciando o MongoDB no Docker

### 1. Iniciar o container do MongoDB
```bash
docker-compose up -d
```

Este comando irá:
- Baixar a imagem do MongoDB (se ainda não estiver baixada)
- Criar e iniciar o container do MongoDB na porta 27017 **com autenticação**
- Criar e iniciar o MongoDB Express (interface web) na porta 8081

### 2. Verificar se os containers estão rodando
```bash
docker-compose ps
```

### 3. Parar os containers
```bash
docker-compose down
```

### 4. Parar e remover volumes (apaga os dados)
```bash
docker-compose down -v
```

## Credenciais do MongoDB

### Usuário e Senha
- **Usuário**: `magma3admin`
- **Senha**: `Magma3Pass@2024`
- **Banco de Dados**: `magma3db`

?? **IMPORTANTE**: Em produção, altere essas credenciais e utilize variáveis de ambiente ou secrets!

## Configuração da Aplicação

### Desenvolvimento Local (app fora do Docker, MongoDB no Docker)
Quando você rodar a aplicação localmente (F5 no Visual Studio), ela usará `appsettings.Development.json`:
```json
"MongoDB": {
  "ConnectionString": "mongodb://magma3admin:Magma3Pass@2024@localhost:27017",
  "DatabaseName": "magma3db"
}
```

### Produção ou app em container
Se você colocar a aplicação em um container Docker também, use `appsettings.json`:
```json
"MongoDB": {
  "ConnectionString": "mongodb://magma3admin:Magma3Pass@2024@mongodb:27017",
  "DatabaseName": "magma3db"
}
```

## MongoDB Express (Interface Web)

Após iniciar os containers, acesse:
- **URL**: http://localhost:8081
- **Login**: Automático (credenciais configuradas no Docker)
- Interface visual para gerenciar o banco MongoDB

## Comandos Úteis do Docker

### Ver logs do MongoDB
```bash
docker logs magma3-mongodb
```

### Acessar o shell do MongoDB
```bash
docker exec -it magma3-mongodb mongosh -u magma3admin -p Magma3Pass@2024
```

### Dentro do mongosh:
```javascript
// Usar o banco de dados
use magma3db

// Ver coleções
show collections

// Ver documentos da coleção clientes
db.clientes.find()

// Contar documentos
db.clientes.countDocuments()

// Criar um usuário específico para a aplicação (opcional)
db.createUser({
  user: "magma3app",
  pwd: "AppPass@2024",
  roles: [{ role: "readWrite", db: "magma3db" }]
})
```

## Segurança em Produção

### Usar variáveis de ambiente
Crie um arquivo `.env` na raiz do projeto (adicione ao `.gitignore`):
```env
MONGO_ROOT_USERNAME=magma3admin
MONGO_ROOT_PASSWORD=SuaSenhaSegura123!
MONGO_DATABASE=magma3db
```

Atualize o `docker-compose.yml`:
```yaml
environment:
  MONGO_INITDB_ROOT_USERNAME: ${MONGO_ROOT_USERNAME}
  MONGO_INITDB_ROOT_PASSWORD: ${MONGO_ROOT_PASSWORD}
  MONGO_INITDB_DATABASE: ${MONGO_DATABASE}
```

E o `appsettings.json`:
```json
"MongoDB": {
  "ConnectionString": "mongodb://${MONGO_ROOT_USERNAME}:${MONGO_ROOT_PASSWORD}@mongodb:27017",
  "DatabaseName": "${MONGO_DATABASE}"
}
```

### Azure Key Vault / AWS Secrets Manager
Para produção, considere usar serviços de gerenciamento de secrets:
- Azure Key Vault
- AWS Secrets Manager
- HashiCorp Vault

## Troubleshooting

### Erro de autenticação
Se receber erro "Authentication failed":
1. Certifique-se de que as credenciais estão corretas
2. Recrie os containers: `docker-compose down -v && docker-compose up -d`
3. Aguarde alguns segundos para o MongoDB inicializar

### Porta 27017 já está em uso
Se você já tem MongoDB instalado localmente, pare o serviço local ou mude a porta no docker-compose.yml:
```yaml
ports:
  - "27018:27017"  # Usa porta 27018 externamente
```
E atualize o appsettings.Development.json:
```json
"ConnectionString": "mongodb://magma3admin:Magma3Pass@2024@localhost:27018"
```

### Resetar banco de dados e credenciais
```bash
docker-compose down -v
docker-compose up -d
```

## Volumes

Os dados do MongoDB são persistidos em volumes Docker:
- `mongodb_data`: Dados do banco
- `mongodb_config`: Configurações do MongoDB

Mesmo parando os containers, os dados permanecem. Use `docker-compose down -v` para removê-los.

## Connection String - Formato

O formato da connection string do MongoDB com autenticação é:
```
mongodb://[username]:[password]@[host]:[port]/[database]?[options]
```

Exemplo:
```
mongodb://magma3admin:Magma3Pass@2024@localhost:27017/magma3db
```

### Caracteres especiais na senha
Se sua senha contém caracteres especiais (@, :, /, etc.), você precisa fazer URL encoding:
- `@` = `%40`
- `:` = `%3A`
- `/` = `%2F`

Exemplo com senha `Pass@123`:
```
mongodb://magma3admin:Pass%40123@localhost:27017
