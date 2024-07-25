Answers questions about your documents with language models. Howabout starts a small webserver and let's you chat in a web browser or in the console. Show-and-tell: a playground for retrieval-augmented document search.

![Browser window showing chat](doc/images/howabout-chat.png)

### How to use

Install on [Linux](doc/how_to/01_install_on_linux.md), [macOS](doc/how_to/02_install_on_macos.md), [Windows](doc/how_to/03_install_on_windows.md), [Docker](doc/how_to/05_install_on_docker.md), [Kubernetes](doc/how_to/06_install_on_kubernetes.md). Start the application and open [http://localhost:5000](http://localhost:5000) in a browser. Alternatively, build from source and run with [Visual Studio IDE](doc/how_to/20_build_with_visual_studio_ide.md), [Visual Studio Code](doc/how_to/21_build_with_visual_studio_code.md), [JetBrains Rider](doc/how_to/22_build_with_rider.md), [dotnet-cli](doc/how_to/23_build_with_dotnet_cli.md) or [dotnet-sdk containers](doc/how_to/24_build_with_docker.md).

```batch
howabout start
howabout add mydocument.txt
howabout ask Can you give me a summary?
```

Howabout is configured in `appsettings.json`. Both through the command line and the web interface, you can add documents, ask questions, and get answers. The app uses language models to find information and answer questions about your documents. You can choose between different models and providers.

- [Use command line.](doc/how_to/40_use_command_line.md)
- [Use web browser.](doc/how_to/41_use_web_browser.md)


The aim of this app is to have a demoable playground for RAG written in C# that is flexible enough to be used in Kubernetes, Linux and Windows. That means there's a bit of work to do after downloading a release from the menu. Before you run Howabout for the first time, configure a model. You can configure additional options too. Here's how.

- [Configure appsettings.json](doc/how_to/30_configure_appsettings.md) and environment variables.
- [Choosing language models](doc/how_to/31_choosing_language_models.md) that work well with RAG search.
- [Setting up Qdrant](doc/how_to/61_setup_qdrant.md) as an optional vector database.


See [How to install latest release](doc/how_to/00.md) and read [How to setup a model](doc/how_to/50_setup_model.md) with remote providers like OpenAI and OpenRouter, and local providers like LM Studio, Jan.ai, etc. Supports local GGUF models invoking Llama.cpp too. There's an option to help you download models from Hugging Face, private Git repos, local and remote locations. Cheers!
