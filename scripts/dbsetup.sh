#!/bin/bash

# Define the name of the container
CONTAINER_NAME="cricket_container"

# Define the name of the image to use
IMAGE_NAME="postgres:latest"

# Define the name of the database to create
DB_NAME="cricket_database"

# Define the password for the PostgreSQL server
POSTGRES_PASSWORD="admin"

# Run the container and create the database
docker run -d --name $CONTAINER_NAME -e POSTGRES_PASSWORD=$POSTGRES_PASSWORD $IMAGE_NAME

# Get the container IP address
CONTAINER_IP=$(docker inspect -f '{{range .NetworkSettings.Networks}}{{.IPAddress}}{{end}}' $CONTAINER_NAME)

# Create the connection string
CONNECTION_STRING="Server=$CONTAINER_IP;Port=5433;Database=$DB_NAME;User ID=postgres;Password=$POSTGRES_PASSWORD"

# Output the connection string to a log file
echo $CONNECTION_STRING > ./cs.log

docker exec -it $CONTAINER_NAME psql -U postgres -c "CREATE DATABASE $DB_NAME;"
