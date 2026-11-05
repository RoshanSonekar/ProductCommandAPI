using JasperFx;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CatalogQueryService.Models
{
	[Table("Subcategories")]
	public class ProductSubCategory
	{
		[Identity]
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int SubcategoryID { get; set; }
		public int CategoryId { get; set; } 
		public string SubcategoryName { get; set; } = default!;
		public int DisplayOrder { get; set; } = 0;
		public string? Description { get; set; }
		public int IsActive { get; set; } = 1;
	}
}
