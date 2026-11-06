using BuildingBlocks.CQRS;
using CatalogQueryService.Infra;
using CatalogQueryService.Models;
using FluentValidation;
using System.Text.Json.Serialization;

namespace CatalogQueryService.Application
{

	#region --- get all categories --- 
	public record SubCategoriesQuery(int? PageNumber = 1, int? PageSize = 10)
			: IQuery<GetSubCategoriesResult>;

	public record GetSubCategoriesResult
	{
		[JsonPropertyName("subCategoryEntities")]
		public IEnumerable<SubCategoryWithCategoryDto> subCategoryEntities { get; init; }

		[JsonPropertyName("totalCount")]
		public int TotalCount { get; init; }

		public GetSubCategoriesResult(IEnumerable<SubCategoryWithCategoryDto> _subCategoryEntities, int totalCount)
		{
			subCategoryEntities = _subCategoryEntities;
			TotalCount = totalCount;
		}
	}

	public class SubCategoriesQueryHandler(ISubCategoryQueryService subCategoryQueryService)
		: IQueryHandler<SubCategoriesQuery, GetSubCategoriesResult>
	{
		public async Task<GetSubCategoriesResult> Handle(SubCategoriesQuery query, CancellationToken cancellationToken)
		{
			int pageNumber = query.PageNumber ?? 1;
			int pageSize = query.PageSize ?? 10;

			var (subCategories, totalCount) = await subCategoryQueryService.GetAllSubCategoriesAsync(
				pageNumber,
				pageSize
			);

			return new GetSubCategoriesResult(subCategories, totalCount);
		}
	}
	#endregion



	#region --- get category by id ---

	public record SubCategoryByIdQuery(int SubCategoryId) 
		: IQuery<SubCategoryByIdQueryResult>;
	public record SubCategoryByIdQueryResult(SubCategoryWithCategoryDto SubCategory);

	public class SubCategoryQueryValidator : AbstractValidator<SubCategoryByIdQuery>
	{
		public SubCategoryQueryValidator()
		{
			RuleFor(x => x.SubCategoryId).GreaterThan(0).WithMessage("SubCategory Id must be a positive integer.");
		}
	}

	public class SubCategoryByIdQueryHandler(ISubCategoryQueryService subCategoryQueryService)
			: IQueryHandler<SubCategoryByIdQuery, SubCategoryByIdQueryResult>
	{
		public async Task<SubCategoryByIdQueryResult> Handle(SubCategoryByIdQuery query, CancellationToken cancellationToken)
		{
			var subCategory = await subCategoryQueryService.GetSubCategoryByIdAsync(query.SubCategoryId);
			return new SubCategoryByIdQueryResult(subCategory);
		}
	}
	#endregion


}
