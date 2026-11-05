using CatalogQueryService.Models;
using Microsoft.EntityFrameworkCore;

namespace CatalogQueryService.SQLServer.DbContextEF
{
	public class CatalogDbContext
	{
		public DbSet<ProductCategory> Categories { get; set; } = default!;
		//public DbSet<ProductCommandAPI.Models.Product> Product { get; set; } = default!;
		public DbSet<ProductSubCategory> Subcategories { get; set; } = default!;
	}
}
