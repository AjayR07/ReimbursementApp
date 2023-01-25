using Azure.Extensions.AspNetCore.Configuration.Secrets;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using FluentValidation.AspNetCore;
using ReimbursementApp.API;
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

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
// Add Application Services
builder.Services.AddServices(configuration);
// Add Infra Context
builder.Services.AddPersistence(configuration);

// Api DI
builder.Services.AddJWT(configuration);

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

var app = builder.Build();

// Configure the HTTP request pipeline.
// if (app.Environment.IsDevelopment())
// {
    app.UseSwagger();
    app.UseSwaggerUI();
// }

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseCors("corsapp");
app.MapControllers();

app.Run();