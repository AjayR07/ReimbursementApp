using System.Reflection;
using System.Resources;
using Azure.Extensions.AspNetCore.Configuration.Secrets;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Versioning;
using ReimbursementApp.API;
using ReimbursementApp.API.Configurations;
using ReimbursementApp.Application;
using ReimbursementApp.Application.Validators;
using ReimbursementApp.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
// Get Configuration 
var configuration = builder.Configuration;

// Add secrets to configuration from Azure Key Vault
var secretClient = new SecretClient(
    new Uri(builder.Configuration["KeyVault:VaultUri"]),
    new DefaultAzureCredential());
builder.Configuration.AddAzureKeyVault(secretClient, new KeyVaultSecretManager());

// Add services to the container.

// Fluent Valdation on Controller 
builder.Services.AddControllers().AddFluentValidation(options =>
{
    // Validate child properties and root collection elements
    options.ImplicitlyValidateChildProperties = true;
    options.ImplicitlyValidateRootCollectionElements = true;

    // Automatic registration of validators in assembly
    options.RegisterValidatorsFromAssemblyContaining<EmployeeDtoValidator>();
});

builder.Services.AddApiVersioning(opt =>
{
    opt.DefaultApiVersion = new ApiVersion(1,0);
    // opt.DefaultApiVersion = ApiVersion.Default;
    opt.AssumeDefaultVersionWhenUnspecified = true;
    opt.ReportApiVersions = true;
    opt.ApiVersionReader = ApiVersionReader.Combine(
        // new UrlSegmentApiVersionReader(),
        // new HeaderApiVersionReader("x-api-version"),
        // new MediaTypeApiVersionReader("x-api-version"),
        new QueryStringApiVersionReader("x-api-version")); //default
});
// Add ApiExplorer to discover versions
builder.Services.AddVersionedApiExplorer(setup =>
{
    setup.GroupNameFormat = "'v'VVV";
    setup.SubstituteApiVersionInUrl = true;
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
// Add Application Services
builder.Services.AddServices(configuration);
// Add Infra Context
builder.Services.AddPersistence(configuration);

// Api DI
builder.Services.AddJWT(configuration);
builder.Services.AddHttpContextAccessor();

// General Service Injections
//Mapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
// Caching
builder.Services.AddMemoryCache();
//Cors Service
builder.Services.AddCors(p => p.AddPolicy("corsapp", builder =>
{
    builder.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
}));

//Swagger API Versioning Configs
builder.Services.ConfigureOptions<ConfigureSwaggerOptions>();

builder.Services.AddRouting(options => options.LowercaseUrls = true);


var app = builder.Build();

app.UsePathBase(new PathString("/api"));

var apiVersionDescriptionProvider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

// Configure the HTTP request pipeline.
// if (app.Environment.IsDevelopment())
// {
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        foreach (var description in apiVersionDescriptionProvider.ApiVersionDescriptions.Reverse())
        {
            options.SwaggerEndpoint($"/api/swagger/{description.GroupName}/swagger.json",
                description.GroupName.ToUpperInvariant());
        }
    });
// }

app.AddGlobalErrorHandler();

app.UseHttpsRedirection();

app.UseCors("corsapp");

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();