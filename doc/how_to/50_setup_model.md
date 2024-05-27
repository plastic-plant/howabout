### How to setup a model

You can configure OpenAI or setup a provider compatible with the OpenAI API (like [LM Studio](https://lmstudio.ai), [Jan](https://jan.ai/)) to pass requests to models (download from [Hugging Face](https://huggingface.co/)).


### Supported model providers 

Supports remote models through [OpenAI API](https://platform.openai.com/docs/models) endpoints. These are blazing fast, cheap and an excellent fit for Microsoft Kernel Memory, but requires an account and some credits.

- [Setting up OpenAI](doc/54_setup_openai.md) as remote model provider.

Supports remote models through compatible APIs by providers such as [OpenRouter](https://openrouter.ai/docs#models). Some models are free to experiment, but require an account.

- [Setting up OpenRouter](doc/55_setup_openrouter.md) as remote model provider.

Supports local model providers that expose OpenAI API compatible endpoints for [embeddings](https://platform.openai.com/docs/api-reference/embeddings) and [completions](https://platform.openai.com/docs/api-reference/chat). These providers wrap [Llama.cpp](https://github.com/ggerganov/llama.cpp) and make downloading, configuring and switching between models for testing easy.

- [Setting up LM Studio](doc/how_to/52_setup_lmstudio.md) as local model provider.
- [Setting up Jan](doc/53_setup_jan.md) as local model provider.

Supports local models in [GGUF format](https://huggingface.co/docs/hub/en/gguf) with Llama directly soon. Simply [download and configure a model from HuggingFace](doc/how_to/51_setup_llama.md). Available soon.
