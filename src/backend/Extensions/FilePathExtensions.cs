using System;
using System.IO;
using System.Reflection;

namespace Howabout.Extensions
{
	public static class FilePathExtension
	{
		public enum RelativePathMatchingStrategy
		{
			None,
			TryCurrentWorkingDirectory,
			TryExecutingAssemblyBaseDirectory,
			TryCurrentWorkingRoot,
			TryAll
		}

		/// <summary>
		/// Expands the paths to full qualified verified paths. Relative paths are expanded to absolute paths
		/// for the current working directory and optionally for the base directory of executing assembly.
		/// By default verifies the path to exist and throws and exception if fails to match an existing file.
		/// URLs are ignores and returned as-is.
		/// </summary>
		public static IEnumerable<string> ToFullPaths(this IEnumerable<string> paths, RelativePathMatchingStrategy options = RelativePathMatchingStrategy.TryAll, bool failIfNotFound = true)
		{
			var expanded = new List<string>();
			foreach (var path in paths)
			{
				var tryPath = path;
				
				//// Command line options are ignored and returned as-is.
				//if (path.StartsWith("-"))
				//{
				//	expanded.Add(path);
				//	continue;
				//}

				// URLs are ignored and returned as-is.
				if (Uri.TryCreate(path, UriKind.Absolute, out _))
				{
					expanded.Add(path);
					continue;
				}

				// Relative paths are expanded to absolute paths.
				if (!Path.IsPathRooted(path)) switch (options)
				{
					case RelativePathMatchingStrategy.TryCurrentWorkingDirectory:
						tryPath = Path.Combine(Directory.GetCurrentDirectory(), path);
						break;

					case RelativePathMatchingStrategy.TryExecutingAssemblyBaseDirectory:
						tryPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, path);
						break;

					case RelativePathMatchingStrategy.TryCurrentWorkingRoot:
						tryPath = Path.Combine(Path.GetPathRoot(Directory.GetCurrentDirectory())!, path);
						break;

					case RelativePathMatchingStrategy.TryAll:
					default:
						tryPath = Path.Combine(Directory.GetCurrentDirectory(), path);
						if (!Path.Exists(tryPath))
						{
							tryPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, path);
						}
						if (!Path.Exists(tryPath))
						{
							tryPath = Path.Combine(Path.GetPathRoot(Directory.GetCurrentDirectory())!, path);
						}
						break;
				}

				// Absolute paths are verified to exist and optionally throw if not.
				if (Path.Exists(tryPath))
				{
					// Do we have a an absolute path that exists, return that.
					expanded.Add(tryPath);
				}
				else if (!failIfNotFound)
				{
					// Return the original path if we could not resolve the relative path.
					expanded.Add(path);
				}
				else
				{
					// Or throw an exception if fail-if-not-found is preferred.
					throw new ArgumentException("Path not found.", path);
				}
			}
			
			return expanded;
		}

		public static IEnumerable<string> IncludeDirectoryFiles(this IEnumerable<string> paths, SearchOption option = SearchOption.AllDirectories, bool excludeDirectoryPath = true)
		{
			var expanded = new List<string>();
			foreach (var path in paths)
			{
				if (Directory.Exists(path))
				{
					// Add all files we find in a directory path. By default do not include the directory path itself.
					expanded.AddRange(Directory.GetFiles(path, "*.*", option));
					if (!excludeDirectoryPath)
					{
						expanded.Add(path);
					}
				}
				else
				{
					// If the path is not a directory or not a path at all, ignore and just add it as-is.
					expanded.Add(path);
				}
			}
			return expanded;
		}
	}
}
