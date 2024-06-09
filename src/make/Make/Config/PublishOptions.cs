using System;
using System.Linq;

namespace Make.Config
{
	public class PublishOptions
	{
		public string Name { get; set; }
		public string FileNameTemplate { get; set; } = "howabout-{0.0.0}.{runtime}";
        public string Configuration { get; set; } = "Release";
        public string Runtime { get; set; } = "win-x64";
		public PackageType Package { get; set; } = PackageType.None;
        public bool SelfContained { get; set; } = true;
		public string SelfContainedString => SelfContained.ToString().ToLower();
		public bool LocalGgufModelSupported => new string[] { "linux-x64", "osx-arm64", "osx-x64", "win-x64" }.Contains(Runtime);
        public bool IsRecommendedPlatformInstaller { get; set; }
    }
}
