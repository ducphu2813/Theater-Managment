
# Movie Theater Management System

This project is designed to explore the microservice architecture and gain practical experience in building services using .NET Core API. The primary goals of this project are:

- **Understanding Microservice Architecture**: Learning the principles and patterns of microservice-based systems.
- **Building Services in Docker Environment**: Developing and deploying services in Docker containers.
- **Resource Management**: Understanding how to control and optimize resources in a containerized environment.
- **Service Communication**: Implementing inter-service communication patterns.
- **Performance Optimization**: Learning how to monitor and optimize the performance of microservices.
  This project aims to provide hands-on experience in working with Docker, .NET Core, and microservice principles, offering a practical approach to building scalable, maintainable, and efficient distributed systems.

## Tech Stack

**Server:** .NET Core, Docker, RabbitMQ, MongoDB, JWT

## Features

- **User Management**: Register, login, and manage user accounts.
- **Movie Management**: Add, update, and delete movies.
- **Movie Schedule Management**: Create, update, check schedules for movies, including showtimes and room number.
- **Booking Management**: Book tickets for movies.
- **Payment Gateway**: Process payments for bookings.

## Installation

This project requires [.NET Core](https://dotnet.microsoft.com/download) and [Docker](https://www.docker.com/products/docker-desktop) to run.

1. Clone the repository:

```bash
git clone
```

2. Navigate to the project directory


3. Run the following command to start the services:

```bash
docker-compose build
docker-compose up
```

4. Import the `TheaterManagement.postman_collection.json` file into Postman to test the APIs.