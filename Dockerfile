# Use a base image with Bash installed
FROM ubuntu:latest

# Update package lists and install Bash
RUN apt-get update && apt-get install -y \
    bash

# Create a directory for the Server files
RUN mkdir /usr/local/Server

# Copy the Server directory into the container
COPY Server /usr/local/Server/

# Set permissions for the copied files
RUN chmod +x /usr/local/Server/Server.x86_64

# Set the working directory
WORKDIR /usr/local/Server

# Set the default command to run when the container starts
CMD ["./Server.x86_64"]

