using CatalogQueryService.Models;
using CatalogQueryService.SQLServer.DbContextEF;

namespace CatalogQueryService.Infra
{
	public class SubCategoryQueryService : ISubCategoryQueryService
	{
		private readonly CatalogDbContext dbContext;
		public SubCategoryQueryService(CatalogDbContext _dbContext)
		{
			dbContext = _dbContext;
		}

		public async Task<(IEnumerable<ProductSubCategory>, int TotalCount)> GetAllSubCategoriesAsync(int? PageNumber = 1, int? PageSize = 10)
		{
			throw new NotImplementedException();
		}
		public async Task<ProductSubCategory> GetSubCategoryByIdAsync(int subCategoryId)
		{
			throw new NotImplementedException();
		}
		public async Task<(IEnumerable<ProductSubCategory>, int TotalCount)> GetSubCategoriesByCategoryIdAsync(int categoryId, int? PageNumber = 1, int? PageSize = 10)
		{
			throw new NotImplementedException();
		}
	}
}
