using Microsoft.EntityFrameworkCore;

public class ProductCommandAPIContext(DbContextOptions<ProductCommandAPIContext> options) : DbContext(options)
{
    public DbSet<ProductCommandAPI.Models.Category> Category { get; set; } = default!;
}
