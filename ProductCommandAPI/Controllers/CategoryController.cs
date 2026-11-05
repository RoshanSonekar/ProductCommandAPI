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
			return StatusCode(StatusCodes.Status201Created, result.CategoryId);
		}
		catch (Microsoft.EntityFrameworkCore.DbUpdateException ex)
				when (ex.InnerException?.Message.Contains("duplicate") == true)
		{
			return Conflict(new { message = "Category already exists." });
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

	// PUT: api/Category
	[HttpPut("V1/Category")]
	public async Task<ActionResult<ProductCategory>> UpdateCategory([FromBody] CategoryUpdateCommand category)
	{
		try
		{
			var result = await _mediatR.Send(category);
			return StatusCode(StatusCodes.Status201Created, result.CategoryId);
		}
		catch (Microsoft.EntityFrameworkCore.DbUpdateException ex)
				when (ex.InnerException?.Message.Contains("duplicate") == true)
		{
			return Conflict(new { message = "Category already exists." });
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



	////// GET: api/Category
	////[HttpGet]
	////public async Task<ActionResult<IEnumerable<Category>>> GetCategory()
	////{
	////    return await _context.Category.ToListAsync();
	////}

	//// GET: api/Category/5
	//[HttpGet("{categoryid}")]
	//public async Task<ActionResult<ProductCategory>> GetCategory(System.Guid categoryid)
	//{
	//  var category = await _context.ProductCategory.FindAsync(categoryid);

	//  if (category == null)
	//  {
	//    return NotFound();
	//  }

	//  return category;
	//}

	////// PUT: api/Category/5
	////// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
	////[HttpPut("{categoryid}")]
	////public async Task<IActionResult> PutCategory(System.Guid? categoryid, Category category)
	////{
	////    if (categoryid != category.CategoryId)
	////    {
	////        return BadRequest();
	////    }

	////    _context.Entry(category).State = EntityState.Modified;

	////    try
	////    {
	////        await _context.SaveChangesAsync();
	////    }
	////    catch (DbUpdateConcurrencyException)
	////    {
	////        if (!CategoryExists(categoryid))
	////        {
	////            return NotFound();
	////        }
	////        else
	////        {
	////            throw;
	////        }
	////    }

	////    return NoContent();
	////}

	//// POST: api/Category
	//// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
	//[HttpPost("V1/Category")]
	//public async Task<ActionResult<ProductCategory>> PostCategory(ProductCategory category)
	//{
	//  category.CategoryId = 1; // This is a placeholder. In a real application, you would generate or assign a unique ID.

	//_context.ProductCategory.Add(category);
	//  await _context.SaveChangesAsync();

	//  return CreatedAtAction("GetCategory", new { categoryid = category.CategoryId }, category);
	//}

	//  //// DELETE: api/Category/5
	//  //[HttpDelete("{categoryid}")]
	//  //public async Task<IActionResult> DeleteCategory(System.Guid? categoryid)
	//  //{
	//  //    var category = await _context.Category.FindAsync(categoryid);
	//  //    if (category == null)
	//  //    {
	//  //        return NotFound();
	//  //    }

	//  //    _context.Category.Remove(category);
	//  //    await _context.SaveChangesAsync();

	//  //    return NoContent();
	//  //}

	//  private bool CategoryExists(System.Guid? categoryid)
	//  {
	//      return _context.ProductCategory.Any(e => e.CategoryId == categoryid);
	//  }
}
