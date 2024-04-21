# Dockerfile
# Use the .NET SDK 8.0 image as the base image for the build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 as build

# Set the working directory inside the docker container to /app
WORKDIR /app

# Expose port 80 for the application
EXPOSE 80

# Copy all .csproj files into the container. These are copied separately to restore dependencies as distinct layers
# and take advantage of Docker's caching mechanism
COPY Pixelbid.sln Pixelbid.sln
COPY src/AuctionService/AuctionService.csproj src/AuctionService/AuctionService.csproj
COPY src/SearchService/SearchService.csproj src/SearchService/SearchService.csproj
COPY src/GatewayService/GatewayService.csproj src/GatewayService/GatewayService.csproj
COPY src/IdentityService/IdentityService.csproj src/IdentityService/IdentityService.csproj
COPY src/Contracts/Contracts.csproj src/Contracts/Contracts.csproj

# Restore the dependencies for the entire solution
RUN dotnet restore Pixelbid.sln

# Copy the source code of the AuctionService and Contracts projects into the container
COPY src/AuctionService src/AuctionService
COPY src/Contracts src/Contracts

# Set the working directory in the container to /app/src/AuctionService
WORKDIR /app/src/AuctionService

# Publish the AuctionService project in Release configuration and output the result to /app/src/out
RUN dotnet publish -c Release -o /app/src/out

# Start a new stage for the runtime image using the .NET ASP.NET 8.0 image as the base image
FROM mcr.microsoft.com/dotnet/aspnet:8.0

# Set the working directory in the container to /app
WORKDIR /app

# Copy the published output from the build stage into the current directory in the container
COPY --from=build /app/src/out .

# Set the entrypoint for the container to run the AuctionService.dll
ENTRYPOINT [ "dotnet", "AuctionService.dll" ]