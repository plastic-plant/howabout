using Make.Config;
using System;
using System.IO;
using System.Linq;
using static Bullseye.Targets;
using static SimpleExec.Command;

string repository = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, @"..\..\..\..\howabout\"));
string solution = Path.GetFullPath(Path.Combine(repository, @"src\Howabout.sln"));
string project = Path.GetFullPath(Path.Combine(repository, @"src\backend\Howabout.csproj"));
string publish = Path.GetFullPath(Path.Combine(repository, @"releases\"));
var configs = PublishOptionsHelper.TakePublishOptionsFromArguments(ref args).ToBuildConfigs(repository, solution, project, publish);

foreach (var config in configs)
{
	Target($"publish-{config.PublishOptions.Name}", async () =>
	{
		await Solution.CleanAsync(config);
		await Solution.BuildAsync(config);
		Bundle.Create(config);
	});
}

Target("build", () => RunAsync("dotnet", "build --configuration Release --nologo --verbosity quiet"));
Target("test", DependsOn("build"), () => RunAsync("dotnet", "test --configuration Release --no-build --nologo --verbosity quiet"));
Target("publish", DependsOn(configs.Select(config => $"publish-{config.PublishOptions.Name}").ToArray()));
Target("default", DependsOn("publish"));

await RunTargetsAndExitAsync(args, ex => ex is SimpleExec.ExitCodeException);