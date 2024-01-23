# Use the official .NET SDK image as the base image
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Set the working directory inside the container
WORKDIR /app

# Copy the project file and restore dependencies
COPY bagel/*.csproj ./
RUN dotnet restore

# Copy the remaining source code
COPY . ./

# Build the application
RUN dotnet publish -c Release -o out

# Use the official ASP.NET runtime image as the base image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime

# Set the working directory inside the container
WORKDIR /app

# Copy the published output from the build stage
COPY --from=build /app/out ./

# Expose the port that the application listens on
EXPOSE 80

# Set the entry point for the container
ENTRYPOINT ["dotnet", "bagel.dll"]
