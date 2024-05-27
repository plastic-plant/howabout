### Configure appsettings.json

Configuration is done in the `appsettings.json` file. The file is located in the root of the application and divided into sections, a simplified version looks like this.

| Section        | Description |
| ---            | --- |
| `Urls`         | Configures the URL that the chat server will listen on (`howabout start`). |
| `AllowedHosts` | Configures the hosts that the server will accept requests from. |
| `Providers`    | The configuration for the different providers. |
| `Logging`      | Logging configuration for commands (`howabout help`, `howabout ask`). |
| `Serilog`      | Logging configuration for built-in chat server (`howabout start`). |

Just open the file in a text editor and change the values to your liking. The default values should get you started.

```json
appsettings.json

{
  "Urls": "http://localhost:5000/",
  "AllowedHosts": "*",

  "Providers": {
    "Partitioning": {},
    "Persistence": {},
    "Embeddings": {},
    "Completions": {}
  },

  "Logging": {},
  "Serilog": {}
}
```

#### AllowedHosts and Urls

When you run `howabout start`, a web-api server will start listening on the URL specified in the `Urls` section. When you open a web browser and navigate to the URL, you can chat with Howabout. Of course you can chat with Howabout in the console as well. When you run a command like `howabout ask`, the command line client will send a request to the server specified in the `Urls` section. If that's useful to you, you can configure multiple URLs by separating them with a semicolon. For example when you want the chat server to be available on both https and http. If you do not specify a port number, the server will listen on port 80 for http and 443 for https. By default we have the server listen on localhost port 5000 (`http://localhost:5000/`), but it's likely that you'll want to change this. For when you run behind a proxy, for example in Kubernetes.

The server will accept requests from the hosts specified in the `AllowedHosts` section. By default we allow all hosts (`*`) and. You can configure multiple AllowedHosts with a comma (`,`) to machines you intent to call the server with the webbrowser from. Note that you can specify host names, ip addresses and use wildcards (`*`) in the AllowedHosts section, but you can't use port numbers.


#### Providers

| Section      | Description |
| ---          | --- |
| Persistence  | Configuration for optionally storing vectors in a database. |
| Partitioning | Configuration for token limits while processing embeddings. |
| Embeddings   | Configuration for embeddings generator model provider. |
| Completions  | Configuration for completions chat model provider. |


```json
appsettings.json

{
  "Providers": {
    "Persistence": {
      "Storage": "File",
      "Directory": "_tmp",
      "Endpoint": "http://localhost:6333",
      "APIKey": ""
    },
    "Partitioning": {
      "MaxTokensPerParagraph": 1000,
      "MaxTokensPerLine": 300,
      "OverlappingTokens": 100
    },
    "Embeddings": {
      "Provider": "OpenAI",
      "Endpoint": "http://localhost:1234/v1/",
      "EmbeddingModel": "lm-studio",
      "EmbeddingModelMaxTokenTotal": 2048,
      "APIKey": "lm-studio",
      "OrgId": "",
      "MaxRetries": 10
    },
    "Completions": {
      "Provider": "OpenAI",
      "Endpoint": "http://localhost:1234/v1/",
      "TextModel": "lm-studio",
      "TextModelMaxTokenTotal": 32768,
      "APIKey": "lm-studio",
      "OrgId": "",
      "MaxRetries": 10
    }
  },
}
```

##### Providers > Persistence

Howabout let's you add documents and processes these into a list of searchable embeddings. By default the app keeps the processed uploads in memory; once you `howabout stop` that's gone. Useful for testing. You can choose to have the embeddings saved to local files or a vector database too. Saves some memory and that way your processed files are available next time the application starts.

You have the following options available:

| Section               | Option    | Description |
| ---                   | ---       | --- |
| Persistence.Provider  | None      | Default; does not persist to storage, keeps in memory. |
|                       | File      | Stores embeddings in vector files on local disk. |
|                       | Qdrant    | Stores embeddings in Qdrant vector database. |
|                       |           |
| Persistence.Directory | directory | When `File` provider chosen, this option overrides the default `_vectors` folder. |
|                       |           |
| Persistence.Endpoint  | url       | When `Qdrant` provider chosen, this option sets an endpoint to server. |
| Persistence.APIKey    | api-key   | When `Qdrant` provider chosen, this option sets an optional api-key for access. |

