using System.Text;
using API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<CustomerService>();
builder.Services.AddScoped<CompanyService>();
builder.Services.AddScoped<CategoryService>();
builder.Services.AddScoped<ProductService>();
builder.Services.AddScoped<JwtService>();
//

// Validators
builder.Services.AddScoped<ProductValidator>();
builder.Services.AddScoped<RegisterCustomerValidator>();
builder.Services.AddScoped<ReqLoginValidation>();
//

// JWT configs to Authenticate and Authorize
var secretKey = 
    builder.Configuration["Jwt:Key"]
        ?? throw new Exception(
            "Jwt:Key not found"
        );

var key = Encoding.UTF8.GetBytes(secretKey);

builder.Services.AddAuthentication(
    JwtBearerDefaults.AuthenticationScheme
)
.AddJwtBearer(options =>
{
    options.Events = new JwtBearerEvents
    {
        // Where's the JWT??
        OnMessageReceived = context =>
        {   
            // The JWT is below:
            context.Token =  context.Request.Cookies["jwt"];

            return Task.CompletedTask;
        }
    };

    // What do I need to validate this Token?
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateLifetime = true,
        ValidateIssuer = false,
        ValidateAudience = false
        
    };
});

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

// Wait a minute! Who're you bro?
app.UseAuthentication();

// Are you authorized to do it?
app.UseAuthorization();

app.MapControllers();

app.Run();
