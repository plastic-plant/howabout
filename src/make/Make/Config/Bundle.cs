using SharpCompress.Common;
using SharpCompress.Writers;
using System;
using System.IO;
using System.Linq;
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
					Compress(ArchiveType.Zip, CompressionType.Deflate, Path.Combine(config.BuildArtifactsFolderPath, "publish"), Path.Combine(config.PackagePublishFolderPath, config.FileName + ".zip"));
					break;

				case PackageType.TarGz:
					Compress(ArchiveType.Tar, CompressionType.GZip, Path.Combine(config.BuildArtifactsFolderPath, "publish"), Path.Combine(config.PackagePublishFolderPath, config.FileName + ".tar.gz"));
					break;

				case PackageType.Deb:
				case PackageType.Rpm:
					var packageType = Enum.GetName(typeof(PackageType), config.PublishOptions.Package).ToLower(); // deb rpm
					CopyFiles(config.BuildArtifactsFolderPath, $"*.{packageType}", config.PackagePublishFolderPath);
					break;

				case PackageType.Exe:
					CopyFile(Path.Combine(config.BuildArtifactsFolderPath, "setup", "howabout.exe"), config.PackagePublishFolderPath, config.FileName + ".exe");
					break;

				case PackageType.App:
					CopyDirectory(Path.Combine(config.BuildArtifactsFolderPath, "setup"), config.PackagePublishFolderPath);
					break;

				case PackageType.Dmg:
					// TODO: Apple Disk Image is possible with create-dmg in Linux and macOS. For now just zipping it; simply copy contents to /Applications/.
					Compress(ArchiveType.Zip, CompressionType.Deflate, Path.Combine(config.BuildArtifactsFolderPath, "publish"), Path.Combine(config.PackagePublishFolderPath, config.FileName + ".zip"));
					break;

				case PackageType.Docker:
					var filePathIn = Path.Combine(config.BuildArtifactsFolderPath, config.FileName + ".tar");
					var filePathOut = Path.Combine(config.PackagePublishFolderPath, config.FileName + ".tar.gz");
					Compress(ArchiveType.GZip, CompressionType.GZip, filePathIn, filePathOut);
					break;

				case PackageType.None:
				default:
					CopyDirectory(Path.Combine(config.BuildArtifactsFolderPath, "publish"), config.PackagePublishFolderPath);
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

		public static void CopyFile(string sourceFileName, string targetFolderPath, string targetFileName)
		{
			if (!File.Exists(sourceFileName))
			{
				throw new DirectoryNotFoundException($"File {sourceFileName} does not exist.");
			}

			EnsureDirectoryExists(targetFolderPath);
			File.Copy(sourceFileName, Path.Combine(targetFolderPath, targetFileName), true);
		}

		public static void CopyFiles(string sourceFolderPath, string matchFileName, string targetFolderPath)
		{
			if (!Directory.Exists(sourceFolderPath))
			{
				throw new DirectoryNotFoundException($"Directory {sourceFolderPath} does not exist.");
			}

			EnsureDirectoryExists(targetFolderPath);
			Directory.GetFiles(sourceFolderPath, matchFileName).ToList().ForEach(file =>
			{
				string targetFileName = Path.GetFileName(file).ToLower();
				File.Copy(file, Path.Combine(targetFolderPath, targetFileName), true);
			});
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
			var options = new WriterOptions(compressionType);
			options.LeaveStreamOpen = false;
			using (var stream = File.OpenWrite(targetFilePath))
			{
				using (var writer = WriterFactory.Open(stream, archiveType, options))
				{
					if (File.Exists(sourceFolderPath))
					{
						writer.Write(Path.GetFileName(sourceFolderPath), sourceFolderPath);
					}
					else if (Directory.Exists(sourceFolderPath))
					{
						writer.WriteAll(sourceFolderPath, "*", SearchOption.AllDirectories);
					}
				}
			}
		}
	}
}