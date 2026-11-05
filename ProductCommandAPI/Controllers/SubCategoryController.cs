using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductCommandAPI.Application;
using ProductCommandAPI.Models;

[Route("api/[controller]")]
[ApiController]
public class SubCategoryController : ControllerBase
{
	private readonly IMediator _mediatR;
	public SubCategoryController	(IMediator mediatR)
	{
		_mediatR = mediatR;
	}

	[HttpPost("V1/SubCategory")]
	public async Task<ActionResult<ProductSubCategory>> CreateSubCategory([FromBody] SubCategoryCommand category)
	{
		try
		{
			var result = await _mediatR.Send(category);
			return StatusCode(StatusCodes.Status201Created, result.SubCategoryId);
		}
		catch (Microsoft.EntityFrameworkCore.DbUpdateException ex)
				when (ex.InnerException?.Message.Contains("duplicate") == true)
		{
			return Conflict(new { message = "SubCategory already exists." });
		}
		catch (System.ComponentModel.DataAnnotations.ValidationException ex)
		{
			return BadRequest(new { message = ex.Message });
		}
		catch (Exception ex)
		{
			return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An unexpected server error occurred." });
		}
	}

	[HttpPut("V1/SubCategory")]
	public async Task<ActionResult<ProductSubCategory>> UpdateSubCategory([FromBody] SubCategoryUpdateCommand category)
	{
		try
		{
			var result = await _mediatR.Send(category);
			return StatusCode(StatusCodes.Status201Created, result.SubCategoryId);
		}
		catch (Microsoft.EntityFrameworkCore.DbUpdateException ex)
				when (ex.InnerException?.Message.Contains("duplicate") == true)
		{
			return Conflict(new { message = "SubCategory already exists." });
		}
		catch (System.ComponentModel.DataAnnotations.ValidationException ex)
		{
			return BadRequest(new { message = ex.Message });
		}
		catch (Exception ex)
		{
			return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An unexpected server error occurred." });
		}
	}
}
