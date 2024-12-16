namespace SimpleEcommerce.Api.Dtos.Catalog
{
    public class ProductDto : EntityDto
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public double Price { get; set; }
        public List<ProductCategoryDto> ProductCategories { get; set; } = new List<ProductCategoryDto>();
        public List<ProductBrandDto> ProductBrands { get; set; } = new List<ProductBrandDto>();
        public List<ProductPictureDto> Pictures { get; set; } = new List<ProductPictureDto>();
    }
}
