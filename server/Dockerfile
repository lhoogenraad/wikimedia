# Use the .NET 9 SDK to build the app
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

# Copy everything from 'server' directory
COPY . ./

# Restore dependencies
RUN dotnet restore

# Build and publish the app
RUN dotnet publish -c Release -o /out

# Use the .NET 9 runtime to run the app
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /out .
EXPOSE 8080
CMD ["dotnet", "server.dll"]

