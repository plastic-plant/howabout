### How to install on macOS

Howabout for Linux is published on the [releases page](https://github.com/plastic-plant/howabout/releases). Various packages are available for your distribution and hardware architecture, but building and running the app yourself should improve the deployment experience.

- Release [howabout-1.0.0.macos-arm64.tar.gz](https://github.com/plastic-plant/howabout/releases/download/v1.0.0/howabout-1.0.0.macos-arm64.tar.gz) for macOS ARM64 on newer Apple Silicon based machines with an M1, M2, M3, etc. chip (macOS 11 Big Sur and later).
- Release [howabout-1.0.0.macos-x64.tar.gz](https://github.com/plastic-plant/howabout/releases/download/v1.0.0/howabout-1.0.0.macos-x64.tar.gz) for macOS 64-bit on older Intel based machines (macOS 10.12 Sierra and later).

- Make scripts `macos-arm64-app` and `macos-x64-app` for Applications folder.

#### No installer package for macOS

The age of drop-and-run is over. Since Big Sur, publishing for macOS is a tiny more complex. Part of the Make scripts, I've put in a simple `howabout.app` bundle structure with manifest that you can build with `make --buildconfig macos-arm64-app`). This helps copying the files to the user's /Applications folder and have the operating system set the right permissions.


#### Code signing and building from source

So, we need to [codesign the app](https://github.com/spyder-ide/spyder/wiki/Dev:-Codesigning-the-macOS-Standalone-Application) to where software originated from. While the packages in release links above are a start, you're on your own here as you need to sign the app bundle first: I haven't put the work in for a pipeline that builds and codesigns a [DMG](https://en.wikipedia.org/wiki/Apple_Disk_Image).

```bash
codesign -s - ./howabout.app/
```

With the command above you can sign the app bundle or files extracted from a .tar.gz release, but you may still need to allow the app in System Preferences > Security & Privacy. If you run `howabout help` and Terminal shows `zsh: killed`, that's most likely a code signing issue. Should be easier to get through by [compiling and running](23_build_with_dotnet_cli.md) the app on macOS from source with [JetBrains Rider](22_build_with_rider.md) or [Visual Studio Code](21_build_with_visual_studio_code.md). Or deploy with Docker.
