namespace Make.Config
{
	public enum PackageType
	{
		None,   // No packaging, default folder structure: https://learn.microsoft.com/en-us/aspnet/core/host-and-deploy/directory-structure
		Zip,	// Zip archive: https://en.wikipedia.org/wiki/Zip_(file_format)
		TarGz,	// Tarball with Gzip archive: https://en.wikipedia.org/wiki/Tar_(computing)
		Deb,	// Debian package: https://en.wikipedia.org/wiki/Deb_(file_format)
		Rpm,    // Red Hat package: https://en.wikipedia.org/wiki/RPM_Package_Manager
		Exe,    // Windows .exe installer: https://en.wikipedia.org/wiki/Windows_Installer
		App,    // macOS .app application hierarchy: https://en.wikipedia.org/wiki/Bundle_(macOS)
		Dmg,    // macOS disk image: https://en.wikipedia.org/wiki/Apple_Disk_Image
		Docker, // Docker image: https://en.wikipedia.org/wiki/Docker_(software)
	}
}
