using ProductCommandAPI.Models;

namespace ProductCommandAPI.Infra;

public class CategoryService : ICategoryService
{
	private readonly ProductCommandAPIContext context;
	public CategoryService(ProductCommandAPIContext _context)
	{
		context = _context;
	}

	// insert/update category
	public async Task<ResponseModel> UpsertCategory(ProductCategory category, CancellationToken cancellationToken)
	{
		try
		{
			if (category.CategoryId == 0)
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
		catch (Microsoft.EntityFrameworkCore.DbUpdateException)
		{
			throw;
		}
		catch
		{
			throw;
		}
	}

	// insert/update sub-category
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
		catch (Microsoft.EntityFrameworkCore.DbUpdateException)
		{
			throw;
		}
		catch
		{
			throw;
		}
	}
}
