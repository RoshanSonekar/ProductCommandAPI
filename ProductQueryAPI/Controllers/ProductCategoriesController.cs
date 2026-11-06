using BuildingBlocks.CQRS;
using CatalogQueryService.Application;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CatalogQueryService.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ProductCategoriesController : ControllerBase
	{
		private readonly IMediator _mediatR;
		public ProductCategoriesController(IMediator mediatR)
		{
			_mediatR = mediatR;
		}

		[HttpGet("V1/Categories")]
		public async Task<ActionResult> GetCategories([FromQuery] int? PageNumber = 1, [FromQuery] int? PageSize = 10, CancellationToken cancellationToken = default)
		{
			var result = await _mediatR.Send(new CategoriesQuery(PageNumber, PageSize), cancellationToken);

			if (result is null || result.categoryEntities is null)
			{
				return NotFound(new
				{
					Error = "CategoriesNotFound",
					Message = "Something went wrong. Please try again later."
				});
			}

			return Ok(new
			{
				categoryEntities = result.categoryEntities,
				totalCount = result.TotalCount
			});
		}

		[HttpGet("V1/Categories/{categoryId}")]
		public async Task<ActionResult> GetCategoryById([FromRoute] int categoryId, CancellationToken cancellationToken = default)
		{
			var category = await _mediatR.Send(new CategoryByIdQuery(categoryId), cancellationToken);

			if (category is null)
			{
				return NotFound(new
				{
					Error = "CategoryNotFound",
					Message = "Something went wrong. Please try again later."
				});
			}

			return Ok(category);
		}


		[HttpGet("V1/SubCategories/{subCategoryId}")]
		public async Task<ActionResult> GetSubCategoryById([FromRoute] int subCategoryId, CancellationToken cancellationToken = default)
		{
			var subCategory = await _mediatR.Send(new SubCategoryByIdQuery(subCategoryId), cancellationToken);

			if (subCategory is null)
			{
				return NotFound(new
				{
					Error = "CategoryNotFound",
					Message = "Something went wrong. Please try again later."
				});
			}

			return Ok(subCategory);
		}

		[HttpGet("V1/SubCategories")]
		public async Task<ActionResult> GetSubCategories([FromQuery] int? PageNumber = 1, [FromQuery] int? PageSize = 10, CancellationToken cancellationToken = default)
		{
			var result = await _mediatR.Send(new SubCategoriesQuery(PageNumber, PageSize), cancellationToken);

			if (result is null || result.subCategoryEntities is null)
			{
				return NotFound(new
				{
					Error = "SubCategoriesNotFound",
					Message = "Something went wrong. Please try again later."
				});
			}

			return Ok(result);
		}
	}
} 