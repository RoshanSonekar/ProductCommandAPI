using JasperFx;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CatalogQueryService.Models
{
	[Table("Subcategories")]
	public class ProductSubCategory
	{
		[Key]
		public int SubcategoryID { get; set; }

		[ForeignKey("Category")] 
		public int CategoryId { get; set; } 
		public string SubcategoryName { get; set; } = default!;
		public int DisplayOrder { get; set; } = 0;
		public string? Description { get; set; }
		public int IsActive { get; set; } = 1;    

		// Navigation Property
		public virtual ProductCategory Category { get; set; } = default!;
	}

	public class SubCategoryWithCategoryDto
	{
		public int SubcategoryID { get; set; }
		public string SubcategoryName { get; set; } = default!;
		public string? Description { get; set; }
		public int DisplayOrder { get; set; } = 0;
		public int IsActive { get; set; } = 1;
		public int CategoryId { get; set; }
		public string CategoryName { get; set; } = default!; // Selected field
	}
}
