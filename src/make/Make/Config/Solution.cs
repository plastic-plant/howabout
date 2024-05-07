using System;
using System.IO;
using System.Reflection;
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

				// https://jrsoftware.org/isinfo.php
				case PackageType.Exe:
					await RunInnoCompiler(config);
					break;

				// https://en.wikipedia.org/wiki/Bundle_(macOS)
				case PackageType.App:
					await RunAsync("dotnet", $"publish {config.SolutionFilePath} --configuration {config.PublishOptions.Configuration} --runtime {config.PublishOptions.Runtime} --self-contained {config.PublishOptions.SelfContainedString}");
					WrapInMacOsAppHierarchy(config);
					break;

				// https://github.com/create-dmg/create-dmg
				case PackageType.Dmg:
					await RunAsync("dotnet", $"publish {config.SolutionFilePath} --configuration {config.PublishOptions.Configuration} --runtime {config.PublishOptions.Runtime} --self-contained {config.PublishOptions.SelfContainedString}");
					WrapInMacOsAppHierarchy(config);
					// TODO: create-dmg for Linux and macOS.
					// await RunAsync("create-dmg", $"--volname \"Howabout Installer\" \"{config.BuildArtifactsFolderPath}/howabout-v{typeof(Program).Assembly.GetName()?.Version?.ToString(2)}.dmg\" \"{config.BuildArtifactsFolderPath}/howabout.app\"");
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

		// We can put this in a tarball and install by extracting to /Applications/ folder
		// or we can package it as .dmg (macOS or Linux) and sign it.
		private static void WrapInMacOsAppHierarchy(BuildConfig config)
		{
			var exeFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
			string templatePath = Path.Combine(exeFolder, "Templates", "macOS.app");
			string outputPath = Path.Combine(config.BuildArtifactsFolderPath, "setup");
			Bundle.CopyDirectory(templatePath, Path.Combine(config.BuildArtifactsFolderPath, "setup", "howabout.app"));
			Bundle.CopyDirectory(Path.Combine(config.BuildArtifactsFolderPath, "publish"), Path.Combine(config.BuildArtifactsFolderPath, "howabout.app", "Contents", "MacOS", "howabout"));

		}

		// TODO: Quick and dirty, gets the job done for now. Nice work on Inno by Jordan Russell
		// and Martijn Laan in Delphi. But win-only and that's fine for PoC. How about wasting
		// some time on latest Wix Toolset? Maybe, if that new v4 documentation ever gets done. :)
		// https://jrsoftware.org/ishelp/topic_setup_architecturesallowed.htm
		// https://jrsoftware.org/ishelp/topic_setup_architecturesinstallin64bitmode.htm
		private static async Task RunInnoCompiler(BuildConfig config)
		{
			var exeFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
			string outputPath = Path.Combine(config.BuildArtifactsFolderPath, "setup");
			string innoPath = @"C:\Program Files (x86)\Inno Setup 6\Compil32.exe";
			string templatePath = Path.Combine(exeFolder, "Templates", "inno-windows-setup-exe.iss");
			string template = File
				.ReadAllText(templatePath)
				.Replace(@"#define MyAppVersion ""0.0""", $@"#define MyAppVersion ""{typeof(Program).Assembly.GetName()?.Version?.ToString(2)}""")
				.Replace(@"#define BuildArtifactsDir = """"", $@"#define BuildArtifactsDir = ""{config.BuildArtifactsFolderPath}""")
				.Replace(@"#define OutputDir = """"", $@"#define OutputDir = ""{outputPath}");
			
			switch (config.PublishOptions.Runtime)
			{
				case "win-x86":
					template = template
						.Replace(@"#define Architecture ""x64""", @"#define Architecture ""x86""")
						.Replace(@"ArchitecturesInstallIn64BitMode={#Architecture}", "");
					break;

				case "win-arm64":
					template = template.Replace(@"#define Architecture ""x64""", @"#define Architecture ""arm64""");
					break;

				case "win-x64":
				default:
					// Script template by default is set to x64, no changes.
					break;
			}
			
			var tempTemplatePath = Path.GetTempFileName();
			File.WriteAllText(tempTemplatePath, template);
			await RunAsync(innoPath, $@"/cc ""{tempTemplatePath}""");
		}
	}
}
