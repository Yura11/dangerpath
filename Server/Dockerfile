# Use a base image with Bash installed
FROM ubuntu:latest

# Update package lists and install Bash
RUN apt-get update && apt-get install -y \
    bash

# Copy Server.x86_64 into the container
COPY Server.x86_64 /usr/local/bin/

# Set permissions for the copied file
RUN chmod +x /usr/local/bin/Server.x86_64

# Set the default command to run when the container starts
CMD ["/usr/local/bin/Server.x86_64"]

