using API.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<CompanyService>();
builder.Services.AddScoped<CategoryService>();
builder.Services.AddScoped<ProductService>();
//

// Validators
builder.Services.AddScoped<ProductValidator>();
builder.Services.AddScoped<RegisterCustomerValidator>();
//

builder.Services.AddControllers();

builder.Services.AddOpenApi();

// Data Base connection
var connectionString = builder.Configuration.GetConnectionString("DbConnection");

builder.Services.AddDbContext<ApiDbContext>(options =>
{
    options.UseNpgsql(connectionString);
});
//

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// MiddleWares
app.UseMiddleware<GlobalExceptionMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
