# Graphy - Ultimate Stats Monitor & Debugger (Unity)

![Graphy Image](https://image.ibb.co/cR3vo7/Graphy_Runtime_4_3_GIF.gif)

Graphy is the ultimate, easy to use, feature packed stats monitor and debugger for your Unity project.

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

**Contact:**
- [Mail](martintayx@gmail.com)
- [Twitter](https://twitter.com/tayx94?)
- [Discord](https://discord.gg/2KgNEHK?)

## Development of Graphy

### Contributing

Let's make Graphy the go-to for stats monitoring in Unity!

I would really appreciate any contributions! Below you can find a roadmap for future planned features and optimisations that you might be able to help out with.

Create a GitHub issue if you want to start a discussion or request a feature, and please label appropriately.

You can also join the [Discord](https://discord.gg/2KgNEHK?) for active discussions with other members of the community.

### Roadmap

**Planned features (DEFINITELY YES):**

  - Add toggle from code API.
  - Scale Canvas (GetComponent<Canvas>().scaleFactor *= multiplier;) -> If it changes, set again.
  - Log Graphy messages to UI.Text in the scene as well as the console.
  - Make UI layout adapt on the fly to compact it when not showing graphs.
  - ~~Add a second graph to the Audio that shows the highest spectrum value in the last X samples~~
    
**Other features (DEFINITELY MAYBE):** 

  - Network stats
  - Show all Debug.Log from Unity in a UI.Text in the scene.
  - Allow storing FPS for a predetermined time to allow benchmarks.
  - Make changes in the inspector immediatly visible in the scene, without waiting to enter play mode.
  - Dump all Graphy Data as a string to:
  	- File
	- Send to server
	- Send mail
  - Add a preprocessor key #GRAPHY to avoid adding the asset in builds
  - Option to hide Graphy when not in Play Mode
  
## License

Graphy is released under the [MIT license](https://github.com/Tayx94/graphy/blob/master/LICENSE). Although I don't require attribution, I would love to know if you decide to use it in a project! Let me know on [Twitter](https://twitter.com/tayx94?) or by [email](martintayx@gmail.com).
