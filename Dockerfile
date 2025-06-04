from ubuntu:latest

# Install dependencies
RUN apt-get update && \
    apt-get install -y \
    nasm

# Set the working directory
WORKDIR /roof