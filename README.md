# Cosmos Experiment

## Prereq's:

* Have docker-compose installed (Ubuntu)

      sudo snap install docker
      sudo groupadd docker
      sudo gpasswd -a $USER docker
      newgrp docker

* add a hostname for the local cosmosdb for TLS goodness

      sudo vi /etc/hosts

        [ append the following line ]
	    127.0.0.1       cosmosdb

* launch the emulator and copy its self-signed certificate

      cd containers/docker-cosmosdb
      ./launchEmulator.sh

      curl -k https://cosmosdb:8081/_explorer/emulator.pem > ~/emulatorcert.crt
      sudo mv ~/emulatorcert.crt /usr/local/share/ca-certificates/
      update-ca-certificates

## Running:

* launch the Cosmos DB Emulator

      cd containers/docker-cosmosdb
      ./launchEmulator.sh

* (in a separate terminal) run Woldrich.CosmosService

      dotnet run --project ./services/Woldrich.CosmosService/Woldrich.CosmosService.csproj

* (in a separate terminal) initialize the local Cosmos DB - this only needs to be run once or whenever you blow away the database that lives in containers/docker-cosmosdb/cosmosdata.  

      cd services/Woldrich.CosmosService
      ./initializeDatabase.sh

* insert a row by POST'ing to the service:

      cd services/Woldrich.CosmosService
      ./postRecord.sh

  The response that comes back should have a URL of the resource that was created in the `Location` header that works as a GET endpoint

* You can explore the data in local Cosmos at the following:

  https://cosmosdb:8081/_explorer/index.html

  