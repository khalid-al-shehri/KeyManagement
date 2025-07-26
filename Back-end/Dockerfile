# Use the official .NET 8 SDK image for building
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy all source files
COPY . .

# Skip clean, just restore and build directly
RUN dotnet restore --force --no-cache
RUN dotnet publish Api/Api.csproj -c Release -o /app/publish --no-restore

# Use the official .NET 8 runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Copy the published application
COPY --from=build /app/publish .

# Expose port
EXPOSE 80
EXPOSE 443

# Set the entry point
ENTRYPOINT ["dotnet", "Api.dll"]
