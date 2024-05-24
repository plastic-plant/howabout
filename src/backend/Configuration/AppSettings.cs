using System;

namespace Howabout.Configuration
{
	public static class App
	{
		static AppSettings _settings = new AppSettings();

		public static AppSettings Settings
		{
			get => _settings;
			set => _settings = value ?? throw new ArgumentNullException();
		}
	}

	public class AppSettings
	{
		static IConfigurationRoot? _config = null;
		public IConfigurationRoot Configuration
		{
			get => _config ??= new ConfigurationBuilder()
						.SetBasePath(Program.ApplicationRootDirectory)
						.AddJsonFile("appsettings.json", optional: false)
						.Build();
			set => _config = value;
		}

		static string? _url = string.Empty;
		public string? Url
		{
			get => _url;
			set => _url = EnsureUrlEndsInSlash(value);
		}

		public AppSettings ReadFromAppSettings()
		{
			Url = Configuration.GetSection("Urls").Value;
			return this;
		}

		public AppSettings ReadFromEnvironmentVariables()
		{
			Url = Environment.GetEnvironmentVariable("ASPNETCORE_URLS");
			return this;
		}

		public AppSettings VerifyOrThrow()
		{
			if (!Uri.TryCreate(this.Url, UriKind.Absolute, out var _))
			{
				throw new ArgumentNullException(nameof(Url));
			}
			return this;
		}

		private string? EnsureUrlEndsInSlash(string? urls)
		{
			if (!string.IsNullOrWhiteSpace(urls))
			{
				string? url = urls.Split(';').FirstOrDefault();
				return url.EndsWith("/") ? url : $"{url}/";
			}
			return _url;
		}
	}
}
