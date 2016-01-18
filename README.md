# Flare
###### "Make a game, not an engine"

[![Join the chat at https://gitter.im/WardBenjamin/Flare.Framework](https://badges.gitter.im/WardBenjamin/Flare.Framework.svg)](https://gitter.im/WardBenjamin/Flare.Framework?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge&utm_content=badge)

Flare is an open source game development framework written in C#, designed to allow the user to focus exclusively on their game design rather than the drawing or other areas of their game. This lets developers make their game, instead of spending time creating their own engine. Flare also directly exposes OpenGL methods from OpenTK, so it still lets you keep the extensibility and low-level rendering options if you need them.

#### Origins

Flare was designed for a personal project, originally as a way to learn OpenGL 3 concepts. Now, built using OpenTK's OpenGL bindings, Flare allows for silent, reliable, and out-of-the-box 2D graphics and text rendering. See the "Features" section for more information.

#### Development status

Flare is a work in progress. New classes are added as soon as they have been tested! We have two nuget packages, Flare.Framework and Flare.GUI. These are usable as-is, but keep in mind that there may be API changes as required in the future.

#### Usage

To use Flare, just download the Nuget package that you want and add it to a C# project. The easiest way to use Flare after that is to write a base class and entry point similar the examples that follow: 

    // Entry point, in Program.cs or a custom location
    static void Main(string[] args)
    {
        Editor editor = new Editor();
    }
    
    // Main class, in own cs file or another custom location
    class Editor
    {
        public Editor()
        {
            // Register methods to be called on loading or every frame/update
            Game.Load += OnLoad;
            Game.RenderFrame += OnRenderFrame;
            Game.UpdateFrame += OnUpdateFrame;
            // Set window title, vsync, etc.
            Game.Run(60.0f); // Run and set FPS
        }
        private void OnLoad(object sender, EventArgs e)
        {
            // Create a spritebatch, load textures, etc.
        }
        private void OnRenderFrame(object sender, FrameEventArgs e)
        {
            Game.Clock.BeginFrame();
            Game.Clear(Color.CornflowerBlue);
            // Draw stuff with SpriteBatch.Add(Sprite) then SpriteBatch.Draw() 
            // and with SpriteBatch.AddString(Text) and SpriteBatch.DrawStrings();
            Game.SwapBuffers();
            Game.Clock.EndFrame();
        }
        private void OnUpdateFrame(object sender, FrameEventArgs e)
        {
        }
    }

Basic samples are located in the Flare.Demo project; at the moment these do not have full coverage of the API but they will be improved and more samples will be added as I have more time for development. Documentation will be available at that point.

#### Features
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
