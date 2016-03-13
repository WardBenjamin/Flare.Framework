OLD CHANGELOGS:

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
