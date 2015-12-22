# Flare
###### "Make a game, not an engine"

Flare is an open source game development framework written in C#, designed to allow the user to focus exclusively on their game design rather than the drawing or other areas of their game. This lets developers make their game, instead of spending time creating their own engine. Flare also directly exposes OpenGL methods from OpenTK, so it still lets you keep the extensibility and low-level rendering options if you need them.

#### Origins

Flare was designed for a personal project, originally as a way to learn OpenGL 3 concepts. Now, built using OpenTK's OpenGL bindings, Flare allows for silent, reliable, and out-of-the-box 2D graphics and text rendering. See the "Features" section for more information.

#### Development status

Flare is a work in progress. New classes are added as soon as they have been tested! We have two nuget packages, Flare.Framework and Flare.GUI. These are usable as-is, but keep in mind that there may be API changes as required in the future.

#### Usage

To use Flare, just download the Nuget package that you want and add it to a C# project. Write a class inheriting from Flare.Framework.GameBase, and then implement the OnLoad, OnRenderFrame, and OnUpdateFrame methods. Flare also depends on texture loading using System.Drawing, so don't forget to add a reference to that as well.

Basic samples are located in the Flare.Demo project; at the moment these do not have full coverage of the API but they will be improved. Documentation will be available at a later date.

#### Features

v1.0.1
 - Rendering hotfix to fix crash on exit

v1.0.0
 - 2D sprite rendering, in screen space (Textures loaded using System.Drawing)
 - Text rendering using fonts exported from AngelCode's BMFont or compatible tools including http://kvazars.com/littera/.

#### Development Policy

At the moment, Flare is completely developed by Benjamin Ward (WardBenjamin here on Github). If you would like to contribute to Flare, just open an issue and/or a pull request and we can discuss your changes.
