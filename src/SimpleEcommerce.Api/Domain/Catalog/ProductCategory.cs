namespace SimpleEcommerce.Api.Domain.Catalog
{
    public class ProductCategory : Entity
    {
        public string ProductId { get; set; }
        public string CategoryId { get; set; }

        public Category Category { get; set; }
        public ProductCategory()
        {

        }
        public ProductCategory(string productId , string categoryId)
        {
            ProductId = productId;
            CategoryId = categoryId;
        }
        public ProductCategory(string categoryId)
        {
            CategoryId = categoryId;
        }

    }
}
