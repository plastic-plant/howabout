﻿namespace Make.Config
{
	public class BuildConfig
	{
		public PublishOptions PublishOptions { get; set; }
		public string RepositoryFolderpath { get; set; }
		public string SolutionFilePath { get; set; }
		public string SolutionFolderPath { get; set; }
		public string ProjectFilePath { get; set; }
		public string ProjectFolderPath { get; set; }
		public string BuildArtifactsFolderPath { get; set; }
		public string PackagePublishFolderPath { get; set; }
		public string VersionShort { get; set; }
		public string VersionLong => $"{VersionShort}.0";
		public string FileName => PublishOptions
			.FileNameTemplate
			.Replace("{0.0.0}", VersionLong)
			.Replace("{name}", PublishOptions.Name)
			.Replace("{runtime}", PublishOptions.Runtime);
	}
}
