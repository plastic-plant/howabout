using SharpCompress.Common;
using SharpCompress.Writers;
using System.IO;
using System.Runtime.InteropServices;

namespace Make.Config
{
	public class Bundle
	{
        public BuildConfig Config { get; set; }

		public static Bundle Create(BuildConfig config)
		{
			EnsureDirectoryEmpty(config.PackagePublishFolderPath);

			switch (config.PublishOptions.Package)
			{
				case PackageType.Zip:					
					Compress(ArchiveType.Zip, CompressionType.Deflate, config.BuildArtifactsFolderPath, Path.Combine(config.PackagePublishFolderPath, $"howabout-{config.PublishOptions.Name}.zip"));
					break;

				case PackageType.TarGz:
					Compress(ArchiveType.Tar, CompressionType.GZip, config.BuildArtifactsFolderPath, Path.Combine(config.PackagePublishFolderPath, $"howabout-{config.PublishOptions.Name}.tar.gz"));
					break;

				case PackageType.None:
				default:
					CopyDirectory(config.BuildArtifactsFolderPath, config.PackagePublishFolderPath);
					break;
			}

			return new Bundle()
			{
				Config = config,

			};
		}
		public static void EnsureDirectoryEmpty(string folderPath)
		{
			try
			{
				Directory.Delete(folderPath, true);
			}
			catch (DirectoryNotFoundException)
			{
				
			}

			EnsureDirectoryExists(folderPath);
		}

		public static void EnsureDirectoryExists(string folderPath)
		{
			if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
			{
				Directory.CreateDirectory(folderPath);
			}
			else
			{
				var chmod775 = // -rwxrwxr-x
					UnixFileMode.UserRead | UnixFileMode.UserWrite | UnixFileMode.UserExecute |
					UnixFileMode.GroupRead | UnixFileMode.GroupWrite | UnixFileMode.GroupExecute |
					UnixFileMode.OtherRead | UnixFileMode.OtherWrite;
				Directory.CreateDirectory(folderPath, chmod775);
			}
		}

		public static void CopyDirectory(string sourceFolderPath, string targetFolderPath)
		{
			if (!Directory.Exists(sourceFolderPath))
			{
				throw new DirectoryNotFoundException($"Directory {sourceFolderPath} does not exist.");
			}

			EnsureDirectoryExists(targetFolderPath);
			var sourceDirectory = new DirectoryInfo(sourceFolderPath);

			foreach (FileInfo file in sourceDirectory.GetFiles())
			{
				string filePath = Path.Combine(targetFolderPath, file.Name);
				file.CopyTo(filePath, true);
			}

			foreach (DirectoryInfo sourceSubDir in sourceDirectory.GetDirectories())
			{
				string targetSubDir = Path.Combine(targetFolderPath, sourceSubDir.Name);
				CopyDirectory(sourceSubDir.FullName, targetSubDir);
			}
		}

		public static void Compress(ArchiveType archiveType, CompressionType compressionType, string sourceFolderPath, string targetFilePath)
		{
			using (var stream = File.OpenWrite(targetFilePath))
			using (var writer = WriterFactory.Open(stream, archiveType, compressionType))
			{
				writer.WriteAll(sourceFolderPath, "*", SearchOption.AllDirectories);
			}
		}
	}
}