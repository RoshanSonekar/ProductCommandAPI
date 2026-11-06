using BuildingBlocks.CQRS;
using CatalogQueryService.Infra;
using CatalogQueryService.Models;
using FluentValidation;
using System.Text.Json.Serialization;

namespace CatalogQueryService.Application;

#region --- get all categories --- 
public record CategoriesQuery(int? PageNumber = 1, int? PageSize = 10)
		: IQuery<GetCategoriesResult>;

	public record GetCategoriesResult
	{
		[JsonPropertyName("categoryEntities")]
		public IEnumerable<ProductCategory> categoryEntities { get; init; }

		[JsonPropertyName("totalCount")]
		public int TotalCount { get; init; }

		public GetCategoriesResult(IEnumerable<ProductCategory> _categoryEntities, int totalCount)
		{
			categoryEntities = _categoryEntities;
			TotalCount = totalCount;
		}
	}

	public class CategoriesQueryHandler(ICategoryQueryService categoryQueryService)
		:IQueryHandler<CategoriesQuery, GetCategoriesResult>
	{
			public async Task<GetCategoriesResult> Handle(CategoriesQuery query, CancellationToken cancellationToken)
			{
				int pageNumber = query.PageNumber ?? 1;
				int pageSize = query.PageSize ?? 10;

				var (categories, totalCount) = await categoryQueryService.GetAllCategoriesAsync(
					pageNumber,
					pageSize
				);

				return new GetCategoriesResult(categories, totalCount);
			} 
	}
#endregion

#region --- get category by id ---
public record CategoryByIdQuery(int CategoryId)
		: IQuery<CategoryByIdQueryResult>;
public record CategoryByIdQueryResult(ProductCategory Category);

public class CategoryQueryValidator : AbstractValidator<CategoryByIdQuery>
{
	public CategoryQueryValidator()
	{
		RuleFor(x => x.CategoryId).GreaterThan(0).WithMessage("Category Id must be a positive integer.");
	}
}

public class CategoryByIdQueryHandler(ICategoryQueryService categoryQueryService)
		: IQueryHandler<CategoryByIdQuery, CategoryByIdQueryResult>
{
	public async Task<CategoryByIdQueryResult> Handle(CategoryByIdQuery query, CancellationToken cancellationToken)
	{
		var category = await categoryQueryService.GetCategoryByIdAsync(query.CategoryId);
		return new CategoryByIdQueryResult(category);
	}
}
#endregion