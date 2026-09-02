using Azure;
using AzureServiceBus;
using JasperFx.Core;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Text.Json;

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
		Guid result = await GetByCategory(command.CategoryName, cancellationToken);
		if (result != Guid.Empty)
			return new CreateCategoryResult(result, "Failed: Already Exist");

		// create product entity from command
		var productCategory = new Models.Category()
		{
			CategoryId = Guid.NewGuid(),
			CategoryName = command.CategoryName.Trim(),
			Description = command.Description,
			DisplayOrder = command.DisplayOrder,
			IsActive = command.IsActive
		};
		string responseMessage = "Success: Created";

		// save to db 
		session.Store(productCategory);    
		await session.SaveChangesAsync(cancellationToken);

		// can be injectd using builder pattern, but for now we will read from appsettings.json
		var config = new ConfigurationBuilder()
		.SetBasePath(Directory.GetCurrentDirectory())
		.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
		.Build();

		// more validations can be added here, but for now we will just check if the send is enabled and send to the bus
		if (config is not null)
		{
			if (config["AzureServiceBus:SendEnabled"] == "true")
			{
				BusRequest busRequest = new BusRequest
				{
					NameSpace = config["AzureServiceBus:NameSpace"]!,
					QueueName = config["AzureServiceBus:QueueName"]!,
					Message =  JsonSerializer.Serialize(productCategory)
				};
				// send to the bus
				await Sender.SendMessage(busRequest);
				responseMessage = string.Concat(responseMessage, " and published to the AZ Bus");
			}
		}
		// return createProductResult result
		return new CreateCategoryResult(productCategory.CategoryId, responseMessage);
	}

	internal async Task<Guid> GetByCategory(string RequestValue, CancellationToken cancellationToken)
	{
		if (string.IsNullOrEmpty(RequestValue))
			return Guid.Empty;

		Models.Category categoryResult = new Models.Category();
		categoryResult = await session.Query<Models.Category>().FirstOrDefaultAsync(x => x.CategoryName.EqualsIgnoreCase(RequestValue.Trim()), cancellationToken);

		if (categoryResult is null)
			return Guid.Empty;

		return categoryResult.CategoryId;
	}
}