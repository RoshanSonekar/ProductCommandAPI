using BuildingBlocks.Behaviors;
using BuildingBlocks.Exceptions.Handler;
using CatalogQueryService.Application;
using CatalogQueryService.Infra;
using CatalogQueryService.SQLServer.DbContextEF;
using FluentValidation;
using HealthChecks.UI.Client;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);
var assembly = typeof(Program).Assembly;

// Add services to the container.
string connectionString = builder.Configuration.GetConnectionString("SQLServer") ?? throw new InvalidOperationException("Connection string 'SqlServer' not found.");

builder.Services.AddControllers();
builder.Services.AddOpenApi();

// Add services to the container.
builder.Services.AddMediatR(config =>
{
	config.RegisterServicesFromAssembly(assembly);
	// Add validation and logging behavior to the MediatR pipeline
	config.AddOpenBehavior(typeof(ValidationBehavior<,>));
	config.AddOpenBehavior(typeof(LoggingBehavior<,>));
});

// Add FluentValidation services
builder.Services.AddValidatorsFromAssembly(assembly);

builder.Services.AddDbContext<CatalogDbContext>
	(
	options => options.UseSqlServer(connectionString
	));

builder.Services.AddExceptionHandler<CustomExceptionHandler>();

// Explicitly register the transaction handlers from the Application project (MongoDB)
builder.Services.AddTransient<MediatR.IRequestHandler<CatalogQueryService.Application.CategoriesQuery, CatalogQueryService.Application.GetCategoriesResult>, CategoriesQueryHandler>();
builder.Services.AddTransient<MediatR.IRequestHandler<CatalogQueryService.Application.CategoryByIdQuery, CatalogQueryService.Application.CategoryByIdQueryResult>, CategoryByIdQueryHandler>();
builder.Services.AddTransient<MediatR.IRequestHandler<CatalogQueryService.Application.SubCategoryByIdQuery, CatalogQueryService.Application.SubCategoryByIdQueryResult>, SubCategoryByIdQueryHandler>();
//builder.Services.AddTransient<MediatR.IRequestHandler<CategoryQuery, GetCategoriesResult>, CategoryQueryHandler>();

// Application Services
builder.Services.AddScoped<ICategoryQueryService, CategoryQueryService>();
builder.Services.AddScoped<ISubCategoryQueryService, SubCategoryQueryService>();


builder.Services.AddHealthChecks()
		.AddSqlServer(
				connectionString: connectionString,
				healthQuery: "SELECT 1;", // Default, but can be customized
				name: "sql-server",
				tags: new [] { "ready" }
		);

var app = builder.Build();

app.UseHealthChecks("/health",
	new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
	{
		ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
	});

app.MapOpenApi();
app.MapScalarApiReference();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.Run();