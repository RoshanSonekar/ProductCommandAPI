using CatalogQueryService.Models;

namespace CatalogQueryService.Infra
{ 
	public interface ISubCategoryQueryService
	{
		Task<(IEnumerable<SubCategoryWithCategoryDto>, int TotalCount)> GetAllSubCategoriesAsync(int? PageNumber = 1, int? PageSize = 10);
		Task<SubCategoryWithCategoryDto> GetSubCategoryByIdAsync(int subCategoryId);
		Task<(ProductCategory productCategory, IEnumerable<ProductSubCategory>, int TotalCount)> GetSubCategoriesByCategoryIdAsync(int categoryId, int? PageNumber = 1, int? PageSize = 10);
	}
}
