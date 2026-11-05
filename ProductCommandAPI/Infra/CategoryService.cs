using ProductCommandAPI.Models;

namespace ProductCommandAPI.Infra;
public class CategoryService : ICategoryService
{
	private readonly ProductCommandAPIContext context;
	public CategoryService(ProductCommandAPIContext _context)
	{
		context = _context;
	}
	public async Task<ResponseModel> UpsertCategory(ProductCategory category, CancellationToken cancellationToken)
	{
		try
		{
			if(category.CategoryId == 0)
				context.Add(category);
			else
				context.Update(category);

			await context.SaveChangesAsync(cancellationToken);

			return new ResponseModel
			{
				Id = category.CategoryId,  
				IsSuccess = true
			};
		}
		catch (Exception ex)
		{
			// For production APIs, consider logging 'ex' here instead of throwing or swallowing it entirely
			return new ResponseModel
			{
				Id = 0,
				IsSuccess = false
			};
		}
	} 

	public async Task<ResponseModel> UpsertSubCategory(ProductSubCategory subCategory, CancellationToken cancellationToken)
	{
		try
		{
			if (subCategory.SubcategoryID == 0)
				context.Add(subCategory);
			else
				context.Update(subCategory);

			await context.SaveChangesAsync(cancellationToken);

			return new ResponseModel
			{
				Id = subCategory.SubcategoryID,
				IsSuccess = true
			};
		}
		catch (Exception ex)
		{
			// For production APIs, consider logging 'ex' here instead of throwing or swallowing it entirely
			return new ResponseModel
			{	
				Id = 0,
				IsSuccess = false
			};
		}
	}
}
