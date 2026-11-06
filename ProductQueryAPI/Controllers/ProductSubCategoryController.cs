using CatalogQueryService.Application;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CatalogQueryService.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ProductSubCategoryController : ControllerBase
	{
		private readonly IMediator _mediatR;
		public ProductSubCategoryController(IMediator mediatR)
		{
			_mediatR = mediatR;
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

		[HttpGet("V1/SubCategories/CategoryId/{CategoryId}")]
		public async Task<ActionResult> GetSubCategories([FromRoute] int CategoryId, [FromQuery] int? PageNumber = 1, [FromQuery] int? PageSize = 10, CancellationToken cancellationToken = default)
		{
			var result = await _mediatR.Send(new SubCategoriesByCategoryIdQuery(CategoryId, PageNumber, PageSize), cancellationToken);

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
