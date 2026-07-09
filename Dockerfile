# Multi-stage build for .NET 9 Blazor / ASP.NET Core app
# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy project file and restore dependencies
COPY ["ERP_Software.csproj", "./"]
RUN dotnet restore "ERP_Software.csproj"

# Copy everything else and publish
COPY . .
RUN dotnet publish "ERP_Software.csproj" -c Release -o /app/publish --no-restore

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:80
EXPOSE 80

ENTRYPOINT ["dotnet", "ERP_Software.dll"]
