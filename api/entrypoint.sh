#!/bin/bash
set -e

echo "Running migrations"
./migrate --connection "$ConnectionStrings__DbConnectionString"

echo "Starting application"
dotnet API.dll