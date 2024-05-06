using System;
using System.Threading.Tasks;
using static SimpleExec.Command;

namespace Make.Config
{
	public class Solution
	{
		public static async Task<BuildConfig> BuildAsync(BuildConfig config)
		{
			await RunAsync("dotnet", $"clean {config.ProjectFilePath}");

			switch (config.PublishOptions.Package)
			{
				// https://github.com/quamotion/dotnet-packaging
				case PackageType.Deb:
				case PackageType.Rpm:
					var packageType = Enum.GetName(typeof(PackageType), config.PublishOptions.Package).ToLower(); // deb rpm
					await RunAsync("dotnet", $"{packageType} --configuration {config.PublishOptions.Configuration} --runtime {config.PublishOptions.Runtime} {config.ProjectFilePath}");
					break;

				// https://learn.microsoft.com/en-us/dotnet/core/tools/dotnet-publish
				case PackageType.TarGz:
				case PackageType.Zip:
				case PackageType.None:
				default:
					await RunAsync("dotnet", $"publish {config.SolutionFilePath} --configuration {config.PublishOptions.Configuration} --runtime {config.PublishOptions.Runtime} --self-contained {config.PublishOptions.SelfContainedString}");
					break;
			}
			return config;
		}
	}
}
