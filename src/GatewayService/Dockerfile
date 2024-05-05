# Use the official .NET SDK image from Microsoft's container registry as the base image
# This image includes the .NET SDK and all its dependencies
FROM mcr.microsoft.com/dotnet/sdk:8.0 as build

# Set the working directory in the container to /app
WORKDIR /app

# Expose port 80 for the application
EXPOSE 80

# Copy the solution file and all project files into the container
# This is done separately from the rest of the code to take advantage of Docker's layer caching
# If these files haven't changed, Docker can reuse the cached layers instead of rebuilding them
COPY Pixelbid.sln Pixelbid.sln
COPY src/AuctionService/AuctionService.csproj src/AuctionService/AuctionService.csproj
COPY src/SearchService/SearchService.csproj src/SearchService/SearchService.csproj
COPY src/BiddingService/BiddingService.csproj src/BiddingService/BiddingService.csproj
COPY src/NotificationService/NotificationService.csproj src/NotificationService/NotificationService.csproj
COPY src/GatewayService/GatewayService.csproj src/GatewayService/GatewayService.csproj
COPY src/IdentityService/IdentityService.csproj src/IdentityService/IdentityService.csproj
COPY src/Contracts/Contracts.csproj src/Contracts/Contracts.csproj

# Restore the NuGet packages for the solution
# This is done separately from the rest of the code to take advantage of Docker's layer caching
# If the project files haven't changed, Docker can reuse the cached layers instead of re-restoring the packages
RUN dotnet restore Pixelbid.sln

# Copy the rest of the code into the container
COPY src/GatewayService src/GatewayService

# Set the working directory in the container to the GatewayService directory
WORKDIR /app/src/GatewayService

# Publish the application in Release configuration and output the published files to /app/src/out
RUN dotnet publish -c Release -o /app/src/out

# Use the official .NET runtime image from Microsoft's container registry as the base image for the runtime container
# This image includes the .NET runtime and all its dependencies, but not the .NET SDK
FROM mcr.microsoft.com/dotnet/aspnet:8.0

# Set the working directory in the container to /app
WORKDIR /app

# Copy the published files from the build container into the runtime container
COPY --from=build /app/src/out .

# Set the command to run when the container starts
# This will start the GatewayService application
ENTRYPOINT [ "dotnet", "GatewayService.dll" ]