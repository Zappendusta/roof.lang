FROM mcr.microsoft.com/devcontainers/dotnet:9.0-bookworm

# Install dependencies
RUN apt-get update && \
    apt-get install -y \
    nasm

# Set the working directory
WORKDIR /workspace