namespace ProductCommandAPI.Services.Category.CreateCategory.V2;

public record CreateCategoryRequest(Guid CategoryId, string CategoryName, string Description, int DisplayOrder, int IsActive);
public record CreateCategoryResponse(Guid CategoryId, string Message);

//{"CategoryId": "d8057768-5661-47fb-b603-291e979174ad", "CategoryName": "Test Name", "Description": "test 1234567890", "DisplayOrder": 44, "IsActive":1}
public class CreateCategoryEndpoint : ICarterModule
{
	public void AddRoutes(IEndpointRouteBuilder app)
	{
		app.MapPost("/category", async (CreateCategoryRequest request, ISender sender) =>
		{
			var command = request.Adapt<CreateCategoryCommand>();
			var result = await sender.Send(command);

			var response = result.Adapt<CreateCategoryResponse>();

			return Results.Created($"/category/{response.CategoryId}", response);
		})
			.WithName("CreateCategory")
			.Produces<CreateCategoryResponse>(StatusCodes.Status201Created)
			.ProducesProblem(StatusCodes.Status400BadRequest)
			.WithSummary("Creates a new category")
			.WithDescription("Creates a new category with the specified details.");
	}
}