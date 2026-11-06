using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductCommandAPI.Application;
using ProductCommandAPI.Models;

[Route("api/[controller]")]
[ApiController]
public class ProductCategoriesController : ControllerBase
{
	private readonly IMediator _mediatR;
	public ProductCategoriesController(IMediator mediatR)
	{
		_mediatR = mediatR;
	}

	// POST: api/Category
	[HttpPost("V1/Category")]
	public async Task<ActionResult<ProductCategory>> CreateCategory([FromBody] CategoryCommand category)
	{
		try
		{
			var result = await _mediatR.Send(category);
			return StatusCode(StatusCodes.Status201Created, new ResponseModel { Id = result.CategoryId, IsSuccess = result.IsSuccess, Message = "Category created successfully." });
		}
		catch (DbUpdateException ex)
				when (ex.InnerException?.Message.Contains("duplicate") == true)
		{
			return Conflict(new ResponseModel { Id = 0, IsSuccess = false, Message = "Category already exists." } );
		}
		catch (DbUpdateException ex)
		{
			return StatusCode(StatusCodes.Status400BadRequest, new ResponseModel { Id = 0, IsSuccess = false, Message = ex.InnerException?.Message ?? ex.Message });
		}
		catch (System.ComponentModel.DataAnnotations.ValidationException ex)
		{
			return BadRequest(new ResponseModel { Id = 0, IsSuccess = false, Message = ex.Message });
		}
		catch
		{
			return StatusCode(StatusCodes.Status500InternalServerError, new ResponseModel { Id = 0, IsSuccess = false, Message = "An unexpected server error occurred." });
		}
	}

	// PUT: api/Category
	[HttpPut("V1/Category")]
	public async Task<ActionResult<ProductCategory>> UpdateCategory([FromBody] CategoryUpdateCommand category)
	{
		try
		{
			var result = await _mediatR.Send(category);
			return StatusCode(StatusCodes.Status201Created, new ResponseModel { Id = result.CategoryId, IsSuccess = result.IsSuccess, Message = "Category updated successfully." });
		}
		catch (DbUpdateException ex)
				when (ex.InnerException?.Message.Contains("duplicate") == true)
		{
			return Conflict(new ResponseModel { Id = 0, IsSuccess = false, Message = "Category already exists." });
		}
		catch (DbUpdateException ex)
		{
			return StatusCode(StatusCodes.Status400BadRequest, new ResponseModel { Id = 0, IsSuccess = false, Message = ex.InnerException?.Message ?? ex.Message });
		}
		catch (System.ComponentModel.DataAnnotations.ValidationException ex)
		{
			return BadRequest(new ResponseModel { Id = 0, IsSuccess = false, Message = ex.Message });
		}
		catch
		{
			return StatusCode(StatusCodes.Status500InternalServerError, new ResponseModel { Id = 0, IsSuccess = false, Message = "An unexpected server error occurred." });
		}
	}
}
