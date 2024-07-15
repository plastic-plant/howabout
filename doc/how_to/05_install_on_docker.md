### How to install on Docker

Docker image is not dropped in a registry like Docker Hub, but it's easy to build from source and I do have an [OCI image](https://opencontainers.org/) in the releases folder if you prefer to import the image over building.

- Download [howabout-1.5.0.docker.tar.gz](https://github.com/plastic-plant/howabout/releases/download/v1.5.0/howabout-1.5.0.docker.tar.gz) with a default OCI image for x86/AMD 64bit architecture that runs almost everywhere.
- Download [howabout-1.5.0.docker-arm.tar.gz](https://github.com/plastic-plant/howabout/releases/download/v1.5.0/howabout-1.5.0.docker-arm.tar.gz) with an OCI image for ARM 64bit architecture like Apple Silicon, Raspberry Pi, Snapdragon.

Keep in mind that you can run the default x86-amd image on all Docker setups, due to virtualisation. The ARM image is optimized for ARM processors and that may be useful, but you don't need it on your Snapdragon for some demoing.


#### Import the image

You can import the image from releases folder, or build it from source. Then run as usual with Docker, Docker Compose or Kubernetes.

```bash
wget (https://github.com/plastic-plant/howabout/releases/download/v1.5.0/howabout-1.5.0.docker.tar.gz
docker load --input howabout-1.5.0.docker.tar.gz
```


#### Build from source

Build a Docker image with Alpine Linux for x86/AMD 64bit architecture. That's the default.

```bash
docker build -t howabout/howabout .
```

As an alternative, you can build on top of another base image with another runtime. For example, build for ARM 64bit (Apple Silicon, Raspberry, Snapdragon) available in the releases. I've added some build arguments for that.

```bash
docker build -t howabout/howabout . --build-arg RUNTIME=osx-arm64 --build-arg BASEIMG=mcr.microsoft.com/dotnet/aspnet:8.0.3-alpine3.18-arm64v8
```

#### Run with Docker

Here's how to run with Docker on port 5000. Run again with `howabout help` to see the options and keep running with `howabout start`. (With `--detach` you get your prompt back.) You can then open a browser and navigate to http://localhost:5000.

```bash
docker run -p 5000:5000 howabout/howabout
docker run -p 5000:5000 howabout/howabout help
docker run -p 5000:5000 howabout/howabout start --detach
docker ps
```

#### Run with Docker Compose

See [Dockerfile](../../src/) and [compose.yaml](../../src/compose.yaml) in the `src` folder. On docker compose up, `howabout start` is invoked, keeps running until you docker compose down.
```bash
docker compose up --build --detach
docker compose ps
docker compose logs
docker compose down
```

You can point your browser to http://localhost:5000 to demo or change the port in the compose file.