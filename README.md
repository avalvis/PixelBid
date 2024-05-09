
# PixelBid: Microservices Video Game Auction App

Welcome to PixelBid, a microservices-based web application for auctioning video games. This project is structured around multiple microservices, each handling a specific part of the application, from auction handling to user authentication and gateway management.

## Getting Started

## Prerequisites

Before you start, ensure you have Docker installed on your system, as it includes Docker Compose needed for running multi-container applications.

To verify Docker and Docker Compose are installed, run the following commands in your terminal:

```bash
docker --version
```

### Setup

Clone the repository to your local machine:

```bash
git clone https://github.com/yourgithub/pixelbid.git
cd pixelbid
```

### Running the Application

Use Docker Compose to run all the services:

```bash
docker compose up --build
```

This command builds and starts all the services defined in your `docker-compose.yml` file.

## Services

PixelBid is composed of several services, each residing in its own directory:

- **Auction Service**: Manages auctions, including CRUD operations.
- **Search Service**: Handles searching and filtering auction items.
- **Identity Service**: Manages user authentication and authorization.
- **Gateway Service**: Acts as the entry point for all requests and routes them to appropriate services.
- **Bid Service**: Handles all bid-related operations.
- **Notification Service**: Manages real-time notifications via SignalR.

## Client Application

The frontend is built using NextJS. It interacts with the backend via a gateway service.

## Features

- CRUD operations for auction items.
- User registration and login.
- Real-time bidding and notifications.
- Search and filtering capabilities.
- Resilient communication between services using RabbitMQ and gRPC.

## Testing

Each service comes with a Postman collection to test its endpoints. Import these collections into Postman to interact with the services directly.

## Deployment

Follow the instructions in the `docker-compose.yml` file to deploy the services using Docker. Ensure all services are properly networked for seamless interaction.

The hosts file is a computer file used by an operating system to map hostnames to IP addresses. In this case, the line `127.0.0.1 id.pixelbid.com app.pixelbid.com api.pixelbid.com` is mapping the hostnames `id.pixelbid.com`, `app.pixelbid.com`, and `api.pixelbid.com` to the IP address `127.0.0.1`, which is the loopback address, often referred to as `localhost`.

The reason for adding these entries to your hosts file is to allow your local machine to recognize these hostnames and direct traffic to the correct local IP address. This is particularly useful when you're developing locally and want to simulate a production-like environment with different subdomains for different services, but without the need to set up DNS records.

In the context of the PixelBid application:

- `id.pixelbid.com` is be the hostname for the Identity Service.
- `app.pixelbid.com` is the hostname for the main client application Service.
- `api.pixelbid.com` is the hostname for the API endpoints.

By adding these entries to your hosts file, when your application tries to reach `id.pixelbid.com`, for example, the operating system will direct that to your local machine (`127.0.0.1`). This allows you to develop and test your microservices locally as if they were in a production environment.

## Environment Variables for Frontend

The frontend of the application also requires certain environment variables to function correctly. These are stored in a `.env.local` file in the root of the frontend directory. 

Here's an example of what the `.env.local` file might look like:

```shellscript
NEXTAUTH_SECRET=your_next_auth_secret
NEXTAUTH_URL=http://your_frontend_url
API_URL=http://your_gateway_service_url/
ID_URL=http://your_identity_service_url
NEXT_PUBLIC_NOTIFY_URL=http://your_notification_service_url/notifications
```

Replace the placeholder values with your actual data:

- `NEXTAUTH_SECRET`: A secret key used by NextAuth for encryption.
- `NEXTAUTH_URL`: The URL where your Next.js application is running.
- `API_URL`: The URL of your Gateway service.
- `ID_URL`: The URL of your Identity service.
- `NEXT_PUBLIC_NOTIFY_URL`: The URL of your Notification service.

Remember, never commit your `.env.local` file to the repository. It contains sensitive information that should not be shared publicly. The `.gitignore` file should already include `.env.local` to prevent it from being committed. If it doesn't, add `.env.local` to `.gitignore`.

## Microservices Configuration

Each microservice in the application has its own configuration files, typically named `appsettings.json` or similar. These files contain settings specific to that microservice, such as database connection strings, service URLs, and other configuration options.

You will need to adjust these settings according to your environment. For example, if you're running the services locally, you might for example set the database connection string to `mongodb://localhost:27017`. If you're deploying to a cloud environment, you would use the connection string provided by your cloud database service.

Remember to keep sensitive information out of your codebase. Use environment variables or secure configuration services to manage this data.