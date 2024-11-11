namespace SimpleEcommerce.Api.Domain.Catalog
{
    public class ProductCategory : Entity
    {
        public int ProductId { get; set; }
        public int CategoryId { get; set; }

        public ProductCategory()
        {

        }
        public ProductCategory(int productId , int categoryId)
        {
            ProductId = productId;
            CategoryId = categoryId;
        }
        public ProductCategory(int categoryId)
        {
            CategoryId = categoryId;
        }

    }
}
