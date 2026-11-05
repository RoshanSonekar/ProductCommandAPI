using ProductCommandAPI.Models;

namespace ProductCommandAPI.Infra
{
	public interface IProductService
	{
		Task<ResponseModel> CreateProduct(ProductCommandAPI.Models.Product product);
		Task<ResponseModel> UpdateProduct(ProductCommandAPI.Models.Product product);
	}
}
