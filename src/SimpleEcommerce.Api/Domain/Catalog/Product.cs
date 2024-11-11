namespace SimpleEcommerce.Api.Domain.Catalog
{
    public class Product : Entity
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public double Price { get; set; }
        public List<ProductCategory> ProductCategories { get; set; }
        public List<ProductBrand> ProductBrands { get; set; }
    }
}
