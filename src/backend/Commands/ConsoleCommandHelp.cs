using Howabout.Configuration;
using Howabout.Interfaces;
using Serilog;

namespace Howabout.Commands
{
	public class ConsoleCommandHelp: IConsoleCommand
	{
		private readonly ConsoleStartupArguments _args;

        public ConsoleCommandHelp(ConsoleStartupArguments args)
        {
			_args = args ?? throw new ArgumentNullException(nameof(args));            
        }

        public Task Verify()
		{
			return Task.CompletedTask;
		}

		public Task Execute()
		{
			Log.Information(@"
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

You can ask questions about the documents:

  ask Can you give me a summary?
  ask 'What do given documents have in common?'
  ask ""Who is the protagonist in this story?""

You can ask for citations about the source of the answer:

  ask Is there a summary in the books? --cite
  ask Why did the wizard come to town? --cite

");

			return Task.CompletedTask;
		}
	}
}
