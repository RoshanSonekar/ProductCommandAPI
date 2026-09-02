using JasperFx;
using System.ComponentModel.DataAnnotations;

namespace ProductCommandAPI.Models
{
	public class Category
	{
		[Identity][Key]
		public Guid CategoryId { get; set; }
		[Required]
		public string CategoryName { get; set; } = default!;
		public int DisplayOrder { get; set; } = 99;
		public string? Description { get; set; }
		public int IsActive { get; set; } = 1;
	}
}
