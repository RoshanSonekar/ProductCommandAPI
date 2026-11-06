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

	#region --- get sub-category by id ---
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

	#region --- get category by id ---
	public record SubCategoriesByCategoryIdQuery(int CategoryId, int? PageNumber = 1, int? PageSize = 10) : IQuery<GetSubCategoriesByCategoryIdResult>;
	public record GetSubCategoriesByCategoryIdResult
	{
		[JsonPropertyName("subCategoryEntities")]
		public IEnumerable<ProductSubCategory> subCategoryEntities { get; init; }

		[JsonPropertyName("categoryEntity")]
		public ProductCategory productCategoryEntity { get; init; }

		[JsonPropertyName("totalCount")]
		public int TotalCount { get; init; }

		public GetSubCategoriesByCategoryIdResult(ProductCategory _productCategoryEntity, IEnumerable<ProductSubCategory> _subCategoryEntities, int _totalCount)
		{
			productCategoryEntity = _productCategoryEntity;
			subCategoryEntities = _subCategoryEntities;
			TotalCount = _totalCount;
		}
	}

	public class SubCategoriesByCategoryIdQueryValidator : AbstractValidator<SubCategoriesByCategoryIdQuery>
	{
		public SubCategoriesByCategoryIdQueryValidator()
		{
			RuleFor(x => x.CategoryId).GreaterThan(0).WithMessage("Category Id must be a positive integer.");
		}
	}

	public class SubCategoriesByCateogoryIdQueryHandler(ISubCategoryQueryService subCategoryQueryService)
			: IQueryHandler<SubCategoriesByCategoryIdQuery, GetSubCategoriesByCategoryIdResult>
	{
		public async Task<GetSubCategoriesByCategoryIdResult> Handle(SubCategoriesByCategoryIdQuery query, CancellationToken cancellationToken)
		{
			int pageNumber = query.PageNumber ?? 1;
			int pageSize = query.PageSize ?? 10;

			var (productCategory, subCategories, totalCount) = await subCategoryQueryService.GetSubCategoriesByCategoryIdAsync(
				query.CategoryId,
				pageNumber,
				pageSize
				);
			return new GetSubCategoriesByCategoryIdResult(productCategory, subCategories, totalCount);
		}
	}
	#endregion
}
