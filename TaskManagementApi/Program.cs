using TaskManagementApi;
using TaskManagementApi.Settings;
using TaskManagementApi.Services;
using TaskManagementApi.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Amazon.Extensions.NETCore.Setup;
using AWS.Logger;
using AWS.Logger.AspNetCore;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// ------------------------
// Logging to AWS CloudWatch
// ------------------------
var awsOptions = configuration.GetAWSOptions();
builder.Services.AddDefaultAWSOptions(awsOptions);

var awsLoggerConfig = configuration.GetAWSLoggingConfigSection("AWSLogger");
builder.Logging.ClearProviders();
builder.Logging.AddAWSProvider(awsLoggerConfig);
builder.Logging.SetMinimumLevel(LogLevel.Information);

// ------------------------
// Load configuration
// ------------------------
builder.Services.Configure<JwtSettings>(configuration.GetSection("TaskMangementSettings:Jwt"));
builder.Services.Configure<CognitoSettings>(configuration.GetSection("TaskMangementSettings:Cognito"));

var jwtSettings = configuration.GetSection("TaskMangementSettings:Jwt").Get<JwtSettings>();

// ------------------------
// JWT Authentication with AWS Cognito
// ------------------------
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = jwtSettings.Authority;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = jwtSettings.ValidIssuer,
            ValidateAudience = jwtSettings.ValidateAudience,
            ValidateLifetime = true
        };
    });

// ------------------------
// Dependency Injection
// ------------------------
builder.Services.AddScoped<IProjectService, ProjectService>();
builder.Services.AddScoped<ITaskService, TaskService>();
builder.Services.AddScoped<AuthService>();

builder.Services.AddScoped<IProjectRepository, ProjectRepository>();
builder.Services.AddScoped<ITaskRepository, TaskRepository>();

builder.Services.AddControllers();

// ------------------------
// Swagger + JWT support
// ------------------------
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "TaskManagementApi", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter your JWT token like this: Bearer <token>"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

// ------------------------
// DB Context (MySQL)
// ------------------------
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(configuration.GetConnectionString("DefaultConnection"),
        new MySqlServerVersion(new Version(8, 0, 34)))
);

// ------------------------
// Build and Run
// ------------------------
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseMiddleware<ErrorHandlingMiddleware>();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
