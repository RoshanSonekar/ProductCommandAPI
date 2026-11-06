using CatalogQueryService.Models;
using CatalogQueryService.SQLServer.DbContextEF;
using Microsoft.EntityFrameworkCore;

namespace CatalogQueryService.Infra
{
	public class SubCategoryQueryService : ISubCategoryQueryService
	{
		private readonly CatalogDbContext dbContext;
		public SubCategoryQueryService(CatalogDbContext _dbContext)
		{
			dbContext = _dbContext;
		}

		public async Task<(IEnumerable<SubCategoryWithCategoryDto>, int TotalCount)> GetAllSubCategoriesAsync(int? PageNumber = 1, int? PageSize = 10)
		{
			int actualPage = PageNumber ?? 1;
			int actualSize = PageSize ?? 10;
			if (actualPage < 1) actualPage = 1;
			if (actualSize < 1) actualSize = 10;

			int totalCount = await dbContext.Subcategories.CountAsync();

			IEnumerable<SubCategoryWithCategoryDto>? pagedSubCategories = await dbContext.Subcategories
					.AsNoTracking()
					.Join(dbContext.Categories,
							sub => sub.CategoryId,
							cat => cat.CategoryId,
							(sub, cat) => new SubCategoryWithCategoryDto
							{
								SubcategoryID = sub.SubcategoryID,
								CategoryId = sub.CategoryId,
								SubcategoryName = sub.SubcategoryName,
								DisplayOrder = sub.DisplayOrder,
								Description = sub.Description,
								CategoryName = cat.CategoryName,
								IsActive = Convert.ToInt16(sub.IsActive)
							})
					.OrderBy(sc => sc.SubcategoryID)
					.Skip((actualPage - 1) * actualSize)
					.Take(actualSize)
					.ToListAsync();  

			return (pagedSubCategories, totalCount);
		}

		public async Task<SubCategoryWithCategoryDto> GetSubCategoryByIdAsync(int subCategoryId)
		{
			SubCategoryWithCategoryDto? subCategory = await dbContext.Subcategories
			 .AsNoTracking()
				.Join(dbContext.Categories,
						sub => sub.CategoryId,
						cat => cat.CategoryId,
						(sub, cat) => new SubCategoryWithCategoryDto
						{
							CategoryId = sub.CategoryId,
							CategoryName = cat.CategoryName,
							SubcategoryID = sub.SubcategoryID,
							SubcategoryName = sub.SubcategoryName,
							DisplayOrder = sub.DisplayOrder,
							Description = sub.Description,
							IsActive = Convert.ToInt16(sub.IsActive)
						})
				.Where(sc => sc.SubcategoryID == subCategoryId)
				.FirstOrDefaultAsync() ?? new SubCategoryWithCategoryDto();

			return subCategory;
		}
		public async Task<(ProductCategory productCategory, IEnumerable<ProductSubCategory>, int TotalCount)> GetSubCategoriesByCategoryIdAsync(int categoryId, int? PageNumber = 1, int? PageSize = 10)
		{
			// fetch the category details first
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

			// If the category is not found, return an empty list of subcategories and total count as 0
			if (category.CategoryId == 0)
				return (category, Enumerable.Empty<ProductSubCategory>(), 0);

			int actualPage = PageNumber ?? 1;
			int actualSize = PageSize ?? 10;
			if (actualPage < 1) actualPage = 1;
			if (actualSize < 1) actualSize = 10;

			int totalCount = await dbContext.Subcategories.CountAsync();

			IEnumerable<ProductSubCategory>? pagedSubCategories = await dbContext.Subcategories
				.AsNoTracking()
				.OrderBy(c => c.CategoryId)
				.Skip((actualPage - 1) * actualSize)
				.Take(actualSize)
				.Select(c => new ProductSubCategory
				{
					SubcategoryID = c.SubcategoryID,
					SubcategoryName = c.SubcategoryName,
					Description = c.Description,
					DisplayOrder = c.DisplayOrder,
					IsActive = Convert.ToInt16(c.IsActive)
				})
				.ToListAsync();

			return (category, pagedSubCategories, totalCount);
		}
	}
}
