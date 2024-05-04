using Howabout.Interfaces;
using System.Diagnostics;

namespace Howabout.Repositories
{
	public class ShellCommand : IShellCommand
	{
		public async Task<string> Execute(string command, string arguments)
		{
			var process = new Process()
			{
				StartInfo = new ProcessStartInfo
				{
					FileName = command,
					Arguments = arguments,
					RedirectStandardOutput = true,
					UseShellExecute = false,
					CreateNoWindow = true
				}
			};

			process.Start();
			string result = await process.StandardOutput.ReadToEndAsync();
			process.WaitForExit();
			return result;
		}
	}
}
