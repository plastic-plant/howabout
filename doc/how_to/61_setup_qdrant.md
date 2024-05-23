### Setting up Qdrant

See [Qdrant Quickstart](https://qdrant.tech/documentation/quick-start/) for various ways to install and start the vector database. If you have Docker installed, it's as simple as:

```bash
docker pull qdrant/qdrant
docker run -p 6333:6333 -v $(pwd)/qdrant_storage:/qdrant/storage:z qdrant/qdrant
```


#### Configure appsettings.json

Configure the endpoint for Qdrant API in Howabout, restart and run.

```json
appsettings.json

{
  "Providers": {
    "Persistence": {
      "Storage": "Qdrant",
      "Endpoint": "http://localhost:6333",
      "APIKey": ""
    }
  }
}
```


#### Dashboard

Open its dashboard at http://localhost:6333/dashboard.


#### Good to know

The local deployment is free and you won't need an api-key for the local deployment. Should you wish to run remote in Qdrant Cloud, you can set up an api-key in [Cloud dashboard](https://qdrant.tech/documentation/cloud/authentication/?q=apikey) and add it to appsettings.json. The cloud hosted version comes with subscriptions, but there is a 1GB free forever subscription too that doesn't even require a credit card. Pretty compelling options to get started.
