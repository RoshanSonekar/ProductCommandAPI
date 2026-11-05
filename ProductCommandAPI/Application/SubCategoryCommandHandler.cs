using ProductCommandAPI.Infra;
using ProductCommandAPI.Models;

namespace ProductCommandAPI.Application;

public record SubCategoryCommand(int CategoryId, string SubCategoryName, string Description, int DisplayOrder)
	: ICommand<SubCategoryResult>;
public record SubCategoryUpdateCommand(int SubCategoryId, int CategoryId, string SubCategoryName, string Description, int DisplayOrder)
	: ICommand<SubCategoryResult>;
public record SubCategoryResult(int SubCategoryId, bool IsSuccess);

public class SubCategoryCommandValidator : AbstractValidator<SubCategoryCommand>
{
	public SubCategoryCommandValidator()
	{
		RuleFor(x => x.SubCategoryName).NotEmpty().WithMessage("SubCategory name is required.");
		RuleFor(x => x.CategoryId).NotEmpty().WithMessage("Category ID is required.");
		RuleFor(x => x.DisplayOrder).GreaterThanOrEqualTo(0).WithMessage("Display order must be a non-negative integer.");
	}
}

public class SubCategoryUpdateCommandValidator : AbstractValidator<SubCategoryUpdateCommand>
{
	public SubCategoryUpdateCommandValidator()
	{
		RuleFor(x => x.SubCategoryName).NotEmpty().WithMessage("SubCategory name is required.");
		RuleFor(x => x.CategoryId).NotEmpty().WithMessage("Category ID is required.");
		RuleFor(x => x.DisplayOrder).GreaterThanOrEqualTo(0).WithMessage("Display order must be a non-negative integer.");
	}
}

public class SubCategoryCommandHandler(ICategoryService categoryService)
		: ICommandHandler<SubCategoryCommand, SubCategoryResult>
{
	public async Task<SubCategoryResult> Handle(SubCategoryCommand command, CancellationToken cancellationToken)
	{
		var subCategory = new ProductSubCategory	
		{
			SubcategoryName = command.SubCategoryName,
			CategoryId = command.CategoryId,
			Description = command.Description,
			DisplayOrder = command.DisplayOrder
		};

		await categoryService.UpsertSubCategory(subCategory, cancellationToken);
		return new SubCategoryResult(subCategory.SubcategoryID, true);
	}
	}

public class SubCategoryUpdateCommandHandler(ICategoryService categoryService)
	: ICommandHandler<SubCategoryUpdateCommand, SubCategoryResult>
{

	public async Task<SubCategoryResult> Handle(SubCategoryUpdateCommand command, CancellationToken cancellationToken)
	{
		var subCategory = new ProductSubCategory
		{
			SubcategoryID = command.SubCategoryId,		
			SubcategoryName = command.SubCategoryName,
			CategoryId = command.CategoryId,
			Description = command.Description,
			DisplayOrder = command.DisplayOrder
		};

		await categoryService.UpsertSubCategory(subCategory, cancellationToken);
		return new SubCategoryResult(subCategory.SubcategoryID, true);
	}
}
