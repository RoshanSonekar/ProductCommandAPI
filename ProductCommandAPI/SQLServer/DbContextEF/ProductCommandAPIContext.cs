using Microsoft.EntityFrameworkCore;

public class ProductCommandAPIContext(DbContextOptions<ProductCommandAPIContext> options) : DbContext(options)
{
    public DbSet<ProductCommandAPI.Models.ProductCategory> ProductCategory { get; set; } = default!;
    public DbSet<ProductCommandAPI.Models.Product> Product { get; set; } = default!;
	public DbSet<ProductCommandAPI.Models.ProductSubCategory> ProductSubCategory { get; set; } = default!;
}
