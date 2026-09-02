using Azure;
using JasperFx.Core;

namespace ProductCommandAPI.Services.Category.CreateCategory.V2;

public record CreateCategoryResult(Guid CategoryId, string Message);

public record CreateCategoryCommand(Guid CategoryId, string CategoryName, string Description, int DisplayOrder, int IsActive)
: ICommand<CreateCategoryResult>;

public class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
{
	public CreateCategoryCommandValidator()
	{
		RuleFor(x => x.CategoryName).NotEmpty().WithMessage("Category is required.");
		RuleFor(x => x.CategoryName).MaximumLength(100).WithMessage("Category lenght must not exceed 100 chars.");
		RuleFor(x => x.DisplayOrder).GreaterThan(0).WithMessage("Display Order must be greater than 0.");
		RuleFor(x => x.IsActive).InclusiveBetween(0,1).WithMessage("IsActive flag must be zero(false) or one(true).");
		RuleFor(x => x.Description).MaximumLength(250).WithMessage("Description length must not exceed 250 chars.");
	}
}

internal class CreateCategoryHandler
	(IDocumentSession session)
	: ICommandHandler<CreateCategoryCommand, CreateCategoryResult>
{
	public async Task<CreateCategoryResult> Handle(CreateCategoryCommand command, CancellationToken cancellationToken)
	{
		var result = await GetByCategory(command.CategoryName, cancellationToken);
		if (result is not null)
		{
			//var r = result.Adapt<GetByCategoryResult>();  
			return new CreateCategoryResult(result.CategoryId, "Failed: Already Exist");
		}
		// create product entity from command
		var productCategory = new Models.Category()
		{
			CategoryId = Guid.NewGuid(),
			CategoryName = command.CategoryName.Trim(),
			Description = command.Description,
			DisplayOrder = command.DisplayOrder,
			IsActive = command.IsActive
		};

		// save to db 
		session.Store(productCategory);   //session.Store(productCategory);
		await session.SaveChangesAsync(cancellationToken);

		// return createProductResult result
		return new CreateCategoryResult(productCategory.CategoryId, "Success: Created");
	}

	internal async Task<Models.Category> GetByCategory(string RequestValue, CancellationToken cancellationToken)
	{
		if(string.IsNullOrEmpty(RequestValue))
			return new Models.Category();

		Models.Category categoryResult = new Models.Category();
		categoryResult = await session.Query<Models.Category>().FirstOrDefaultAsync(x => x.CategoryName.EqualsIgnoreCase(RequestValue.Trim()), cancellationToken);
		if (categoryResult is null)
			return new Models.Category();

		return categoryResult;
	}
}