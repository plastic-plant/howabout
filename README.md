Answers questions about your documents with language models. Howabout starts a small webserver and let's you chat in a web browser or in the console.

### How to use?

- [Use command line](doc/how_to/40_use_command_line.md)
- [Use web browser](doc/how_to/41_use_web_browser.md)

```batch
howabout start
howabout add mydocument.txt
howabout ask Can you give me a summary?
```

![Browser window showing chat](doc/images/howabout-chat.png)

### How to install or build yourself?

Install on [Linux](doc/how_to/01_install_on_linux.md), [macOS](doc/how_to/02_install_on_macos.md), [Windows](doc/how_to/03_install_on_windows.md), [Raspberry Pi](doc/how_to/04_install_on_raspberry.md), [Docker](doc/how_to/05_install_on_docker.md), [Kubernetes](doc/how_to/06_install_on_kubernetes.md), [Azure](doc/how_to/07_install_on_azure.md), [AWS](doc/how_to/08_install_on_aws.md), [Google Cloud](doc/how_to/09_install_on_google_cloud.md) or [Heroku](doc/how_to/10_install_on_heroku.md). Simple.

Build and run with [Visual Studio IDE](doc/how_to/20_build_with_visual_studio_ide.md), [Visual Studio Code](doc/how_to/21_build_with_visual_studio_code.md), [JetBrains Rider](doc/how_to/22_build_with_rider.md), [dotnet-cli](doc/how_to/23_build_with_dotnet_cli.md) or  [dotnet-sdk containers](doc/how_to/24_build_with_docker.md). Have a look.


### How to configure?

When you run Howabout for the first time, the app will help you select a language model. You can configure additional options too. Here's how.

- [Configure appsettings.json](doc/how_to/30_configure_appsettings.md) and environment variables.
- [Choosing language models](doc/how_to/31_choosing_language_models.md)
- [Setting up Qdrant](doc/how_to/61_setup_qdrant.md) as an optional vector database.


See [How to setup a model](doc/how_to/50_setup_model.md) with remote providers like OpenAI and OpenRouter, and local providers like LM Studio, Jan, etc. Supports local GGUF models invoking Llama too.
