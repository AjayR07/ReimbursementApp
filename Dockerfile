FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["ReimbursementApp.API/ReimbursementApp.API.csproj", "ReimbursementApp.API/"]
COPY ["ReimbursementApp.Infrastructure/ReimbursementApp.Infrastructure.csproj", "ReimbursementApp.Infrastructure/"]
COPY ["ReimbursementApp.Domain/ReimbursementApp.Domain.csproj", "ReimbursementApp.Domain/"]
COPY ["ReimbursementApp.Application/ReimbursementApp.Application.csproj", "ReimbursementApp.Application/"]
RUN dotnet restore "ReimbursementApp.API/ReimbursementApp.API.csproj"
COPY . .
WORKDIR "/src/ReimbursementApp.API"
RUN dotnet build "ReimbursementApp.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ReimbursementApp.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ReimbursementApp.API.dll"]
