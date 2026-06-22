using System.Text;
using System.Text.Json.Serialization;
using API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using Npgsql;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<AdminService>();
builder.Services.AddScoped<CustomerService>();
builder.Services.AddScoped<CompanyService>();
builder.Services.AddScoped<CategoryService>();
builder.Services.AddScoped<ProductService>();
builder.Services.AddScoped<JwtService>();
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
builder.Services.AddHttpContextAccessor();
//

// Validators
builder.Services.AddScoped<ProductValidator>();
builder.Services.AddScoped<RegisterValidator>();
builder.Services.AddScoped<LoginValidation>();
builder.Services.AddScoped<AdminProfileValidator>();
//

// Data Base connection
var connectionString = builder.Configuration.GetConnectionString("DbConnection");

builder.Services.AddDbContext<ApiDbContext>(options =>
{
    options.UseNpgsql(connectionString);
});
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

// Swagger
builder.Services.AddMvc();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo 
    {
        Title = "E-Commerce API", 
        Version = "v1"
    });
});
//

builder.Services.AddControllers();

builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseSwagger();

app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "E-Commerce API");
});

// MiddleWares
app.UseMiddleware<GlobalExceptionMiddleware>();

app.UseHttpsRedirection();

// Wait a minute! Who're you bro?
app.UseAuthentication();

// Are you authorized to do it?
app.UseAuthorization();

app.MapControllers();

app.Run();
