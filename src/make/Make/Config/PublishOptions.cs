using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Make.Config
{
	public class PublishOptions
	{
		public string Name { get; set; }
		public string Configuration { get; set; } = "Release";
        public string Runtime { get; set; } = "win-x64";
		public PackageType Package { get; set; } = PackageType.None;

        public bool SelfContained { get; set; } = true;
		public string SelfContainedString => SelfContained.ToString().ToLower();

	}

	



}
