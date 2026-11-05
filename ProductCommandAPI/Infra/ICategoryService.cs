using ProductCommandAPI.Models;

namespace ProductCommandAPI.Infra
{
	public interface ICategoryService
	{
		Task<ResponseModel> UpsertCategory(ProductCategory category,CancellationToken cancellationToken);

		Task<ResponseModel> UpsertSubCategory(ProductSubCategory subCategory, CancellationToken cancellationToken);
	}
}
