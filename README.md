# Table of Contents
1. [Introduction](#graphy---ultimate-fps-counter---stats-monitor--debugger-unity)
2. [Installation](#installation)
   - [Unity Asset Store](#unity-asset-store)
   - [OpenUPM](#openupm)
   - [Unity Package Manager](#unity-package-manager)
   - [Choosing an Installation Method](#choosing-an-installation-method)
3. [Quick Start Guide](#quick-start-guide)
4. [Practical Use Cases](#practical-use-cases)
5. [Development of Graphy](#development-of-graphy)
6. [Contributing](#contributing)
7. [Roadmap](#roadmap)
8. [License](#license)

# Graphy - Ultimate FPS Counter - Stats Monitor & Debugger (Unity)

[![openupm](https://img.shields.io/npm/v/com.tayx.graphy?label=openupm&registry_uri=https://package.openupm.com)](https://openupm.com/packages/com.tayx.graphy/)
[![License: MIT](https://img.shields.io/badge/License-MIT-brightgreen.svg)](https://github.com/Tayx94/graphy/blob/master/LICENSE)
[![Unity 2019.4+](https://img.shields.io/badge/unity-2019.4%2B-blue.svg)](https://unity3d.com/get-unity/download)

[![Open Issues](https://img.shields.io/github/issues-raw/tayx94/graphy)](https://github.com/Tayx94/graphy/issues)
[![Downloads](https://img.shields.io/github/downloads/tayx94/graphy/total)](https://github.com/Tayx94/graphy/releases)
[![Contributors](https://img.shields.io/github/contributors/tayx94/graphy)](https://github.com/Tayx94/graphy/graphs/contributors)
[![Stars](https://img.shields.io/github/stars/Tayx94/graphy)](https://github.com/Tayx94/graphy)
[![Forks](https://img.shields.io/github/forks/Tayx94/graphy)](https://github.com/Tayx94/graphy)

[![Chat Discord](https://img.shields.io/discord/406037880314789889)](https://discord.gg/2KgNEHK?)

[![Twitter](https://img.shields.io/twitter/follow/martintayx?label=Follow&style=social)](http://twitter.com/intent/user?screen_name=martinTayx)

**Links:** [Discord](https://discord.gg/2KgNEHK?) | [Mail](mailto:martintayx@gmail.com) | [Twitter](http://twitter.com/intent/user?screen_name=martinTayx) | [Asset Store](https://assetstore.unity.com/packages/tools/gui/graphy-ultimate-stats-monitor-debugger-105778) | [Forum post](https://forum.unity.com/threads/graphy-ultimate-stats-monitor-debugger-released.517387/) | [Donations](https://www.paypal.me/MartinPaneUK)

**WINNER** of the **BEST DEVELOPMENT ASSET** in the **Unity Awards 2018**.

![Graphy Image](https://image.ibb.co/dbcDu8/2018_07_15_15_10_05.gif)

Graphy is the ultimate, easy to use, feature packed FPS Counter, stats monitor and debugger for your Unity project.

**Main Features:**
- Graph & Text:
  - FPS
  - Memory
  - Audio
- Advanced device information 
- Debugging tools 

With this tool you will be able to visualize and catch when your game has some unexpected hiccup or stutter, and act accordingly! 

The debugger allows you to set one or more conditions, that if met will have the consequences you desire, such as taking a screenshot, pausing the editor, printing a message to the console and more! Even call a method from your own code if you want! 

**Additional features:**
- Customizable look and feel 
- Multiple layouts 
- Custom Inspector 
- Hotkeys 
- Easy to use API (accessible from code) 
- Works on multiple platforms 
- Background Mode 
- Works from Unity 5.4 and up! 
- Well documented C# and Shader code included 

**Links:**
- [Asset store](https://assetstore.unity.com/packages/tools/gui/graphy-ultimate-stats-monitor-debugger-105778)
- [Forum post](https://forum.unity.com/threads/graphy-ultimate-stats-monitor-debugger-released.517387/)
- [Video Teaser](https://youtu.be/2X3vXxLANk0)

**Contact:**
- [Mail](martintayx@gmail.com)
- [Twitter](https://twitter.com/martinTayx)
- [Discord](https://discord.gg/2KgNEHK?)
# Installation

### Unity Asset Store

* [Graphy on the Asset Store](https://assetstore.unity.com/packages/tools/gui/graphy-ultimate-stats-monitor-debugger-105778)
* For Unity 5.4+, use the [legacy version](https://github.com/Tayx94/graphy/releases/tag/v1.6.0-Unity5.4)

### OpenUPM
Use the [openupm-cli](https://github.com/openupm/openupm-cli) for command-line installation, suitable for automation scripts or if you prefer terminal commands.
```
openupm add com.tayx.graphy
```
### Unity Package Manager
1. **Scoped Registry Method:** You can add this package in Unity Package Manager by adding it to the Scoped Registries at Edit > Project Settings > Package Manager > Scoped Registries
	* Add `package.openupm.com` as a Scoped Registry in Project Settings
	* Set URL to `https://package.openupm.com`
	* Include `com.tayx.graphy` under Scopes
	* Graphy will then appear in Package Manager for installation ![image](https://github.com/ROBYER1/graphy/assets/10745594/dbf18c6e-a170-4128-b6c6-f12d9cb75ea6)
![image](https://github.com/ROBYER1/graphy/assets/10745594/0a6328d1-4a00-47d4-97d0-964535c37400) 
2. **Git URL:** If you're comfortable with direct Git integration, this method allows you to track the latest commits or specific branches.
	* Add the Git URL to your `manifest.json` dependencies.
	```
	{
	  "dependencies": {
	    ...
	    "com.tayx.graphy": "https://github.com/Tayx94/graphy.git",
	    ...
	  }
	}
	```
## Choosing an Installation Method
-   Use the **Asset Store** for the simplest installation process, directly through Unity's interface.
-   The **openupm-cli** method is best for those who use openupm and prefer command-line tools.
-   **Scoped Registries** offer a balance between ease of use and control, directly integrating with Unity's built-in package management system.
-   The **Git URL** method provides the most control and is best suited for those tracking development closely or needing specific revisions.

## Quick Start Guide

1. **Open Your Unity Project**: Start Unity and open your project.
2. **Installation**: Choose your preferred installation method:
   - Via the [Unity Asset Store](https://assetstore.unity.com/packages/tools/gui/graphy-ultimate-stats-monitor-debugger-105778)
   - Using the openupm-cli: Run `openupm add com.tayx.graphy` in your terminal.
   - For other methods, refer to the [Installation section](#installation).
3. **Add Graphy to Your Scene**: In Unity, navigate to `Graphy - Ultimate Stats Monitor > Prefab > Place [Graphy] in your scene`.
4. **Initial Configuration of Graphy**: To get Graphy up and running quickly:
   - **Graphy Mode**: Set to 'FULL' to enable all monitoring features.
   - **Enable On Startup**: Check this to have Graphy start monitoring as soon as your game runs.
   - **Keep Alive**: Ensure this is checked to prevent Graphy from closing unexpectedly.
   - **Background**: Choose a background to enhance visibility according to your scene's color scheme.
   - **Hotkeys**: Enable hotkeys and set them to your preferences for quick access to Graphy's features.
   - **Positioning**: Select 'TOP_RIGHT' for Graphy's location on the screen, or choose another position that doesn't obstruct gameplay.
   - **Offset**: Adjust the X and Y offsets if necessary to fine-tune Graphy's position.
5. **Run Your Project**: Play your scene in Unity to see Graphy in action, monitoring your project's performance.

For detailed usage and advanced settings, please refer to the [full documentation](https://github.com/Tayx94/graphy/blob/master/Readme!%20-%20Graphy%20-%20Documentation.pdf).


## Practical Use Cases

Graphy is an indispensable tool in the Unity developer's arsenal, allowing for detailed performance analysis and debugging. Here are several practical ways to utilize Graphy:

#### Performance Optimization
Monitor and analyze FPS during gameplay to identify and address performance drops, ensuring a smooth player experience.

#### Memory Monitoring
Track memory allocation and deallocation in real-time to pinpoint potential leaks and optimize memory usage.

#### Audio Monitoring
Use the audio module to ensure balanced sound levels and to debug any audio-related performance issues.

#### Automated Debugging
Set up conditional debug actions to automatically capture screenshots, log messages, or execute custom scripts when performance thresholds are met.

Graphy's versatility makes it a valuable tool for a range of tasks from development through to QA and post-release support.

## Development of Graphy

Maintainer and main developer: **Martín Pane** [![Twitter](https://img.shields.io/twitter/follow/martintayx?label=Follow&style=social)](http://twitter.com/intent/user?screen_name=martinTayx)

Graphy is **FREE** to use, but if it helped you and you want to contribute to its development, feel free to leave a donation! 

- [Donation Link](https://www.paypal.me/MartinPaneUK)

### Contributing

Let's make Graphy the go-to for stats monitoring in Unity!

I would really appreciate any contributions! Below you can find a roadmap for future planned features and optimisations that you might be able to help out with. If you want to make a big pull request, please do it on the "dev" branch.

Create a GitHub issue if you want to start a discussion or request a feature, and please label appropriately.

You can also join the [Discord](https://discord.gg/2KgNEHK?) for active discussions with other members of the community.

## Roadmap

**Planned features (No ETA):**

  - Add GfxDriver stats to the RAM module.
  - Scale Canvas (GetComponent<Canvas>().scaleFactor *= multiplier;) -> If it changes, set again.
  - Make a template for a graph + text module so people can create their own easily.
  - Allow storing FPS for a predetermined time to allow benchmarks.
  - Dump all Graphy Data as a string to:
  	- File.
	- Send to server.
	- Send mail.
  - Add a preprocessor key #GRAPHY to avoid adding the asset in builds.
  
## License

Graphy is released under the [MIT license](https://github.com/Tayx94/graphy/blob/master/LICENSE). Although I don't require attribution, I would love to know if you decide to use it in a project! Let me know on [Twitter](https://twitter.com/martinTayx) or by [email](martintayx@gmail.com).
