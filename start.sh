#!/bin/bash
dapr run --app-id demo --app-port 5000 -d src/Application/dapr-components -- dotnet run --project src/Application/Application.csproj
