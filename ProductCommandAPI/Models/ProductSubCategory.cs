using JasperFx;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductCommandAPI.Models
{
	[Table("Subcategories")]
	public class ProductSubCategory
	{
		[Identity]
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int SubcategoryID { get; set; }

		[Required]
		public int CategoryId { get; set; }

		[Required]
		public string SubcategoryName { get; set; } = default!;

		[Required]
		public int DisplayOrder { get; set; } = 0;
		public string? Description { get; set; }
		public int IsActive { get; set; } = 1;
		public DateTime UpdatedDate { get; set; } = DateTime.UtcNow;
	} 
}
