# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy solution and project files
COPY ["SimpleECommerceForTraining.sln", "./"]
COPY ["src/SimpleECommerce.Core/SimpleECommerce.Core.csproj", "src/SimpleECommerce.Core/"]
COPY ["src/SimpleECommerce.DataAccess/SimpleECommerce.DataAccess.csproj", "src/SimpleECommerce.DataAccess/"]
COPY ["src/SimpleECommerce.Business/SimpleECommerce.Business.csproj", "src/SimpleECommerce.Business/"]
COPY ["src/SimpleECommerce.API/SimpleECommerce.API.csproj", "src/SimpleECommerce.API/"]

# Restore dependencies
RUN dotnet restore "src/SimpleECommerce.API/SimpleECommerce.API.csproj"

# Copy all source code
COPY . .

# Build the application
WORKDIR "/src/src/SimpleECommerce.API"
RUN dotnet build "SimpleECommerce.API.csproj" -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish "SimpleECommerce.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Copy published files
COPY --from=publish /app/publish .

# Set environment variables
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production

ENTRYPOINT ["dotnet", "SimpleECommerce.API.dll"]
