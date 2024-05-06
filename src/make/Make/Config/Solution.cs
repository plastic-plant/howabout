using System.Threading.Tasks;
using static SimpleExec.Command;

namespace Make.Config
{
	public class Solution
	{
		public static async Task<BuildConfig> BuildAsync(BuildConfig config)
		{
			await RunAsync("dotnet", $"clean {config.SolutionFilePath}");
			await RunAsync("dotnet", $"publish {config.SolutionFilePath} --configuration {config.PublishOptions.Configuration} --runtime {config.PublishOptions.Runtime} --self-contained {config.PublishOptions.SelfContainedString}");
			return config;
		}
	}
}
