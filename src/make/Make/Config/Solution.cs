﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using static SimpleExec.Command;

namespace Make.Config
{
	public class Solution
	{
		public static async Task<BuildConfig> CleanAsync(BuildConfig config)
		{
			var binDirectory = Path.Combine(config.ProjectFolderPath, "bin");
			var objDirectory = Path.Combine(config.ProjectFolderPath, "obj");

			if (Directory.Exists(binDirectory))
			{
				Directory.Delete(binDirectory, true);
			}

			if (Directory.Exists(objDirectory))
			{
				Directory.Delete(objDirectory, true);
			}

			await RunAsync("dotnet", $"clean {config.ProjectFilePath} --configuration {config.PublishOptions.Configuration}");
						
			if (Directory.Exists(binDirectory))
			{
				Directory.Delete(binDirectory, true);
			}

			if (Directory.Exists(objDirectory))
			{
				Directory.Delete(objDirectory, true);
			}

			await RunAsync("dotnet", $"restore {config.ProjectFilePath} --runtime {config.PublishOptions.Runtime}");

			return config;
		}

		public static async Task<BuildConfig> BuildAsync(BuildConfig config)
		{
			switch (config.PublishOptions.Package)
			{
				// https://github.com/quamotion/dotnet-packaging
				case PackageType.Deb:
					await RunAsync("dotnet", $"msbuild {config.ProjectFilePath} -target:CreateDeb -property:Version={config.VersionLong} -property:Configuration={config.PublishOptions.Configuration} -property:RuntimeIdentifier={config.PublishOptions.Runtime} -property:OutputPath={config.BuildArtifactsFolderPath};NOOP=NOOP "); // Added ;NOOP=NOOP no-operation option to prevent -p:OutputPath="" gobbling up the rest of the command line in dotnet msbuild. Not sure what's going on here, the normal msbuild quotes " ' &quot; are somehow stripped by dotnet msbuild, but ; still works well.
					break;

				// https://github.com/quamotion/dotnet-packaging
				case PackageType.Rpm:
					await RunAsync("dotnet", $"msbuild {config.ProjectFilePath} -target:CreateRpm -property:Version={config.VersionLong} -property:Configuration={config.PublishOptions.Configuration} -property:RuntimeIdentifier={config.PublishOptions.Runtime} -property:OutputPath={config.BuildArtifactsFolderPath};NOOP=NOOP ");
					break;

				// https://jrsoftware.org/isinfo.php
				case PackageType.Exe:
					await RunAsync("dotnet", $"publish {config.ProjectFilePath} --configuration {config.PublishOptions.Configuration} --runtime {config.PublishOptions.Runtime} --self-contained {config.PublishOptions.SelfContainedString}");
					await RunInnoCompiler(config);
					break;

				// https://en.wikipedia.org/wiki/Bundle_(macOS)
				case PackageType.App:
					await RunAsync("dotnet", $"publish {config.ProjectFilePath} --configuration {config.PublishOptions.Configuration} --runtime {config.PublishOptions.Runtime} --self-contained {config.PublishOptions.SelfContainedString}");
					WrapInMacOsAppHierarchy(config);
					break;

				// https://github.com/create-dmg/create-dmg
				case PackageType.Dmg:
					await RunAsync("dotnet", $"publish {config.ProjectFilePath} --configuration {config.PublishOptions.Configuration} --runtime {config.PublishOptions.Runtime} --self-contained {config.PublishOptions.SelfContainedString}");
					WrapInMacOsAppHierarchy(config);
					// TODO: create-dmg for Linux and macOS.
					// await RunAsync("create-dmg", $"--volname \"Howabout Installer\" \"{config.BuildArtifactsFolderPath}/howabout-v{typeof(Program).Assembly.GetName()?.Version?.ToString(2)}.dmg\" \"{config.BuildArtifactsFolderPath}/howabout.app\"");
					break;

				case PackageType.Docker:
					var baseimg = config.PublishOptions.Runtime switch
					{
						"linux-x64" => "mcr.microsoft.com/dotnet/aspnet:8.0.3-alpine3.18-amd64",
						"linux-musl-arm64" => "mcr.microsoft.com/dotnet/aspnet:8.0.3-alpine3.18-arm64v8",
						_ => throw new NotSupportedException($"Docker base image not defined for runtime {config.PublishOptions.Runtime}.")
					};

					var archtag = config.PublishOptions.Runtime switch
					{
						"linux-x64" => "amd",
						"linux-musl-arm64" => "arm",
						_ => throw new NotSupportedException($"Docker tag name not defined for runtime {config.PublishOptions.Runtime}.")
					};

					var tags = new List<string>()
					{
						$"howabout/howabout:{archtag}",
						$"howabout/howabout:{config.VersionShort}-{archtag}",
					};
					
					if (archtag == "amd")
					{
						// Docker builds for x32-x64/amd are most popular, run (emulated) anywhere 
						// and a good default fetch for `docker run howabout` when tag unspecified.
						tags.Add($"howabout/howabout:latest");
					}

					Bundle.EnsureDirectoryExists(config.BuildArtifactsFolderPath);
					var output = Path.Combine(config.BuildArtifactsFolderPath, config.FileName + ".tar");
					var context = config.SolutionFolderPath.ToLinuxForwardSlashes().WithQuotes(); 
					var dockerfile = Path.Combine(config.SolutionFolderPath, "Dockerfile").ToLinuxForwardSlashes().WithQuotes();

					await RunAsync("docker", $"build -t {string.Join(" -t ", tags)} -f {dockerfile} {context} --build-arg RUNTIME={config.PublishOptions.Runtime} --build-arg BASEIMG={baseimg}");
					await RunAsync("docker", $"save --output {output} {string.Join(" ", tags)}");
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
				.Replace(@"#define MyAppVersion ""0.0""", $@"#define MyAppVersion ""{config.VersionShort}""")
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
