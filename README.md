Answers questions about your documents with large language models. Proof of concept, testing.

```bash
howabout start

howabout add mydocument.txt
howabout add myblogwiki.txt
howabout add newproduct.pdf
howabout add whitepaper.docx --tag Documents --tag Books

howabout ask Can you find the document about the new product launch?
howabout ask Can you give me a summary?
howabout ask How much will the product cost?
howabout ask Which influencers endorse the Hairdryer 3000?

howabout stop
```



![Browser window showing chat](/doc/images/howabout-chat.png)


### Configuration

You can configure OpenAI or setup a provider compatible with the OpenAI API (like [LM Studio](https://lmstudio.ai), [Jan](https://jan.ai/)) to pass requests to models (download from [Hugging Face](https://huggingface.co/)). As of yet I didn't have satisfying results with smaller local models for word embedding yet. Currently testing with OpenAI and more models. Some documentation and best practices soon.

By default a web chat is configured at http://localhost:5000/. During development, [Vite](https://vitejs.dev/) serves the React frontend at http://localhost:5001/.


```json
appsettings.json

{
  "Urls": "http://localhost:5000/",
  "Providers": {
    "Completions": {
      "Endpoint": "http://localhost:1234/v1/",
      "TextModel": "gemma-2b-it",
      "TextModelMaxTokenTotal": 8192,
      "APIKey": "lm-studio"
    },
    "Embeddings": {
      "Endpoint": "http://localhost:1234/v1/",
      "EmbeddingModel": "nomic-embed-text-v1.5.Q4_0.gguf",
      "EmbeddingModelMaxTokenTotal": 8192,
      "APIKey": "lm-studio"
    },
    "Partitioning": {
      "OverlappingTokens": 100
    }
  }
}
```


### Command line
```bash
Usage: howabout <command> [options]

Commands:
  help      Display help information.
  start     Start the server.
  stop      Stop the server.
  add       Add documents to the server.
  ask       Ask questions.

You can upload documents by name, folder or url:

  howabout add file.txt
  howabout add file1.txt file2.pdf file3.docx
  howabout add ..\docs\file1.txt ../docs/file2.pdf
  howabout add C:\docs\file1.txt /home/user/docs/file2.pdf
  howabout add C:\docs D:\books\
  howabout add /home/user/docs
  howabout add https://server/file2.pdf

You can group documents with a tag or multiple tags:

  howabout add file.txt --tag Documents
  howabout add file.pdf --tag Documents --tag Books

You can ask questions about the documents:

  ask Can you give me a summary?
  ask 'What do given documents have in common?'
  ask "Who is the protagonist in this story?"

You can ask for citations about the source of the answer:

  ask Is there a summary in the books? --cite
  ask Why did the wizard come to town? --cite


```

![Browser window showing backend-for-frontend API endpoints in Swagger](/doc/images/howabout-swagger.png)