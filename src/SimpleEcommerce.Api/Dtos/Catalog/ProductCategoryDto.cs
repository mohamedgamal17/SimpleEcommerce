namespace SimpleEcommerce.Api.Dtos.Catalog
{
    public class ProductCategoryDto : EntityDto
    {
        public int CategoryId { get; set; }
        public int ProductId { get; set; }
        public CategoryDto Category { get; set; }
    }
}
