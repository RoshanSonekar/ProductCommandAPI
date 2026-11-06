using JasperFx;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CatalogQueryService.Models
{
	[Table("Categories")]
	public class ProductCategory
	{
		[Key]
		public int CategoryId { get; set; }
		public string CategoryName { get; set; } = default!;
		public int DisplayOrder { get; set; } = 0;
		public string? Description { get; set; }
		public int IsActive { get; set; } = 1;
		public DateTime UpdatedDate { get; set; } = DateTime.UtcNow;
	}
}
