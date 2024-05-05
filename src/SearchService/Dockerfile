# Use the official Microsoft .NET 8 SDK image as the base image
FROM mcr.microsoft.com/dotnet/sdk:8.0 as build

# Set the working directory in the container to /app
WORKDIR /app

# Expose port 80 for the application
EXPOSE 80

# Copy the solution file and all project files into the container
# This is done separately from the rest of the code to take advantage of Docker's layer caching
# If these files haven't changed, Docker can reuse the cached layers instead of restoring the packages every time
COPY Pixelbid.sln Pixelbid.sln
COPY src/AuctionService/AuctionService.csproj src/AuctionService/AuctionService.csproj
COPY src/SearchService/SearchService.csproj src/SearchService/SearchService.csproj
COPY src/BiddingService/BiddingService.csproj src/BiddingService/BiddingService.csproj
COPY src/NotificationService/NotificationService.csproj src/NotificationService/NotificationService.csproj
COPY src/GatewayService/GatewayService.csproj src/GatewayService/GatewayService.csproj
COPY src/IdentityService/IdentityService.csproj src/IdentityService/IdentityService.csproj
COPY src/Contracts/Contracts.csproj src/Contracts/Contracts.csproj

# Restore the NuGet packages for the solution
RUN dotnet restore Pixelbid.sln

# Copy the rest of the code into the container
COPY src/SearchService src/SearchService
COPY src/Contracts src/Contracts

# Set the working directory to the SearchService project directory
WORKDIR /app/src/SearchService

# Publish the SearchService project, creating a self-contained application with all its dependencies
RUN dotnet publish -c Release -o /app/src/out

# Start a new stage of the build, using the official Microsoft ASP.NET runtime image as the base image
FROM mcr.microsoft.com/dotnet/aspnet:8.0

# Set the working directory in the container to /app
WORKDIR /app

# Copy the published application from the build stage into the current stage
COPY --from=build /app/src/out .

# Set the command to run when the container starts
# This will start the SearchService application
ENTRYPOINT [ "dotnet", "SearchService.dll" ]