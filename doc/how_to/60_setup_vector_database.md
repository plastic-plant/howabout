### How to set up a vector database

Optionally supports vector database. By default, Howabout keeps uploaded documents in-memory. The text of documents and embeddings can be stored as temporary files on disk too, and you have the option to use an external vector database for documents you upload. [Configure appsettings.json](doc/how_to/30_configure_appsettings.md) shows an overview of the options available, here's the gist:


#### Keep vectors in memory (default)

By default Howabout keeps embedding vectors in memory without storing them on disk. You can set `Providers`.`Persistence`.`Storage` to `None` or leave it out as it's the default. when you restart the application, the generated embeddings for previously uploaded documents are lost. Good for some testing.

```json
appsettings.json

{
  "Providers": {
    "Persistence": {
      "Storage": "None"
    }
  }
}
```

#### Keep vectors in a local folder

Alternatively, you can have the embeddings stored in local files. Use the following configuration (and you can leave out Directory as it's the default). This is useful for keeping embeddings between restarts.

```json
appsettings.json

{
  "Providers": {
    "Persistence": {
      "Storage": "File",
      "Directory": "_tmp"
    }
  }
}
```

#### Keep vectors in a (remote) database

You can also store embeddings in a database server. This is useful for keeping embeddings between restarts, and makes sure that the embeddings can be shared between multiple instances of Howabout. Generally, if you run a Kubernetes cluster, you'll probably want a seperate database for storing and matching vectors. You may even have a dedicated remote service for that.

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


### Supported vector databases

- [Setting up Qdrant](doc/how_to/61_setup_qdrant.md)] as vector database.
