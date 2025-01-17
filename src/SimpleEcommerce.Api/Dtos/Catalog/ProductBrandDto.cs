namespace SimpleEcommerce.Api.Dtos.Catalog
{
    public class ProductBrandDto : EntityDto
    {
        public int BrandId { get; set; }
        public string ProductId { get; set; }
        public BrandDto Brand { get; set; }
    }
}
