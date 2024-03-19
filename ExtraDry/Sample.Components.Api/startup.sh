#!/bin/bash

# When running locally using docker compose, CosmosDB is started in an emulator which requires SSL.
# The emulator uses a self-signed certificate which isn't trusted by the ASPNET image.  This script
# downloads the certificate and adds it to the trusted certificates; enabling SSL between the API
# and the CosmosDB emulator.

# Install Curl as it isn't part of ASPNET image
echo "Installing Curl"
apt-get update
apt-get -y install curl

# Find the IP address and URL for the CosmosDB emulator (certificate only works with IP address)
#cosmosip=$(getent ahostsv4 cosmosdb | grep STREAM | head -n 1 | awk '{print $1}')
cosmoscerturl="https://cosmosdb:8081/_explorer/emulator.pem"

# Wait for the CosmosDB emulator to start
echo "Waiting for CosmosDB to start"
until [ $(curl --insecure --silent https://cosmosdb:8081/_explorer/emulator.pem | grep -c CERTIFICATE) -gt 0 ]
do
    printf '.'
    sleep 1
done

# Download the certificate and add it to the trusted certificates
echo "CosmosDB started, downloading certificate"
curl -k -o /usr/local/share/ca-certificates/cosmos.crt $cosmoscerturl
update-ca-certificates
