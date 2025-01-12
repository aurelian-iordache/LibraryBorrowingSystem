# Use the ASP.NET Core runtime base image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

# Use the .NET SDK image for build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy the project file and restore dependencies
COPY ["CustomerService/CustomerService.csproj", "CustomerService/"]
RUN dotnet restore "CustomerService/CustomerService.csproj"

# Copy the entire source code and build the project
COPY . .
WORKDIR "/src/CustomerService"
RUN dotnet build "CustomerService.csproj" -c Release -o /app/build

# Publish the app to the publish folder
FROM build AS publish
RUN dotnet publish "CustomerService.csproj" -c Release -o /app/publish

# Use the runtime base image for the final container
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CustomerService.dll"]
