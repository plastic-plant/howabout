﻿namespace Make.Config
{
	public class BuildConfig
	{
        public PublishOptions PublishOptions { get; set; }
        public string SolutionFilePath { get; set; }
		public string SolutionFolderPath { get; set; }
		public string BuildArtifactsFolderPath { get; set; }
		public string PackagePublishFolderPath { get; set; }
    }
}