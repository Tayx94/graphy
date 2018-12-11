# Graphy - Ultimate FPS Counter - Stats Monitor & Debugger (Unity)

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
- [Twitter](https://twitter.com/tayx94?)
- [Discord](https://discord.gg/2KgNEHK?)

## Development of Graphy

Maintainer and main developer: **Martín Pane**

### Contributing

Let's make Graphy the go-to for stats monitoring in Unity!

I would really appreciate any contributions! Below you can find a roadmap for future planned features and optimisations that you might be able to help out with. If you want to make a big pull request, please do it on the "dev" branch.

Create a GitHub issue if you want to start a discussion or request a feature, and please label appropriately.

You can also join the [Discord](https://discord.gg/2KgNEHK?) for active discussions with other members of the community.

### Roadmap

**Planned features (DEFINITELY YES):**

  - Console Module: (developed in the [console-module](https://github.com/Tayx94/graphy/tree/console-module) branch)
    - Show Debug.Log messages from Unity in this module.
    - Log Graphy messages to this module.
    - Integrate Graphy commands into it.
    - Allow custom commands from user.
  - Add GfxDriver stats to the RAM module.
  - Prewarm framerates: X seconds where min/max fps are not registered to avoid loading spikes registering.
  - Scale Canvas (GetComponent<Canvas>().scaleFactor *= multiplier;) -> If it changes, set again.
  - Make UI layout adapt on the fly to compact it when not showing graphs.
    
**Other features (DEFINITELY MAYBE):** 

  - Network Stats Module
  - Audio Module:
  	- Average of the whole sound
	- Indicator for LUFS
  - Allow storing FPS for a predetermined time to allow benchmarks.
  - Dump all Graphy Data as a string to:
  	- File
	- Send to server
	- Send mail
  - Add a preprocessor key #GRAPHY to avoid adding the asset in builds
  
## License

Graphy is released under the [MIT license](https://github.com/Tayx94/graphy/blob/master/LICENSE). Although I don't require attribution, I would love to know if you decide to use it in a project! Let me know on [Twitter](https://twitter.com/tayx94?) or by [email](martintayx@gmail.com).
