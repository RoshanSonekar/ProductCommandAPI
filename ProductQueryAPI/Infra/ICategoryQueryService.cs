using CatalogQueryService.Models;

namespace CatalogQueryService.Infra
{
	public interface ICategoryQueryService
	{
		Task<(IEnumerable<ProductCategory>, int TotalCount)> GetAllCategoriesAsync(int? PageNumber = 1, int? PageSize = 10);
		Task<(IEnumerable<ProductCategory>, int TotalCount)> GetAllCategoriesAsync(string searchQuery, int? PageNumber = 1, int? PageSize = 10);
		Task<ProductCategory> GetCategoryByIdAsync(int categoryId);
	}
}
