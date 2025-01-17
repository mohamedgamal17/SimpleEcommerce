namespace SimpleEcommerce.Api.Dtos.Catalog
{
    public class ProductCategoryDto : EntityDto
    {
        public string CategoryId { get; set; }
        public string ProductId { get; set; }
        public CategoryDto Category { get; set; }
    }
}
