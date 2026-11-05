using CatalogQueryService.Models;

namespace CatalogQueryService.Infra
{ 
	public interface ISubCategoryQueryService
	{
		Task<(IEnumerable<ProductSubCategory>, int TotalCount)> GetAllSubCategoriesAsync(int? PageNumber = 1, int? PageSize = 10);
		Task<ProductSubCategory> GetSubCategoryByIdAsync(int subCategoryId);
		Task<(IEnumerable<ProductSubCategory>, int TotalCount)> GetSubCategoriesByCategoryIdAsync(int categoryId, int? PageNumber = 1, int? PageSize = 10);
	}
}
