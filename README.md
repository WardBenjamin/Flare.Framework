# Flare
###### "Make a game, not an engine"

###*Note: Flare has been superseded by the [Shift Engine](https://github.com/WardBenjamin/Shift).*

Flare is an open source game development framework written in C#, designed to allow the user to focus exclusively on their game design rather than basic engine functionality. This lets developers make their game, instead of spending time creating their own engine. Flare directly exposes native OpenGL 3 Core methods, with support for OpenGL 4+ functionality as well. Additionally, Flare handles window management and input handling for you, using SDL2 as a backend, with functionality for both mouse and keyboard input.

Additionally, plans are in place for controller input, using SDL2's controller backend, as well as audio, where there is no current planned backend, but which will tentatively use OpenAL.

Join the chat at [https://gitter.im/WardBenjamin/Flare.Framework](https://gitter.im/WardBenjamin/Flare.Framework?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge&utm_content=badge)

[![Join the chat at https://gitter.im/WardBenjamin/Flare.Framework](https://badges.gitter.im/WardBenjamin/Flare.Framework.svg)](https://gitter.im/WardBenjamin/Flare.Framework?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge&utm_content=badge)

#### Origins

Flare was designed for a personal project, originally as a way to learn OpenGL 3 concepts. Now, built using generated bindings via the OpenGL4C# project, Flare allows for silent, reliable, and out-of-the-box window handling and input. See the "Features" section for more information.

#### Development status

Flare is a work in progress. New classes are added as soon as they have been tested! We have two nuget packages, Flare.Framework and Flare.GUI. These are usable as-is, but keep in mind that there may be API changes as required in the future.

#### Usage

To use Flare, just download the Nuget package that you want and add it to a C# project. The easiest way to use Flare after that is to write a base class and entry point similar to this:

            Game game = new Game("Game1", 800, 600);
            game.IsMouseVisible = true;
            game.Run();

We are still in need of samples and more advanced documentation. In the future, once the API is more well documented, there will be online documentation available as well as sample applications.

#### Features

Note: See FEATURES.md for older changelogs.

v2.0.0
 - Complete refactor!
   - New features include the following:
     - Switch over to a pure SDL2 backend, using SDL2#.
     - Complete API change, moving to a more XNA-like structure for ease of use
     - Custom math library, including vector and matrix functionality
     - Removed sprite and text rendering, as they were generally unextensible and not useful. A different form of text rendering, as well as mesh and texture loading support, will be added in a later release.
     - Removed deprecated OpenGL bindings, and updated to use minimum OpenGL 3.0+ Core, equivalent to OpenGL 3.3+. OpenGL 4+ functionality also available.
     - Icons will automatically load based on the window title. If SDL2-image is present, these can be PNGs, otherwise only bitmaps are supported.

#### Development Policy

At the moment, Flare is completely developed by Benjamin Ward (WardBenjamin here on Github). If you would like to contribute to Flare, just open an issue and/or a pull request and we can discuss your changes.

#### License

Flare is released under the Creative Commons Attribution 4.0 International Public License. See LICENSE.md for details.

Flare uses code from the FNA project, released under the Microsoft Public License. See fna.LICENSE for details.

Flare uses the SDL2# project for SDL2 bindings, released under the zlib license. See lib/SDL2-CS/LICENSE for details.
