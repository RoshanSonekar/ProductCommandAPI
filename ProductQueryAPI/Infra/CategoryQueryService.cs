using CatalogQueryService.Models;
using CatalogQueryService.SQLServer.DbContextEF;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System.Linq.Dynamic.Core;

namespace CatalogQueryService.Infra
{
	public class CategoryQueryService : ICategoryQueryService
	{
		private readonly CatalogDbContext dbContext;
		public CategoryQueryService(CatalogDbContext _dbContext)
		{
			dbContext = _dbContext;
		}

		public async Task<(IEnumerable<ProductCategory>, int TotalCount)> GetAllCategoriesAsync(int? PageNumber = 1, int? PageSize = 10)
		{
			int actualPage = PageNumber ?? 1;
			int actualSize = PageSize ?? 10;
			if (actualPage < 1) actualPage = 1;
			if (actualSize < 1) actualSize = 10;

			int totalCount = await dbContext.Categories.CountAsync();

			IEnumerable<ProductCategory>? pagedCategories = await dbContext.Categories
					.AsNoTracking()
					.OrderBy(c => c.CategoryId)
					.Skip((actualPage - 1) * actualSize)
					.Take(actualSize)
					.Select(c => new ProductCategory
					{
						CategoryId = c.CategoryId,
						CategoryName = c.CategoryName,
						Description = c.Description,
						DisplayOrder = c.DisplayOrder,
						IsActive = Convert.ToInt16(c.IsActive)
					})
					.ToListAsync();

			return (pagedCategories, totalCount);
		}

		public async Task<ProductCategory> GetCategoryByIdAsync(int categoryId)
		{
			ProductCategory? category = await dbContext.Categories
					.AsNoTracking()
					.Where(c => c.CategoryId == categoryId)
					.Select(c => new ProductCategory
					{
						CategoryId = c.CategoryId,
						CategoryName = c.CategoryName,
						Description = c.Description,
						DisplayOrder = c.DisplayOrder,
						IsActive = Convert.ToInt16(c.IsActive)
					})
					.FirstOrDefaultAsync() ?? new ProductCategory();

			return category;
		}

		public async Task<(IEnumerable<ProductCategory>, int TotalCount)> GetAllCategoriesAsync(string searchQuery, int? PageNumber, int? PageSize)
		{
			int actualPage = Math.Max(PageNumber ?? 1, 1);
			int actualSize = Math.Max(PageSize ?? 10, 1);

			var query = dbContext.Categories.AsNoTracking();

			if (!string.IsNullOrWhiteSpace(searchQuery))
			{
				query = query.Where(c => c.CategoryName.Contains(searchQuery));
			}

			var result = await query
					.OrderBy(c => c.CategoryId)
					.Select(c => new
					{
						TotalCount = query.Count(),
						Data = new ProductCategory
						{
							CategoryId = c.CategoryId,
							CategoryName = c.CategoryName,
							Description = c.Description,
							DisplayOrder = c.DisplayOrder,
							IsActive = Convert.ToInt16(c.IsActive)
						}
					})
					.Skip((actualPage - 1) * actualSize)
					.Take(actualSize)
					.ToListAsync();

			int totalCount = result.FirstOrDefault()?.TotalCount ?? 0;
			var pagedCategories = result.Select(r => r.Data);

			return (pagedCategories, totalCount);
		}
	}
}