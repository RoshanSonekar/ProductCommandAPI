using CatalogQueryService.SQLServer.DbContextEF;
using CatalogQueryService.Models;

namespace CatalogQueryService.Infra
{
	public class CategoryQueryService: ICategoryQueryService
	{
		private readonly CatalogDbContext dbContext;
		public CategoryQueryService(CatalogDbContext _dbContext)
		{
			dbContext = _dbContext;
		}

		public async Task<(IEnumerable<ProductCategory>, int TotalCount)> GetAllCategoriesAsync(int? PageNumber = 1, int? PageSize = 10)
		{
			throw new NotImplementedException(); 
		}

		public async Task<ProductCategory> GetCategoryByIdAsync(int categoryId)
		{
			throw new NotImplementedException();
		}
	}
}
