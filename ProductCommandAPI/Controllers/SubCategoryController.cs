using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
	public async Task<ActionResult<ProductSubCategory>> CreateSubCategory([FromBody] SubCategoryCommand subCategory)
	{
		try
		{
			var result = await _mediatR.Send(subCategory);
			return StatusCode(StatusCodes.Status201Created, new ResponseModel { Id = result.SubCategoryId, IsSuccess = result.IsSuccess, Message = "Sub-Category created successfully." });
		}
		catch (DbUpdateException ex)
				when (ex.InnerException?.Message.Contains("duplicate") == true)
		{
			return Conflict(new ResponseModel { Id = 0, IsSuccess = false, Message = "Sub-Category already exists." });
		}
		catch (DbUpdateException ex)
		{
			return StatusCode(StatusCodes.Status400BadRequest, new ResponseModel { Id = 0, IsSuccess = false, Message = "Category might not exist. >>" + ex.InnerException?.Message ?? ex.Message });
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

	[HttpPut("V1/SubCategory")]
	public async Task<ActionResult<ProductSubCategory>> UpdateSubCategory([FromBody] SubCategoryUpdateCommand subCategory)
	{
		try
		{
			var result = await _mediatR.Send(subCategory);
			return StatusCode(StatusCodes.Status201Created, new ResponseModel { Id = result.SubCategoryId, IsSuccess = result.IsSuccess, Message = "Sub-Category updated successfully." });
		}
		catch (DbUpdateException ex)
				when (ex.InnerException?.Message.Contains("duplicate") == true)
		{
			return Conflict(new ResponseModel { Id = 0, IsSuccess = false, Message = "Sub-Category already exists." });
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
