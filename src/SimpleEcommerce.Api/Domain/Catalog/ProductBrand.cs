namespace SimpleEcommerce.Api.Domain.Catalog
{
    public class ProductBrand : Entity
    {
        public string ProductId { get; set; }
        public string BrandId { get; set; }
        public Brand Brand { get; set; }
        public ProductBrand()
        {
            
        }
        public ProductBrand(string productId , string brandId)
        {
            ProductId = productId;
            BrandId = brandId;
        }

        public ProductBrand(string brandId)
        {
            BrandId = brandId;
        }
    }
}