For example, to keep the **embeddings in memory**, use the following configuration (or fully leave out as it's the default):
```json
{
  "Providers": {
    "Persistence": {
      "Provider": "None"
    }
  }
}
```

For example, to store the **embeddings in local files**, use the following configuration (and you can leave out Directory as it's the default):
```json
{
  "Providers": {
    "Persistence": {
      "Provider": "File",
      "Directory": "_tmp"
    }
  }
}
```

For example, to store the **embeddings in remote database**, use the following configuration:
```json
{
  "Providers": {
    "Persistence": {
      "Provider": "Qdrant",
      "Endpoint": "http://localhost:6333",
      "APIKey": ""
    }
  }
}
```

See [Setting up Qdrant](doc/how_to/61_setup_qdrant.md)] as vector database.


##### Providers > Partitioning

Howabout chops up your documents after tokenization and passes chunks of tokens to models while storing an embedding. This section allows you to configure the token limits while processing embeddings. Follows [Kernel Memory defaults](https://microsoft.github.io/kernel-memory/how-to/custom-partitioning). You can go with the following default settings or override with some custom partitioning that fits your model or setup better.


| Section                            | Default |
| ---                                | ---     |
| Partitioning.MaxTokensPerParagraph | 1000    |
| Partitioning.MaxTokensPerLine      | 300     |
| Partitioning.OverlappingTokens     | 100     |

```json
{
  "Providers": {
    "Partitioning": {
      "MaxTokensPerParagraph": 1000,
      "MaxTokensPerLine": 300,
      "OverlappingTokens": 100
    },
  }
}
```

##### Providers > Embeddings and Completions

Two language models are used for processing text: embeddings and completions. For each model you can configure an endpoint to a remote model provider, for example a test server running in LM Studio. The embedding endpoint convert text into numerical vectors for semantic matching, while completions endpoints generate text based on prompts. When you upload a document, the embeddings model is used to convert the text into embeddings. When you ask a question, the completions model is used to generate text based on the question and the most closest embeddings of documents.

Here's an example of a configuration that leaves the selection of both models to LM Studio runnimg at default port 1234. You can also configure the maximum token total for the models, the API key and the organization id. The maximum retries is the number of times the application will retry a request to the model provider before giving up.


```json
appsettings.json

{
  "Providers": {
    "Embeddings": {
      "Provider": "OpenAI",
      "Endpoint": "http://localhost:1234/v1/",
      "EmbeddingModel": "lm-studio",
      "EmbeddingModelMaxTokenTotal": 2048,
      "APIKey": "lm-studio",
      "OrgId": "",
      "MaxRetries": 10
    },
    "Completions": {
      "Provider": "OpenAI",
      "Endpoint": "http://localhost:1234/v1/",
      "TextModel": "lm-studio",
      "TextModelMaxTokenTotal": 32768,
      "APIKey": "lm-studio",
      "OrgId": "",
      "MaxRetries": 10
    }
  },
}
```

Read more on [How to setup a model](doc/how_to/50_setup_model.md) for choosing a model and configuring a provider.


### Logging and Serilog

The application is built with the [Serilog](https://serilog.net/) logging provider on top of [Microsoft.Logging](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/logging/?view=aspnetcore-8.0). Long story short: there are two templates that allow you to have fine-grained control over the log output of command-line actions (`Logging`) and server (`Serilog`). Have a look at the documentation for Serilog and Microsoft.Logging for more information.

```json
appsettings.json

{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning",
      "Microsoft.Hosting": "Warning",
      "Microsoft.KernelMemory": "Warning"
    },
    "Console": {
      "FormatterName": "Console",
      "FormatterOptions": {
        "SingleLine": true,
        "OutputTemplate": "{Timestamp:HH:mm} {Message:lj}{NewLine}{Exception}"
      }
    }
  },

  "Serilog": {
    "MinimumLevel": {
      "Default": "Information",
      "Override": {
        "Microsoft.AspNetCore": "Warning",
        "Microsoft.Hosting": "Warning",
        "Microsoft.KernelMemory": "Warning"
      }
    },
    "WriteTo": [
      {
        "Name": "Console",
        "Args": {
          "theme": "Serilog.Sinks.SystemConsole.Themes.AnsiConsoleTheme::Code, Serilog.Sinks.Console",
          "outputTemplate": "{Timestamp:HH:mm} {Message:lj}{NewLine}{Exception}"
        }
      }
    ]
  }
}
```

Here's how you can have Serilog configigured to have the chat server log written to [sinks for specific log writers](https://github.com/serilog/serilog/wiki/Provided-Sinks) like Amazon, Azure, ElasticSearch, Splunk, Sentry, NewRelic, etc. For example, a sink for writing logs to a text file `log-{date}.txt` that adds a new log file every day:

```json
appsettings.json

{
  "Serilog": {
    "MinimumLevel": {
      "Default": "Information"
    },
    "WriteTo": [
      {
        "Name": "Console",
        "Args": {
          "theme": "Serilog.Sinks.SystemConsole.Themes.AnsiConsoleTheme::Code, Serilog.Sinks.Console",
          "outputTemplate": "{Timestamp:HH:mm} {Message:lj}{NewLine}{Exception}"
        }
      },
      {
        "Name": "File",
        "Args": {
          "path": "logs/log-.txt",
          "rollingInterval": "Day",
          "outputTemplate": "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level}] {Message:lj}{NewLine}{Exception}"
        }
      }
    ]
  }
}
```



### Environment variables

Apart from appsettings.json, you can also configure application settings using environment variables. The environment variables are read by the application at startup and override the values in the appsettings.json file. The environment variables are case-insensitive. Useful when running a Docker image, etc. At the moment one environment variable is available, mostly in use for debugging.

| Environment variable | Example | Description |
| ---                  | ---     | ---         |
| ASPNETCORE_URLS      | http://localhost:5000/ | The URL that the chat server will listen on. |

```bash
Linux, macOS

export ASPNETCORE_URLS=http://localhost:5000/
```

```batch
Windows Command Prompt

set ASPNETCORE_URLS=http://localhost:5000/
```

```powershell
Windows PowerShell

$env:ASPNETCORE_URLS = "http://localhost:5000/"
```

```yaml
Dockerfile

ENV: ASPNETCORE_URLS=http://localhost:5000/
```
