using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Magma3.Domain;
using Magma3.Infraestructure.Interfaces;

namespace Magma3.Infraestructure.Persistence
{
    // não usei o IUnitOfWork aqui porque nunca mexi com mongo e já tinha implementado o LiteDB
    public class MongoDbClienteRepository : IClienteRepository
    {
        private readonly IMongoCollection<MongoCliente> _collection;

        public MongoDbClienteRepository(string connectionString, string databaseName)
        {
            var client = new MongoClient(connectionString);
            var database = client.GetDatabase(databaseName);
            _collection = database.GetCollection<MongoCliente>("clientes");

            var indexKeysDefinition = Builders<MongoCliente>.IndexKeys.Ascending(x => x.Id);
            var indexModel = new CreateIndexModel<MongoCliente>(indexKeysDefinition);
            _collection.Indexes.CreateOne(indexModel);

            var emailIndexKeys = Builders<MongoCliente>.IndexKeys.Ascending(x => x.Email);
            var emailIndexOptions = new CreateIndexOptions { Unique = true, Sparse = true };
            var emailIndexModel = new CreateIndexModel<MongoCliente>(emailIndexKeys, emailIndexOptions);
            _collection.Indexes.CreateOne(emailIndexModel);
        }

        public async Task<Cliente?> GetByIdAsync(int id)
        {
            var filter = Builders<MongoCliente>.Filter.Eq(x => x.Id, id);
            var mongoCliente = await _collection.Find(filter).FirstOrDefaultAsync();
            return mongoCliente?.ToCliente();
        }

        public async Task<List<Cliente>> GetAllAsync()
        {
            var mongoClientes = await _collection.Find(_ => true).ToListAsync();
            return mongoClientes.Select(mc => mc.ToCliente()).ToList();
        }

        public async Task<Cliente> CreateAsync(Cliente cliente)
        {
            var mongoCliente = MongoCliente.FromCliente(cliente);

            if (mongoCliente.Id == 0)
            {
                var maxId = await GetMaxIdAsync();
                mongoCliente.Id = maxId + 1;
            }

            if (mongoCliente.DataCadastro == default)
            {
                mongoCliente.DataCadastro = DateTime.UtcNow;
            }

            await _collection.InsertOneAsync(mongoCliente);
            cliente.Id = mongoCliente.Id;
            cliente.DataCadastro = mongoCliente.DataCadastro;
            return cliente;
        }

        public async Task<Cliente> UpdateAsync(Cliente cliente)
        {
            var mongoCliente = MongoCliente.FromCliente(cliente);
            var filter = Builders<MongoCliente>.Filter.Eq(x => x.Id, cliente.Id);
            var result = await _collection.ReplaceOneAsync(filter, mongoCliente);

            if (result.MatchedCount == 0)
            {
                throw new InvalidOperationException($"Cliente with ID {cliente.Id} not found");
            }

            return cliente;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var filter = Builders<MongoCliente>.Filter.Eq(x => x.Id, id);
            var result = await _collection.DeleteOneAsync(filter);
            return result.DeletedCount > 0;
        }

        private async Task<int> GetMaxIdAsync()
        {
            var lastCliente = await _collection
                      .Find(_ => true)
               .SortByDescending(x => x.Id)
            .Limit(1)
            .FirstOrDefaultAsync();

            return lastCliente?.Id ?? 0;
        }

        private class MongoCliente // isso daqui deveria ir separado para uma pasta de map, mas nunca mexi com mongo, então nem vou mexer muito
        {
            [BsonId]
            [BsonRepresentation(BsonType.ObjectId)]
            public string? ObjectId { get; set; }

            [BsonElement("id")]
            public int Id { get; set; }

            [BsonElement("nome")]
            public string? Nome { get; set; }

            [BsonElement("email")]
            public string? Email { get; set; }

            [BsonElement("telefone")]
            public string? Telefone { get; set; }

            [BsonElement("cpf")]
            public string? Cpf { get; set; }

            [BsonElement("dataCadastro")]
            [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
            public DateTime DataCadastro { get; set; }

            [BsonElement("ativo")]
            public bool Ativo { get; set; }

            public static MongoCliente FromCliente(Cliente cliente)
            {
                return new MongoCliente
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

            public Cliente ToCliente()
            {
                return new Cliente
                {
                    Id = Id,
                    Nome = Nome,
                    Email = Email,
                    Telefone = Telefone,
                    Cpf = Cpf,
                    DataCadastro = DataCadastro,
                    Ativo = Ativo
                };
            }
        }
    }
}
