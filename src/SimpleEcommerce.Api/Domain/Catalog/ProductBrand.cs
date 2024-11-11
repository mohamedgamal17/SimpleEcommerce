namespace SimpleEcommerce.Api.Domain.Catalog
{
    public class ProductBrand : Entity
    {
        public int ProductId { get; set; }
        public int BrandId { get; set; }
        public Brand Brand { get; set; }
        public ProductBrand()
        {
            
        }
        public ProductBrand(int productId , int brandId)
        {
            ProductId = productId;
            BrandId = brandId;
        }

        public ProductBrand(int brandId)
        {
            BrandId = brandId;
        }
    }
}
