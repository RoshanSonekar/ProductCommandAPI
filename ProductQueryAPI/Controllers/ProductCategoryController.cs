using BuildingBlocks.CQRS;
using CatalogQueryService.Application;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CatalogQueryService.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ProductCategoryController : ControllerBase
	{
		private readonly IMediator _mediatR;
		public ProductCategoryController(IMediator mediatR)
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


		
	}
} 