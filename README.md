
# PixelBid: Microservices Video Game Auction Web App

Welcome to PixelBid, a microservices-based web application for auctioning video games. This project is structured around multiple microservices, each handling a specific part of the application, from auction handling to user authentication and gateway management.

## Getting Started

## Prerequisites

Before you start, ensure you have Docker installed on your system, as it includes Docker Compose needed for running multi-container applications.

To verify Docker and Docker Compose are installed, run the following commands in your terminal:

```bash
docker --version
docker compose version


### Setup

Clone the repository to your local machine:

```bash
git clone https://github.com/yourgithub/pixelbid.git
cd pixelbid
```

### Running the Application

Use Docker Compose to run all the services:

```bash
docker-compose up --build
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

Also, add the line below to your hosts file:
127.0.0.1 id.pixelbid.com app.pixelbid.com api.pixelbid.com

