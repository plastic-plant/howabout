namespace Howabout.Interfaces
{
	public interface IConsoleCommand
	{
		Task Verify();
		Task Execute();
	}
}
