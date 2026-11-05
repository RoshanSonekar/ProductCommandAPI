using JasperFx;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductCommandAPI.Models
{
	[Table("Categories")]
	public class ProductCategory
	{
		[Identity]
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int CategoryId { get; set; }

		[Required]
		public string CategoryName { get; set; } = default!;
		public int DisplayOrder { get; set; } = 0;
		public string? Description { get; set; }
		public int IsActive { get; set; } = 1;
	}
}
