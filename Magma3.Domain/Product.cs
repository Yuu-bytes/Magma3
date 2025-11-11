namespace Magma3.Domain
{
    public class Product
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public decimal Price { get; set; }

        public void Update(string? name, decimal? price)
        {
            if (name != null)
            {
                Name = name;
            }

            if (price.HasValue)
            {
                Price = price.Value;
            }
        }
    }
}
