namespace Howabout.Interfaces
{
	public interface IShellCommand
	{
		Task<string> Execute(string command, string arguments);
	}
}
