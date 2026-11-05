using BuildingBlocks.Exceptions.Handler;
using HealthChecks.UI.Client;
using JasperFx.MultiTenancy;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using ProductCommandAPI.Infra;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);
var assembly = typeof(Program).Assembly;

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
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

//// Add Carter for minimal API
//builder.Services.AddCarter();

//builder.Services.AddMarten(options =>
//{
//	options.Connection(builder.Configuration.GetConnectionString("PostgreSql")!); // Roshan#2026@ DB password
//}).UseLightweightSessions();

builder.Services.AddDbContext<ProductCommandAPIContext>
	(
	options => options.UseSqlServer(connectionString
	));

builder.Services.AddExceptionHandler<CustomExceptionHandler>();

//builder.Services.AddHealthChecks()
//	.AddNpgSql(builder.Configuration.GetConnectionString("PostgreSql")!);

// Application Services
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IProductService, ProductService>();

builder.Services.AddHealthChecks()
		.AddSqlServer(
				connectionString: connectionString,
				healthQuery: "SELECT 1;", // Default, but can be customized
				name: "sql-server",
				tags: new[] { "ready" }
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