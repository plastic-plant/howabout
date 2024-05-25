### How to install on Linux

Howabout for Linux is published on the [releases page](https://github.com/plastic-plant/howabout/releases). Various packages are available for your distribution and hardware architecture.

- Download [howabout-1.0.0.linux-x64.deb](https://github.com/plastic-plant/howabout/releases/download/v1.0.0/howabout-1.0.0.linux-x64.deb) as .DEB package.
- Download [howabout-1.0.0.linux-x64.rpm](https://github.com/plastic-plant/howabout/releases/download/v1.0.0/howabout-1.0.0.linux-x64.rpm) as .RPM package.
- Download [howabout-1.0.0.linux-x64.tar.gz](https://github.com/plastic-plant/howabout/releases/download/v1.0.0/howabout-1.0.0.linux-x64.tar.gz) as tarball.


#### Install with Debian or APT package manager

On Debian, Ubuntu, Mint, Elementary, Kali, Pop!_OS and other Debian-based distributions, you can use the APT package tool to install Howabout. This typically extracts the the files in `/usr/share/howabout/` and creates a symbolic link `/usr/local/bin/howabout` to the executable. You can then run the executable from the command line.

```bash
wget https://github.com/plastic-plant/howabout/releases/download/v1.0.0/howabout-1.0.0.linux-x64.deb
sudo apt install ./howabout-1.0.0.linux-x64.deb

howabout help
sudo apt remove howabout
```

Installing the same package with Debian package manager typically extracts the the files in `/usr/local/share/howabout/` and creates a symbolic link `/usr/local/bin/howabout`.

```bash
wget https://github.com/plastic-plant/howabout/releases/download/v1.0.0/howabout-1.0.0.linux-x64.deb
sudo dpkg --install howabout-1.0.0.linux-x64.deb

howabout help
sudo dpkg --remove howabout
```


#### Install with RPM, DNF, YUM or ZYPPER package managers

On Fedora, CentOS, OracleLinux, SUSE and other RPM-based distributions, you can use the RPM package manager to install Howabout. On systems that use DNF or YUM, you can use those package managers to install the .RPM file. (DNF is the default package manager for Fedora, while YUM is used on CentOS and older Fedora versions.) This example shows how to install and remove with each of the three package managers, which typically extracts the the files in `/usr/share/howabout/` and creates a symbolic link `/usr/local/bin/howabout` to the executable.


```bash
wget https://github.com/plastic-plant/howabout/releases/download/v1.0.0/howabout-1.0.0.linux-x64.rpm

sudo rpm --install howabout-1.0.0.linux-x64.rpm
sudo dnf install howabout-1.0.0.linux-x64.rpm
sudo yum install howabout-1.0.0.linux-x64.rpm
sudo zypper install howabout-1.0.0.linux-x64.rpm

howabout help

sudo rpm --erase howabout
sudo dnf remove howabout
sudo yum remove howabout
sudo zypper remove howabout
``` 


#### Install as tarball

Downloading and extracting a tarball, allows us some more control over the installation. You can download and extract the files to a directory of your choice. Here's an example that downloads, extracts the program files and creates a symbolic link to the howabout executable. Then runs it once as a test, shows the version. Afterwards removes the symbolic link, the extracted files and the downloaded tarball.

```bash
cd ~

wget https://github.com/plastic-plant/howabout/releases/download/v1.0.0/howabout-1.0.0.linux-x64.tar.gz
sudo tar -xvf howabout-1.0.0.linux-arm.tar.gz -C /usr/local/howabout
sudo ln -s /usr/share/howabout/howabout /usr/local/bin/howabout

cd /usr/local/howabout
howabout help
howabout version

sudo rm /usr/local/bin/howabout
sudo rm -rf /usr/local/howabout
rm ~/howabout-1.0.0.linux-x64.tar.gz
```


#### Linux ARM and MUSL lightweight

Some versions are available for lightweight Linux distributions. Please see the setup pages for [Install on Raspberry Pi](doc/how_to/04_install_on_raspberry.md)], [Install on Docker](doc/how_to/05_install_on_docker.md)] and [Install on Kubernetes](doc/how_to/06_install_on_kubernetes.md)] for details.

- Download [howabout-1.0.0.linux-arm.tar.gz](https://github.com/plastic-plant/howabout/releases/download/v1.0.0/howabout-1.0.0.linux-arm.tar.gz) for Linux distributions on ARM like Raspbian on Raspberry Pi Model 2+.
- Download [howabout-1.0.0.linux-arm64-musl.tar.gz](https://github.com/plastic-plant/howabout/releases/download/v1.0.0/howabout-1.0.0.linux-arm64-musl.tar.gz) as tarball for Linux distributions running on 64-bit ARM like Raspberry Pi OS and Ubuntu Server on Raspberry Pi Model 3+
- Download [howabout-1.0.0.linux-arm64.tar.gz](https://github.com/plastic-plant/howabout/releases/download/v1.0.0/howabout-1.0.0.linux-arm64.tar.gz) as tarball for lightweight Linux 64-bit ARMv8 to build Docker images.
- Download [howabout-1.0.0.linux-x64-musl.tar.gz](https://github.com/plastic-plant/howabout/releases/download/v1.0.0/howabout-1.0.0.linux-x64-musl.tar.gz) as tarball for lightweight Linux 64-bit distributions using musl like Alpine.
