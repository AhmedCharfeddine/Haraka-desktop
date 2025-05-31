<p align="center">
  <img src="Assets/haraka-logo-circle.ico" alt="Haraka Logo" width="120" />
  <h1 align="center">Haraka Desktop</h1>
</p>

<p align="center">
  🌀 A lightweight Arabic transliteration tool for Windows with real-time system-wide suggestions.
</p>

---

## ✨ Features

- 🌐 Real-time Arabic transliteration (like Yamli.com, but offline!)
- ⚙️ Customizable settings via GUI
- 🖥️ Runs in the system tray
- 🛠 Powered by the [Haraka CLI](https://github.com/AhmedCharfeddine/haraka)
- 📦 Packaged with Inno Setup for easy installation

## 🚀 Download

👉 [Download the latest installer from Releases](https://github.com/AhmedCharfeddine/haraka-desktop/releases)

The installer automatically includes the required [.NET 8 Desktop Runtime](https://dotnet.microsoft.com/en-us/download/dotnet/8.0).

## 📦 Requirements

- Windows 10 or 11 (x64)
- No manual prerequisites — the installer handles it for you

## 🧪 Run from Source

1. Make sure the [Haraka CLI](https://github.com/AhmedCharfeddine/haraka) binary is built and available as `bin/haraka.exe` (see the README for how to build, or download the binary directly from the releases page)
2. Clone this repository:
   ```bash
   git clone https://github.com/AhmedCharfeddine/haraka-desktop
   cd haraka-desktop
   ```
3. Run the app:
   ```bash
   dotnet run --project Haraka-desktop
   ```
4. Optional. Publish the app:
   ```bash
   dotnet publish Haraka-desktop.csproj -c Release -f net8.0-windows -r win-x64 -o publish
   ```

## 🙌 Contributing
Feel free to open an issue or PR! Ideas and improvements are always welcome.

## 📜 License
This project is licensed under the MIT License. See the [LICENSE](/LICENSE.md) file for details.

---

<p align="center"><i>Made with ❤️ for Arabic speakers</i></p>
