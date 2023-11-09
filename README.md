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

**Links:** [Discord](https://discord.gg/2KgNEHK?) | [Mail](mailto:martintayx@gmail.com) | [Twitter](http://twitter.com/intent/user?screen_name=martinTayx) | [Asset store](https://assetstore.unity.com/packages/tools/gui/graphy-ultimate-stats-monitor-debugger-105778) | [Forum post](https://forum.unity.com/threads/graphy-ultimate-stats-monitor-debugger-released.517387/) | [Donations](https://www.paypal.me/MartinPaneUK)

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

## Installation
1. The package is available on the [openupm registry](https://openupm.com). You can install it via [openupm-cli](https://github.com/openupm/openupm-cli).
```
openupm add com.tayx.graphy
```
2. You can also install via git url by adding this entry in your **manifest.json**
```
{
  "dependencies": {
    ...
    "com.tayx.graphy": "https://github.com/Tayx94/graphy.git",
    ...
  }
}
```
3. You can also download it from the [Asset Store](https://assetstore.unity.com/packages/tools/gui/graphy-ultimate-stats-monitor-debugger-105778)

4. Click here for old version that supports Unity 5.4+: 
[![Unity 5.4+](https://img.shields.io/badge/unity-5.4%2B-blue.svg)](https://github.com/Tayx94/graphy/releases/tag/v1.6.0-Unity5.4)

## Development of Graphy

Maintainer and main developer: **Martín Pane** [![Twitter](https://img.shields.io/twitter/follow/martintayx?label=Follow&style=social)](http://twitter.com/intent/user?screen_name=martinTayx)

Graphy is **FREE** to use, but if it helped you and you want to contribute to its development, feel free to leave a donation! 

- [Donation Link](https://www.paypal.me/MartinPaneUK)

### Contributing

Let's make Graphy the go-to for stats monitoring in Unity!

I would really appreciate any contributions! Below you can find a roadmap for future planned features and optimisations that you might be able to help out with. If you want to make a big pull request, please do it on the "dev" branch.

Create a GitHub issue if you want to start a discussion or request a feature, and please label appropriately.

You can also join the [Discord](https://discord.gg/2KgNEHK?) for active discussions with other members of the community.

### Roadmap

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
