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
					.ToListAsync(); // Guaranteed to return an empty list [] if no records found

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
							SubcategoryID = sub.SubcategoryID,
							CategoryId = sub.CategoryId,
							SubcategoryName = sub.SubcategoryName,
							DisplayOrder = sub.DisplayOrder,
							Description = sub.Description,
							CategoryName = cat.CategoryName,
							IsActive = Convert.ToInt16( sub.IsActive)
						})
				.Where(sc => sc.SubcategoryID == subCategoryId)
				.FirstOrDefaultAsync() ?? new SubCategoryWithCategoryDto();

		//	var result = await dbContext.Subcategories
		//.AsNoTracking()
		//.Where(s => s.SubcategoryID == subcategoryId)
		//.Join(dbContext.Categories,
		//		sub => sub.CategoryId,
		//		cat => cat.CategoryId,
		//		(sub, cat) => new
		//		{
		//			Subcategory = sub,
		//			Category = cat
		//		})
		//.FirstOrDefaultAsync();

			return subCategory;
		}
		public async Task<(IEnumerable<SubCategoryWithCategoryDto>, int TotalCount)> GetSubCategoriesByCategoryIdAsync(int categoryId, int? PageNumber = 1, int? PageSize = 10)
		{
			throw new NotImplementedException();
		}
	}
}
