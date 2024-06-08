using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Make.Config
{
	public static class PublishOptionsHelper
	{
		public static List<BuildConfig> ToBuildConfigs(this List<PublishOptions> selected, string repositoryFolderPath, string solutionFilePath, string projectFilePath, string publishFolderPath)
		{
			return selected.Select(config => new BuildConfig
			{
				PublishOptions = config,
				RepositoryFolderpath = repositoryFolderPath,
				SolutionFilePath = solutionFilePath,
				SolutionFolderPath = $"{Path.GetDirectoryName(solutionFilePath)}\\",
				ProjectFilePath = projectFilePath,
				ProjectFolderPath = $"{Path.GetDirectoryName(projectFilePath)}\\",
				BuildArtifactsFolderPath = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(solutionFilePath), $"backend\\bin\\{config.Configuration}\\net8.0\\{config.Runtime}\\")),
				PackagePublishFolderPath = Path.Combine(publishFolderPath, $@"{config.Name}"),
				VersionShort = File.ReadAllText(Path.Combine(repositoryFolderPath, "version")).Trim(),
			}).ToList();
		}

		public static List<PublishOptions> GetAvailablePublishOptions(string[] args)
		{
			return new List<PublishOptions>()
			{
				new() { Name = "docker-amd" , Runtime = "linux-musl-x64", Package = PackageType.Docker },
				new() { Name = "docker-arm" , Runtime = "linux-musl-arm64", Package = PackageType.Docker },
				new() { Name = "linux-x64-tgz" , Runtime = "linux-x64", Package = PackageType.TarGz },
				new() { Name = "linux-x64-deb" , Runtime = "linux-x64", Package = PackageType.Deb },
				new() { Name = "linux-x64-rpm" , Runtime = "linux-x64", Package = PackageType.Rpm },
				new() { Name = "linux-musl-x64-tgz" , Runtime = "linux-musl-x64", Package = PackageType.TarGz },
				new() { Name = "linux-musl-arm64-tgz" , Runtime = "linux-musl-arm64", Package = PackageType.TarGz },
				new() { Name = "linux-arm-tgz" , Runtime = "linux-arm", Package = PackageType.TarGz },
				new() { Name = "linux-arm64-tgz" , Runtime = "linux-arm64", Package = PackageType.TarGz },
				new() { Name = "macos-x64-tgz" , Runtime = "osx-x64", Package = PackageType.TarGz },
				new() { Name = "macos-x64-app" , Runtime = "osx-x64", Package = PackageType.App },
				new() { Name = "macos-x64-dmg" , Runtime = "osx-x64", Package = PackageType.Dmg },
				new() { Name = "macos-arm64-tgz" , Runtime = "osx-arm64", Package = PackageType.TarGz },
				new() { Name = "macos-arm64-app" , Runtime = "osx-arm64", Package = PackageType.App },
				new() { Name = "macos-arm64-dmg" , Runtime = "osx-arm64", Package = PackageType.Dmg },
				new() { Name = "win-x64-zip", Runtime = "win-x64", Package = PackageType.Zip },
				new() { Name = "win-x64-exe", Runtime = "win-x64", Package = PackageType.Exe },
				new() { Name = "win-x86-zip", Runtime = "win-x86", Package = PackageType.Zip  },
				new() { Name = "win-x86-exe", Runtime = "win-x86", Package = PackageType.Exe },
				new() { Name = "win-arm64-zip", Runtime = "win-arm64", Package = PackageType.Zip }				
			};
		}

		public static string GetPublishNameWithoutPackageType(this string publishName)
		{
			return publishName
				.Replace("-app", "")
				.Replace("-deb", "")
				.Replace("-dmg", "")
				.Replace("-exe", "")
				.Replace("-rpm", "")
				.Replace("-tgz", "")
				.Replace("-zip", "");
		}

		/// <summary>
		/// Finds matching build configurations by their name from command line arguments
		/// and removes the --buildconfig configname and -bc configname from the ref args
		/// so that the remaining args can be passed to run targets and dotnet build.
		/// </summary>
		public static List<PublishOptions> TakePublishOptionsFromArguments(ref string[] args)
		{
			var configs = GetAvailablePublishOptions(args);
			var selected = new List<PublishOptions>();

			for (int i = 0; i < args.Length; i++)
			{
				if (args[i].StartsWith("--buildconfig") || args[i].StartsWith("-bc"))
				{
					var nextArgumentIndexForValue = i + 1;
					string buildconfigname = args[nextArgumentIndexForValue];

					// Removes the --buildconfig buildconfigname arguments from args.
					var newArgs = new List<string>(args);
					newArgs.RemoveAt(nextArgumentIndexForValue);
					newArgs.RemoveAt(i);
					args = newArgs.ToArray();

					// Adds all build configurations if selected.
					if (buildconfigname == "all")
					{
						Console.WriteLine("Selected all build configurations.");
						selected.Clear();
						selected.AddRange(configs);
						break;
					}

					// Adds selected build configuration.
					var found = configs.Where(config => config.Name == buildconfigname).FirstOrDefault();
					if (found != null)
					{
						Console.WriteLine($"Selected build configuration: {found.Name}");
						selected.Add(found);
					}
					else
					{
						throw new ArgumentException($"Build configuration '{buildconfigname}' not found.");
					}
				}
			}

			return selected;
		}

		public static string ToLinuxForwardSlashes(this string path)
		{
			return path.Replace("\\", "/");
		}

		public static string WithQuotes(this string path)
		{
			return $"\"{path}\"";
		}
	}
}
