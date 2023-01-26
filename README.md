
# Reimbursement System

 A .Net 6 Web API Project for the implementation of Reimbursement System





## Demo

https://reimbursementsystem.azurewebsites.net/swagger/index.html


## Tech Stack

**Backend:** .Net 6 

**Database:** Azure SQL Server

**Configuration Management:** Azure Key Vault


## Features Implemented 

- Entity Framework Core (ORM)
- Memory Caching
- JWT Authentication and Role based Authorization
- CORS
- Exception Handling
- Fluent Validations
- DTO & AutoMappers
- Clean Architecture
- Code First Approach

## Azure Services 

- Azure App Service
- Azure KeyVault
- Azure SQL Server
- Azure Resource Group
- Azure Service Bus Queues
- Azure Storage Account Containers - Blob Storage
- Azure API Management Service
- Azure Application Insights

## Extensions Added

- AutoMapper
- AutoMapper.Extensions.Microsoft.DependencyInjection
- Azure.Extensions.AspNetCore.Configuration.Secrets
- Azure.Identity
- Azure.Messaging.ServiceBus
- Azure.Security.KeyVault.Secrets
- Azure.Storage.Blobs
- BCrypt.Net-Next
- FluentValidation.AspNetCore
- Microsoft.AspNetCore.Authentication.JwtBearer
- Microsoft.AspNetCore.Http.Features
- Microsoft.EntityFrameworkCore
- Microsoft.EntityFrameworkCore.Design
- Microsoft.EntityFrameworkCore.SqlServer
- Microsoft.EntityFrameworkCore.Tools
- Microsoft.Extensions.Configuration.Abstractions
- Microsoft.Extensions.DependencyInjection.Abstractions
- Swashbuckle.AspNetCore
- System.IdentityModel.Tokens.Jwt


## Run Locally

Clone the project

```bash
  git clone https://github.com/AjayR07/ReimbursementApp.git
```

To run Migrations 

```bash
  dotnet ef migrations add MigrationName -s ReimbursementApp.API -p ReimbursementApp.Infrastructure

```

To update database

```bash
  dotnet ef database update -s ReimbursementApp.API -p ReimbursementApp.Infrastructure 
```




## Environment Variables

To run this project, you will need to add the following environment variables to KeyVault Secrets and update the KeyVault URL in appsettings.json

`ConnectionStrings--SSMS` - SQL Server Connection String

`JWT--Key` - JWT Key

`JWT--Issuer` - JWT Issuer 

`JWT--Audience` - JWT Audience

`JWT--Expiry` - JWT Expiry Duration

`AzureServiceBusConnectionString` - Service Bus Connection String

`AdminQueueName` - Azure Service Bus Queue Name

`BlobConnectionString` - Azure Storage Account Connection String

`BlobContainerName` - Azure Storage Account Container Name

`BlobContainerName` - Azure Storage Account Container Name


