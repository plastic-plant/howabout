# syntax=docker/dockerfile:1.7.0

# ###############################################################################
#
#  Builds Docker image with Alpine Linux for x86/AMD 64bit architecture (default)
#  or alternatively builds for ARM 64bit (Apple Silicon, Raspberry, Snapdragon).
#
#    docker build -t howabout/howabout .
#    docker build -t howabout/howabout . --build-arg RUNTIME=linux-musl-x64 --build-arg BASEIMG=mcr.microsoft.com/dotnet/aspnet:8.0.3-alpine3.18-amd64
#    docker build -t howabout/howabout . --build-arg RUNTIME=linux-arm64 --build-arg BASEIMG=mcr.microsoft.com/dotnet/aspnet:8.0.3-alpine3.18-arm64v8
#
#  Then run image with docker run or compose:
#
#    docker run -p 5000:5000 howabout/howabout
#    docker run -p 5000:5000 howabout/howabout help
#    docker run -p 5000:5000 howabout/howabout start --detach
#    docker ps
#
#    docker compose up --detach
#    docker compose ps
#    docker compose logs
#    docker compose down
#
# ###############################################################################

ARG BASEIMG=mcr.microsoft.com/dotnet/aspnet:8.0.3-alpine3.18-amd64


# Build backend with .NET SDK, which depends on a build of frontend with Node.js.
FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build
ARG RUNTIME=linux-musl-x64
COPY . /src
WORKDIR /src
RUN apk add nodejs npm
RUN dotnet publish backend --configuration Release --runtime ${RUNTIME} --no-self-contained --property:PublishDir=/app

# Build image.
FROM ${BASEIMG} AS image
WORKDIR /app
COPY --from=build /app .

# Howabout executes under non-root user at startup.
USER $APP_UID
ENTRYPOINT ["dotnet", "howabout.dll"]
