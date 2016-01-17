# Flare
###### "Make a game, not an engine"

Flare is an open source game development framework written in C#, designed to allow the user to focus exclusively on their game design rather than the drawing or other areas of their game. This lets developers make their game, instead of spending time creating their own engine. Flare also directly exposes OpenGL methods from OpenTK, so it still lets you keep the extensibility and low-level rendering options if you need them.

#### Origins

Flare was designed for a personal project, originally as a way to learn OpenGL 3 concepts. Now, built using OpenTK's OpenGL bindings, Flare allows for silent, reliable, and out-of-the-box 2D graphics and text rendering. See the "Features" section for more information.

#### Development status

Flare is a work in progress. New classes are added as soon as they have been tested! We have two nuget packages, Flare.Framework and Flare.GUI. These are usable as-is, but keep in mind that there may be API changes as required in the future.

#### Usage

To use Flare, just download the Nuget package that you want and add it to a C# project. The easiest way to use Flare after that is to write a base class and entry point similar the examples that follow: 

-- TODO ---
Readd this based on new API

Basic samples will be located in the Flare.Examples repository; at the moment these do not have full coverage of the API but they will be improved and more samples will be added as I have more time for development. Documentation will be available at that point.

#### Features
v1.3.0
 - Complete refactor!
   - New features include the following:
     - Switch over to a pure SDL2 backend, using SDL2#.
     - Complete API change, moving to a more XNA-like structure for ease of use
     - Custom math library, including vector and matrix functionality
 - Note: this most likely will be the last major breaking API change. In the future, I would like to keep Flare as stable as possible in the interest of ease of use and forward/backward functionality.

v1.2.2
 - Remove annoying console output

v1.2.1
 - Add missing Game.Focused property
 - Generate documentation by default

v1.2.0
 - Access to all missing events including FocusedChanged, IconChanged, missing keyboard and mouse events, Unload, etc.
 - Access to all OpenTK.GameWindow properties excluding update and render statistics.
 - Access to missing methods, such as Game.Exit(), Game.Close(), etc.

v1.1.1 (Corresponds to Flare.GUI v1.0.1)
 - Access to keyboard events: KeyUp, KeyDown, and KeyPress
   - KeyPress can be used for text input and should work correctly with non-English keyboards and letters, unlike KeyDown and KeyUp
 - Access to mouse events: MouseDown, MouseUp, and MouseMove
   - MouseDown and MouseUp contain both left and right clicks
 - Ability to set the icon, using a System.Drawing.Icon object
 - Alterable VSync mode
 - Color constructors for Sprites and Text, using either a 0-1 scale Vector4 struct or a 0-255 scale System.Drawing.Color struct
 - (Flare.GUI) Color constructor for the FPS/Clock Display

v1.1.0
 - Complete API refactor for easier usage
 - Wrap OpenTK GameWindow class for more transparent use cases (aka you don't have to interact with OpenTK at all if not required).

v1.0.1
 - Rendering hotfix to fix crash on exit

v1.0.0
 - 2D sprite rendering, in screen space (Textures loaded using System.Drawing)
 - Text rendering using fonts exported from AngelCode's BMFont or compatible tools including http://kvazars.com/littera/.

#### Development Policy

At the moment, Flare is completely developed by Benjamin Ward (WardBenjamin here on Github). If you would like to contribute to Flare, just open an issue and/or a pull request and we can discuss your changes.

#### License

Flare is released under the Creative Commons Attribution 4.0 International Public License. See LICENSE.md for details.

Flare uses code from the FNA project, released under the Microsoft Public License. See fna.LICENSE for details.

Flare uses the SDL2# project for SDL2 bindings, released under the zlib license. See lib/SDL2-CS/LICENSE for details.