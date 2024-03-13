#!/bin/bash
dapr run --app-id demo --app-port 5000 -- dotnet run --project src/Application/Application.csproj
