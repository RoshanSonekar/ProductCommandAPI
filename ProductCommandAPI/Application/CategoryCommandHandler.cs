using ProductCommandAPI.Application;
using BuildingBlocks.CQRS; 
using FluentValidation;
using ProductCommandAPI.Infra;
using ProductCommandAPI.Models;

namespace ProductCommandAPI.Application;
	public record CategoryCommand(string CategoryName, string Description, int DisplayOrder)
	: ICommand<CategoryResult>;
	public record CategoryUpdateCommand(int CategoryId, string CategoryName, string Description, int DisplayOrder)
		: ICommand<CategoryResult>;
	public record CategoryResult(int CategoryId, bool IsSuccess);

public class CategoryCommandValidator : AbstractValidator<CategoryCommand>
{
	public CategoryCommandValidator()
	{
		RuleFor(x => x.CategoryName).NotEmpty().WithMessage("Category name is required.");
		RuleFor(x => x.DisplayOrder).GreaterThanOrEqualTo(0).WithMessage("Display order must be a non-negative integer.");
	}
}

public class CategoryUpdateCommandValidator : AbstractValidator<CategoryUpdateCommand>
{
	public CategoryUpdateCommandValidator()
	{
		RuleFor(x => x.CategoryName).NotEmpty().WithMessage("Category name is required.");
		RuleFor(x => x.DisplayOrder).GreaterThanOrEqualTo(0).WithMessage("Display order must be a non-negative integer.");
	}
}

public class CategoryCommandHandler(ICategoryService categoryService)
		: ICommandHandler<CategoryCommand, CategoryResult>
{
	public async Task<CategoryResult> Handle(CategoryCommand command, CancellationToken cancellationToken)
	{
		var category = new ProductCategory
		{
			CategoryName = command.CategoryName,
			Description = command.Description,
			DisplayOrder = command.DisplayOrder
		};

		await categoryService.UpsertCategory(category, cancellationToken);
		return new CategoryResult(category.CategoryId, true);
	}
}
public class CategoryUpdateCommandHandler(ICategoryService categoryService)
	: ICommandHandler<CategoryUpdateCommand, CategoryResult>
{
	public async Task<CategoryResult> Handle(CategoryUpdateCommand command, CancellationToken cancellationToken)
	{
		var category = new ProductCategory
		{
			CategoryId = command.CategoryId,
			CategoryName = command.CategoryName,
			Description = command.Description,
			DisplayOrder = command.DisplayOrder
		};

		await categoryService.UpsertCategory(category, cancellationToken);
		return new CategoryResult(category.CategoryId, true);
	}
}
