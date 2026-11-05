using JasperFx;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductCommandAPI.Models
{
	[Table("Products")]
	public class Product
	{
		[Identity]
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Matches SQL Server's IDENTITY(1,1)
		public int ProductID { get; set; }

		[Required]
		public int SubCategoryID { get; set; }  

		[Required]
		[MaxLength(50)]
		public string SKU { get; set; } = string.Empty;

		[Required]
		[MaxLength(200)]
		public string ProductName { get; set; } = string.Empty;

		public string? Description { get; set; }
		public int DisplayOrder { get; set; } = 0;

		[Required]
		public decimal Price { get; set; }

		[Required]
		public int StockQuantity { get; set; } = 0;

		[MaxLength(2048)]
		public string? PrimaryImageURL { get; set; }

		public byte[]? PrimaryImage { get; set; }  

		[MaxLength(50)]
		public string? PrimaryImageMimeType { get; set; }

		public bool IsActive { get; set; } = true; 

		public DateTime? UpdatedDate { get; set; }
	}
}
