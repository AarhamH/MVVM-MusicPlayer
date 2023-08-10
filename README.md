# 🎧 Welcome to PalmPlayer 🎧 

Browse and play music on your desktop without WiFi!

[Click here to see the player in action!](https://youtu.be/HRxdCkUZAug)
## Table of Contents

- [Project Description](#project-description)
- [Features](#features)
- [Installation and Running for Visual Studio](#installation-and-running-for-visual-studio)

## Project Description

PalmPlayer, or Palm for short, is a native desktop music player application for Windows, which allows users to store MP3/WAV files into a playlist or browse media from Youtube. The program is built using .NET WPF with C# and XAML. Media and playlist information stored in a rooted SQLite database and mapped via Microsoft Entity Framework. 

<img src="https://github.com/AarhamH/dotnet-media-player/assets/105332385/7cc11524-ba70-4923-8a94-9a2049b2dcdb" width="600" height="400" /> 
<img src="https://github.com/AarhamH/dotnet-media-player/assets/105332385/2ef9f871-db8f-48ea-a042-2fde25c565ca" width="600" height="400" /> 
<img src="https://github.com/AarhamH/dotnet-media-player/assets/105332385/9e9aef89-690e-4c68-936f-c9b14bd8b6ba" width="600" height="400" /> 


## Features
- Storage for 100+ songs 
- High quality audio feedback
- Create personalized playlists with naming and banner switching features
- Audio navigation buttons (forward/backward) and volume and track sliders
- Search bar in the browsing page which fetches results from Youtube based on user input
- Beautiful UI

## Installation and Running for Visual Studio
1. Install the latest version of Visual Studio and .NET Framework SDK
2. Import the solution files into Visual Studio
3. To try out the program, open a debug window in Visual Studio or run:
```bash
$ cd "where you kept the repo"
$ MSBuild PalmPlayer.sln -restore /p:Configuration=Debug
$ cd "where you kept repo"\PalmClient\bin\Debug\net7.0-windows
$ PalmClient.exe
```
