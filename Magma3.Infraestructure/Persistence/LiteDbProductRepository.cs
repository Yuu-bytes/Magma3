using LiteDB;
using Magma3.Domain;
using Magma3.Infraestructure.Interfaces;

namespace Magma3.Infraestructure.Persistence
{
    public class LiteDbProductRepository : IProductRepository
    {
        private readonly LiteDatabase _database;
        private readonly ILiteCollection<Product> _collection;

        public LiteDbProductRepository(string databasePath)
        {
            _database = new LiteDatabase(databasePath);
            _collection = _database.GetCollection<Product>("products");

            _collection.EnsureIndex(x => x.Id);
        }

        public async Task<Product?> GetByIdAsync(int id)
        {
            return await Task.FromResult(_collection.FindById(id));
        }

        public async Task<List<Product>> GetAllAsync()
        {
            return await Task.FromResult(_collection.FindAll().ToList());
        }

        public async Task<Product> CreateAsync(Product product)
        {
            if (product.Id == 0)
            {
                var id = _collection.Insert(product);
                product.Id = id.AsInt32;
            }
            else
            {
                _collection.Insert(product);
            }

            return await Task.FromResult(product);
        }

        public async Task<Product> UpdateAsync(Product product)
        {
            var updated = _collection.Update(product);
            if (!updated)
            {
                throw new InvalidOperationException($"Product with ID {product.Id} not found");
            }

            return await Task.FromResult(product);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var deleted = _collection.Delete(id);
            return await Task.FromResult(deleted);
        }

        public void Dispose()
        {
            _database?.Dispose();
        }
    }
}
