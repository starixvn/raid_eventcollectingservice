#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

# Stage 1: Build the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

RUN apt-get update && apt-get install -y iputils-ping && rm -rf /var/lib/apt/lists/*
# Copy project file
COPY ["eventcollectingservice.csproj", "./"]

# Restore dependencies
RUN dotnet restore "eventcollectingservice.csproj"

# Copy the rest of the source code
COPY . .

# Build and publish the application
RUN dotnet publish "eventcollectingservice.csproj" -c Release -o /app/publish

# Stage 2: Create the final image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "eventcollectingservice.dll"]
