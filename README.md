# Weather Forecasting Application

This project is a weather forecasting application designed using C# and .NET. It integrates external weather services, Redis caching, and database management to provide real-time weather updates and historical data access.

## Features

- **Real-Time Weather Updates**: Fetches data from external weather APIs.
- **Caching**: Uses Redis to cache frequently accessed weather data.
- **Database Support**: Stores historical data in a database for fallback in case of external API failures.
- **Dockerized**: Includes a Dockerfile for containerization and easy deployment.

## Project Structure

- **Api**: Handles API requests and responses for weather data.
- **Core**: Contains business logic and core functionalities.
- **Common**: Shared utilities and configurations.
- **Infra**: Infrastructure-related code (e.g., Redis, database integrations).

## Prerequisites

- .NET SDK
- Redis server
- Docker (optional for containerized deployment)

## Getting Started

1. Clone the repository:
   ```bash
   git clone https://github.com/Al1r3z4asadi/weatherForcasting.git
   cd weatherForcasting
   ```
2. Configure `appsettings.json` with your Redis connection string and external API credentials.
3. Run the project:
   ```bash
   dotnet run
   ```
4. Access the application via the provided API endpoints.

## Deployment

To deploy using Docker:
1. Build the Docker image:
   ```bash
   docker build -t weather-forecasting .
   ```
2. Run the container:
   ```bash
   docker run -p 8080:80 weather-forecasting
   ```

## Future Improvements

- Enhanced error handling and logging.
- Integration with additional weather APIs.
- Advanced UI for data visualization.
