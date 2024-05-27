### How to use Howabout with command line instructions

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

### How to use

Simply start the server:
```batch
howabout start
```
You can now open a browser and navigate to http://localhost:5000/ to chat with Howabout. Both in browser and in console you may add documents and ask questions:

```bash
howabout add mydocument.txt
howabout ask Can you give me a summary?
```