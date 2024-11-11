namespace SimpleEcommerce.Api.Models.Catalog
{
    public class ProductModel
    {
        public string  Name { get; set; }
        public string? Description { get; set; }
        public double Price { get; set; }
        public List<string>? Categories { get; set; }
        public List<string>? Brands { get; set; }
    }
}
