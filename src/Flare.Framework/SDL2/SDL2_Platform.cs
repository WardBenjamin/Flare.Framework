#region License
/* Flare - A framework by developers, for developers.
 * Copyright 2016 Benjamin Ward
 * 
 * Released under the Creative Commons Attribution 4.0 International Public License
 * See LICENSE.md for details.
 */
#endregion

#region Using Statements
using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Runtime.InteropServices;

using SDL2;
using Flare.Input;
#endregion

namespace Flare.SDL2
{
    static class SDL2_Platform
    {
        #region  Static Constants

        static readonly string OSVersion = SDL.SDL_GetPlatform();

        #endregion

        #region Internal Static Methods

        internal static void Init(Game game, string title, int width, int height)
        {
            SDL.SDL_SetMainReady();

            // This _should_ be the first real SDL call we make.
            SDL.SDL_Init(
                SDL.SDL_INIT_VIDEO |
                SDL.SDL_INIT_JOYSTICK |
                SDL.SDL_INIT_GAMECONTROLLER |
                SDL.SDL_INIT_HAPTIC
            );

            // Set up hinting for joystick.
            string hint = SDL.SDL_GetHint(SDL.SDL_HINT_JOYSTICK_ALLOW_BACKGROUND_EVENTS);
            if (String.IsNullOrEmpty(hint))
            {
                SDL.SDL_SetHint(
                    SDL.SDL_HINT_JOYSTICK_ALLOW_BACKGROUND_EVENTS,
                    "1"
                );
            }

            // If available, load the SDL_GameControllerDB.
            string mappingsDB = Path.Combine(
                Directory.GetCurrentDirectory(),
                "gamecontrollerdb.txt"
            );
            if (File.Exists(mappingsDB))
            {
                SDL.SDL_GameControllerAddMappingsFromFile(
                    mappingsDB
                );
            }

            // Set and initialize the SDL2 window.
            game.Window = new SDL2_GameWindow(title, width, height);

            // Disable the screensaver.
            SDL.SDL_DisableScreenSaver();

            // We hide the mouse cursor by default.
            SDL.SDL_ShowCursor(0);
        }

        internal static void Dispose(Game game)
        {
            if (game.Window != null)
            {
                /* Some window managers might try to minimize the window as we're
				 * destroying it. This looks pretty stupid and could cause problems,
				 * so set this hint right before we destroy everything.
				 */
                SDL.SDL_SetHintWithPriority(
                    SDL.SDL_HINT_VIDEO_MINIMIZE_ON_FOCUS_LOSS,
                    "0",
                    SDL.SDL_HintPriority.SDL_HINT_OVERRIDE
                );

                SDL.SDL_DestroyWindow(game.Window.Handle);

                game.Window = null;
            }

            // This should be the last SDL call we make.
            SDL.SDL_Quit();
        }

        internal static void RunLoop(Game game)
        {
            SDL.SDL_ShowWindow(game.Window.Handle);

            // Which display did we end up on?
            int displayIndex = SDL.SDL_GetWindowDisplayIndex(
                game.Window.Handle
            );

            // OSX has some interesting fullscreen features that we can use.
            bool osxUseSpaces;
            if (OSVersion.Equals("Mac OS X"))
            {
                string hint = SDL.SDL_GetHint(SDL.SDL_HINT_VIDEO_MAC_FULLSCREEN_SPACES);
                osxUseSpaces = (String.IsNullOrEmpty(hint) || hint.Equals("1"));
            }
            else
            {
                osxUseSpaces = false;
            }

            // Do we want to read keycodes or scancodes?
            SDL2_KeyboardUtil.UseScancodes = Environment.GetEnvironmentVariable(
                "FLARE_KEYBOARD_USE_SCANCODES"
            ) == "1";
            if (SDL2_KeyboardUtil.UseScancodes)
            {
                Log.Write("Using scancodes instead of keycodes!");
            }

            // Active Key List
            List<Keys> keys = new List<Keys>();

            /* Setup Text Input Control Character Arrays
			 * (Only 4 control keys supported at this time)
			 */
            bool[] _TextInputControlDown = new bool[4];
            int[] _TextInputControlRepeat = new int[4];
            bool _TextInputSuppress = false;

            SDL.SDL_Event evt;

            while (game.RunApplication)
            {
                while (SDL.SDL_PollEvent(out evt) == 1)
                {
                    // Keyboard
                    if (evt.type == SDL.SDL_EventType.SDL_KEYDOWN)
                    {
                        Keys key = SDL2_KeyboardUtil.ToXNA(ref evt.key.keysym);
                        if (!keys.Contains(key))
                        {
                            keys.Add(key);
                            if (key == Keys.Back)
                            {
                                _TextInputControlDown[0] = true;
                                _TextInputControlRepeat[0] = Environment.TickCount + 400;
                                game.OnKeyPress((char)8); // Backspace
                            }
                            else if (key == Keys.Tab)
                            {
                                _TextInputControlDown[1] = true;
                                _TextInputControlRepeat[1] = Environment.TickCount + 400;
                                game.OnKeyPress((char)9); // Tab
                            }
                            else if (key == Keys.Enter)
                            {
                                _TextInputControlDown[2] = true;
                                _TextInputControlRepeat[2] = Environment.TickCount + 400;
                                game.OnKeyPress((char)13); // Enter
                            }
                            else if (keys.Contains(Keys.LeftControl) && key == Keys.V)
                            {
                                _TextInputControlDown[3] = true;
                                _TextInputControlRepeat[3] = Environment.TickCount + 400;
                                game.OnKeyPress((char)22); // Control-V (Paste)
                                _TextInputSuppress = true;
                            }
                        }
                    }
                    else if (evt.type == SDL.SDL_EventType.SDL_KEYUP)
                    {
                        Keys key = SDL2_KeyboardUtil.ToXNA(ref evt.key.keysym);
                        if (keys.Remove(key))
                        {
                            if (key == Keys.Back)
                            {
                                _TextInputControlDown[0] = false;
                            }
                            else if (key == Keys.Tab)
                            {
                                _TextInputControlDown[1] = false;
                            }
                            else if (key == Keys.Enter)
                            {
                                _TextInputControlDown[2] = false;
                            }
                            else if ((!keys.Contains(Keys.LeftControl) && _TextInputControlDown[3]) || key == Keys.V)
                            {
                                _TextInputControlDown[3] = false;
                                _TextInputSuppress = false;
                            }
                        }
                    }

                    // Mouse Input
                    else if (evt.type == SDL.SDL_EventType.SDL_MOUSEMOTION)
                    {
                        SDL2_MouseUtil._IsWarped = false;
                    }
                    else if (evt.type == SDL.SDL_EventType.SDL_MOUSEWHEEL)
                    {
                        // 120 units per notch. TODO: Be able to set this somewhere.
                        SDL2_MouseUtil._MouseWheel += evt.wheel.y * 120;
                    }

                    // Misc. window events
                    else if (evt.type == SDL.SDL_EventType.SDL_WINDOWEVENT)
                    {
                        // Window Focus
                        if (evt.window.windowEvent == SDL.SDL_WindowEventID.SDL_WINDOWEVENT_FOCUS_GAINED)
                        {
                            game.IsFocused = true;

                            if (!osxUseSpaces)
                            {
                                // If we alt-tab away, we lose the 'fullscreen desktop' flag on some window managers.
                                SDL.SDL_SetWindowFullscreen(
                                    game.Window.Handle,
                                    (game.Window as SDL2_GameWindow).IsFullscreen ?
                                        (uint)SDL.SDL_WindowFlags.SDL_WINDOW_FULLSCREEN_DESKTOP :
                                        0
                                );
                            }

                            // Disable the screensaver when we're back.
                            SDL.SDL_DisableScreenSaver();
                        }
                        else if (evt.window.windowEvent == SDL.SDL_WindowEventID.SDL_WINDOWEVENT_FOCUS_LOST)
                        {
                            game.IsFocused = false;

                            if (!osxUseSpaces)
                            {
                                SDL.SDL_SetWindowFullscreen(game.Window.Handle, 0);
                            }

                            // Give the screensaver back, we're not that important now.
                            SDL.SDL_EnableScreenSaver();
                        }

                        // Window resize
                        else if (evt.window.windowEvent == SDL.SDL_WindowEventID.SDL_WINDOWEVENT_RESIZED)
                        {
                            SDL2_MouseUtil._WindowWidth = evt.window.data1;
                            SDL2_MouseUtil._WindowHeight = evt.window.data2;

                            // This should be called on user resize only.
                            ((SDL2_GameWindow)game.Window)._ClientSizeChanged();
                        }
                        else if (evt.window.windowEvent == SDL.SDL_WindowEventID.SDL_WINDOWEVENT_SIZE_CHANGED)
                        {
                            SDL2_MouseUtil._WindowWidth = evt.window.data1;
                            SDL2_MouseUtil._WindowHeight = evt.window.data2;
                        }

                        // Window Move
                        else if (evt.window.windowEvent == SDL.SDL_WindowEventID.SDL_WINDOWEVENT_MOVED)
                        {
                            // We don't actually do anything here.
                        }

                        // Mouse Focus
                        else if (evt.window.windowEvent == SDL.SDL_WindowEventID.SDL_WINDOWEVENT_ENTER)
                        {
                            SDL.SDL_DisableScreenSaver();
                        }
                        else if (evt.window.windowEvent == SDL.SDL_WindowEventID.SDL_WINDOWEVENT_LEAVE)
                        {
                            SDL.SDL_EnableScreenSaver();
                        }
                    }

                    // Controller device management TODO: Controller implementation
                    /*else if (evt.type == SDL.SDL_EventType.SDL_CONTROLLERDEVICEADDED)
                    {
                        _AddInstance(evt.cdevice.which);
                    }
                    else if (evt.type == SDL.SDL_EventType.SDL_CONTROLLERDEVICEREMOVED)
                    {
                        _RemoveInstance(evt.cdevice.which);
                    }*/

                    // Text input
                    else if (evt.type == SDL.SDL_EventType.SDL_TEXTINPUT && !_TextInputSuppress)
                    {
                        string text;

                        // Based on the SDL2# LPUtf8StrMarshaler
                        unsafe
                        {
                            byte* endPtr = evt.text.text;
                            while (*endPtr != 0)
                            {
                                endPtr++;
                            }
                            byte[] bytes = new byte[endPtr - evt.text.text];
                            Marshal.Copy((IntPtr)evt.text.text, bytes, 0, bytes.Length);
                            text = System.Text.Encoding.UTF8.GetString(bytes);
                        }

                        if (text.Length > 0)
                        {
                            game.OnKeyPress(text[0]);
                        }
                    }

                    // Quit
                    else if (evt.type == SDL.SDL_EventType.SDL_QUIT)
                    {
                        game.RunApplication = false;
                        break;
                    }
                }
                // Text input controls key handling
                if (_TextInputControlDown[0] && _TextInputControlRepeat[0] <= Environment.TickCount)
                {
                    game.OnKeyPress((char)8);
                }
                if (_TextInputControlDown[1] && _TextInputControlRepeat[1] <= Environment.TickCount)
                {
                    game.OnKeyPress((char)9);
                }
                if (_TextInputControlDown[2] && _TextInputControlRepeat[2] <= Environment.TickCount)
                {
                    game.OnKeyPress((char)13);
                }
                if (_TextInputControlDown[3] && _TextInputControlRepeat[3] <= Environment.TickCount)
                {
                    game.OnKeyPress((char)22);
                }

                Keyboard.SetKeys(keys);
                game.Tick();
            }

            // We out.
            game.Exit();
        }

        internal static void BeforeInitialize()
        {
            /* TODO: Controller implementation
            // We want to initialize the controllers ASAP!
            SDL.SDL_Event[] evt = new SDL.SDL_Event[1];
            SDL.SDL_PumpEvents(); // Required to get OSX device events this early.
            while (SDL.SDL_PeepEvents(
                evt,
                1,
                SDL.SDL_eventaction.SDL_GETEVENT,
                SDL.SDL_EventType.SDL_CONTROLLERDEVICEADDED,
                SDL.SDL_EventType.SDL_CONTROLLERDEVICEADDED
            ) == 1)
            {
                _AddInstance(evt[0].cdevice.which);
            }*/
        }

        // TODO: Audio
        /*static IALDevice CreateALDevice()
        {
            try
            {
                return new OpenALDevice();
            }
            catch (DllNotFoundException e)
            {
                Log.Write("OpenAL not found! Need Flare.dll.config?");
                throw e;
            }
            catch (Exception)
            {
                // We ignore device creation exceptions,
				// as they are handled down the line with Instance != null
				 
                return null;
            }
        }*/

        static void SetPresentationInterval(PresentInterval interval)
        {
            if (interval == PresentInterval.Default || interval == PresentInterval.One)
            {
                if (OSVersion.Equals("Mac OS X"))
                {
                    // Apple is a big fat liar about swap_control_tear. Use stock VSync.
                    SDL.SDL_GL_SetSwapInterval(1);
                }
                else
                {
                    if (SDL.SDL_GL_SetSwapInterval(-1) != -1)
                    {
                        Log.Write("Using EXT_swap_control_tear VSync!");
                    }
                    else
                    {
                        Log.Write("EXT_swap_control_tear unsupported. Fall back to standard VSync.");
                        SDL.SDL_ClearError();
                        SDL.SDL_GL_SetSwapInterval(1);
                    }
                }
            }
            else if (interval == PresentInterval.Immediate)
            {
                SDL.SDL_GL_SetSwapInterval(0);
            }
            else if (interval == PresentInterval.Two)
            {
                SDL.SDL_GL_SetSwapInterval(2);
            }
            else
            {
                throw new NotSupportedException("Unrecognized PresentInterval!");
            }
        }

        internal static void OnIsMouseVisibleChanged(bool visible)
        {
            SDL.SDL_ShowCursor(visible ? 1 : 0);
        }

        internal static string GetStorageRoot()
        {
            if (OSVersion.Equals("Windows"))
            {
                return Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                    "SavedGames"
                );
            }
            if (OSVersion.Equals("Mac OS X"))
            {
                string osConfigDir = Environment.GetEnvironmentVariable("HOME");
                if (String.IsNullOrEmpty(osConfigDir))
                {
                    return "."; // Oh well.
                }
                osConfigDir += "/Library/Application Support";
                return osConfigDir;
            }
            if (OSVersion.Equals("Linux"))
            {
                // Assuming a non-OSX Unix platform will follow the XDG. Which it should.
                string osConfigDir = Environment.GetEnvironmentVariable("XDG_DATA_HOME");
                if (String.IsNullOrEmpty(osConfigDir))
                {
                    osConfigDir = Environment.GetEnvironmentVariable("HOME");
                    if (String.IsNullOrEmpty(osConfigDir))
                    {
                        return "."; // Oh well.
                    }
                    osConfigDir += "/.local/share";
                }
                return osConfigDir;
            }
            throw new NotSupportedException("Unhandled SDL2 platform!");
        }

        internal static bool IsStoragePathConnected(string path)
        {
            if (OSVersion.Equals("Linux") ||
                OSVersion.Equals("Mac OS X"))
            {
                /* Linux and Mac use locally connected storage in the user's
				 * home location, which should always be "connected".
				 */
                return true;
            }
            if (OSVersion.Equals("Windows"))
            {
                try
                {
                    return new DriveInfo(path).IsReady;
                }
                catch
                {
                    // The storageRoot path is invalid / has been removed.
                    return false;
                }
            }
            throw new NotSupportedException("Unhandled SDL2 platform");
        }

        internal static void ShowRuntimeError(string title, string message)
        {
            SDL.SDL_ShowSimpleMessageBox(
                SDL.SDL_MessageBoxFlags.SDL_MESSAGEBOX_ERROR,
                title,
                message,
                IntPtr.Zero
            );
        }

        internal static void TextureDataFromStream(Stream stream, out int width, out int height, 
            out byte[] pixels, int reqWidth = -1, int reqHeight = -1, bool zoom = false)
        {
            // Load the Stream into an SDL_RWops*.
            byte[] mem = new byte[stream.Length];
            GCHandle handle = GCHandle.Alloc(mem, GCHandleType.Pinned);
            stream.Read(mem, 0, mem.Length);
            IntPtr rwops = SDL.SDL_RWFromMem(mem, mem.Length);

            // Load the SDL_Surface* from RWops, get the image data.
            IntPtr surface = SDL_image.IMG_Load_RW(rwops, 1);
            handle.Free();
            if (surface == IntPtr.Zero)
            {
                // File not found, supported, etc.
                width = 0;
                height = 0;
                pixels = null;
                return;
            }
            surface = _convertSurfaceFormat(surface);

            // Image scaling, if applicable.
            if (reqWidth != -1 && reqHeight != -1)
            {
                // Get the file surface dimensions now.
                int rw;
                int rh;
                unsafe
                {
                    SDL.SDL_Surface* surPtr = (SDL.SDL_Surface*)surface;
                    rw = surPtr->w;
                    rh = surPtr->h;
                }

                // Calculate the image scale factor.
                bool scaleWidth;
                if (zoom)
                {
                    scaleWidth = rw < rh;
                }
                else
                {
                    scaleWidth = rw > rh;
                }
                float scale;
                if (scaleWidth)
                {
                    scale = reqWidth / (float)rw;
                }
                else
                {
                    scale = reqHeight / (float)rh;
                }

                // Calculate the scaled image size, crop if zoomed.
                int resultWidth;
                int resultHeight;
                SDL.SDL_Rect crop = new SDL.SDL_Rect();
                if (zoom)
                {
                    resultWidth = reqWidth;
                    resultHeight = reqHeight;
                    if (scaleWidth)
                    {
                        crop.x = 0;
                        crop.w = rw;
                        crop.y = (int)(rh / 2 - (reqHeight / scale) / 2);
                        crop.h = (int)(reqHeight / scale);
                    }
                    else
                    {
                        crop.y = 0;
                        crop.h = rh;
                        crop.x = (int)(rw / 2 - (reqWidth / scale) / 2);
                        crop.w = (int)(reqWidth / scale);
                    }
                }
                else
                {
                    resultWidth = (int)(rw * scale);
                    resultHeight = (int)(rh * scale);
                }

                // Alloc surface, blit!
                IntPtr newSurface = SDL.SDL_CreateRGBSurface(
                    0,
                    resultWidth,
                    resultHeight,
                    32,
                    0x000000FF,
                    0x0000FF00,
                    0x00FF0000,
                    0xFF000000
                );
                SDL.SDL_SetSurfaceBlendMode(
                    surface,
                    SDL.SDL_BlendMode.SDL_BLENDMODE_NONE
                );
                if (zoom)
                {
                    SDL.SDL_BlitScaled(
                        surface,
                        ref crop,
                        newSurface,
                        IntPtr.Zero
                    );
                }
                else
                {
                    SDL.SDL_BlitScaled(
                        surface,
                        IntPtr.Zero,
                        newSurface,
                        IntPtr.Zero
                    );
                }
                SDL.SDL_FreeSurface(surface);
                surface = newSurface;
            }

            // Copy surface data to output managed byte array
            unsafe
            {
                SDL.SDL_Surface* surPtr = (SDL.SDL_Surface*)surface;
                width = surPtr->w;
                height = surPtr->h;
                pixels = new byte[width * height * 4]; // MUST be SurfaceFormat.Color!
                Marshal.Copy(surPtr->pixels, pixels, 0, pixels.Length);
            }
            SDL.SDL_FreeSurface(surface);

            /* Ensure that the alpha pixels are... well, actual alpha.
			 * You think this looks stupid, but be assured: Your paint program is
			 * almost certainly even stupider.
			 * -flibit
			 */
            for (int i = 0; i < pixels.Length; i += 4)
            {
                if (pixels[i + 3] == 0)
                {
                    pixels[i] = 0;
                    pixels[i + 1] = 0;
                    pixels[i + 2] = 0;
                }
            }
        }

        internal static void SavePNG(Stream stream, int width, int height, int imgWidth, int imgHeight, byte[] data)
        {
            // Create an SDL_Surface*, write the pixel data
            IntPtr surface = SDL.SDL_CreateRGBSurface(
                0,
                imgWidth,
                imgHeight,
                32,
                0x000000FF,
                0x0000FF00,
                0x00FF0000,
                0xFF000000
            );
            SDL.SDL_LockSurface(surface);
            unsafe
            {
                SDL.SDL_Surface* surPtr = (SDL.SDL_Surface*)surface;
                Marshal.Copy(
                    data,
                    0,
                    surPtr->pixels,
                    data.Length
                );
            }
            SDL.SDL_UnlockSurface(surface);
            data = null; // We're done with the original pixel data.

            // Blit to a scaled surface of the size we want, if needed.
            if (width != imgWidth || height != imgHeight)
            {
                IntPtr scaledSurface = SDL.SDL_CreateRGBSurface(
                    0,
                    width,
                    height,
                    32,
                    0x000000FF,
                    0x0000FF00,
                    0x00FF0000,
                    0xFF000000
                );
                SDL.SDL_BlitScaled(
                    surface,
                    IntPtr.Zero,
                    scaledSurface,
                    IntPtr.Zero
                );
                SDL.SDL_FreeSurface(surface);
                surface = scaledSurface;
            }

            // Create an SDL_RWops*, save PNG to RWops
            const int pngHeaderSize = 41;
            const int pngFooterSize = 57;
            byte[] pngOut = new byte[
                (width * height * 4) +
                pngHeaderSize +
                pngFooterSize +
                256 // FIXME: Arbitrary zlib data padding for low-res images
            ]; // Max image size
            IntPtr dst = SDL.SDL_RWFromMem(pngOut, pngOut.Length);
            SDL_image.IMG_SavePNG_RW(surface, dst, 1);
            SDL.SDL_FreeSurface(surface); // We're done with the surface.

            // Get PNG size, write to stream.
            int size = (
                (pngOut[33] << 24) |
                (pngOut[34] << 16) |
                (pngOut[35] << 8) |
                (pngOut[36])
            ) + pngHeaderSize + pngFooterSize;
            stream.Write(pngOut, 0, size);
        }

        #endregion

        #region GamePad Backend
        /*
        private enum HapticType
        {
            Simple = 0,
            LeftRight = 1,
            LeftRightMacHack = 2
        }

        // Controller device information
        private static IntPtr[] _devices = new IntPtr[GamePad.GAMEPAD_COUNT];
        private static Dictionary<int, int> _instanceList = new Dictionary<int, int>();
        private static string[] _guids = GenStringArray();

        // Haptic device information
        private static IntPtr[] _haptics = new IntPtr[GamePad.GAMEPAD_COUNT];
        private static HapticType[] _hapticTypes = new HapticType[GamePad.GAMEPAD_COUNT];

        // Light bar information
        private static string[] _lightBars = GenStringArray();

        // Cached GamePadStates/Capabilities
        private static GamePadState[] _states = new GamePadState[GamePad.GAMEPAD_COUNT];
        private static GamePadCapabilities[] _capabilities = new GamePadCapabilities[GamePad.GAMEPAD_COUNT];

        // We use this to apply XInput-like rumble effects.
        private static SDL.SDL_HapticEffect _leftRightEffect = new SDL.SDL_HapticEffect
        {
            type = SDL.SDL_HAPTIC_LEFTRIGHT,
            leftright = new SDL.SDL_HapticLeftRight
            {
                type = SDL.SDL_HAPTIC_LEFTRIGHT,
                length = SDL.SDL_HAPTIC_INFINITY,
                large_magnitude = ushort.MaxValue,
                small_magnitude = ushort.MaxValue
            }
        };

        // We use this to get left/right support on OSX via a nice driver workaround!
        private static ushort[] leftRightMacHackData = { 0, 0 };
        private static GCHandle leftRightMacHackPArry = GCHandle.Alloc(leftRightMacHackData, GCHandleType.Pinned);
        private static IntPtr leftRightMacHackPtr = leftRightMacHackPArry.AddrOfPinnedObject();
        private static SDL.SDL_HapticEffect _leftRightMacHackEffect = new SDL.SDL_HapticEffect
        {
            type = SDL.SDL_HAPTIC_CUSTOM,
            custom = new SDL.SDL_HapticCustom
            {
                type = SDL.SDL_HAPTIC_CUSTOM,
                length = SDL.SDL_HAPTIC_INFINITY,
                channels = 2,
                period = 1,
                samples = 2,
                data = leftRightMacHackPtr
            }
        };

        // FIXME: SDL_GameController config input inversion!
        private static float invertAxis = Environment.GetEnvironmentVariable(
            "FLARE_WORKAROUND_INVERT_YAXIS"
        ) == "1" ? -1.0f : 1.0f;

        
        static GamePadCapabilities GetGamePadCapabilities(int index)
        {
            if (_devices[index] == IntPtr.Zero)
            {
                return new GamePadCapabilities();
            }
            return _capabilities[index];
        }

        internal static GamePadState GetGamePadState(int index, GamePadDeadZone deadZoneMode)
        {
            IntPtr device = _devices[index];
            if (device == IntPtr.Zero)
            {
                return new GamePadState();
            }

            // Do not attempt to understand this number at all costs!
            const float DeadZoneSize = 0.27f;

            // The "master" button state is built from this.
            Buttons gc_buttonState = (Buttons)0;

            // Sticks
            GamePadThumbSticks gc_sticks = new GamePadThumbSticks(
                new Vector2(
                    (float)SDL.SDL_GameControllerGetAxis(
                        device,
                        SDL.SDL_GameControllerAxis.SDL_CONTROLLER_AXIS_LEFTX
                    ) / 32768.0f,
                    (float)SDL.SDL_GameControllerGetAxis(
                        device,
                        SDL.SDL_GameControllerAxis.SDL_CONTROLLER_AXIS_LEFTY
                    ) / -32768.0f * invertAxis
                ),
                new Vector2(
                    (float)SDL.SDL_GameControllerGetAxis(
                        device,
                        SDL.SDL_GameControllerAxis.SDL_CONTROLLER_AXIS_RIGHTX
                    ) / 32768.0f,
                    (float)SDL.SDL_GameControllerGetAxis(
                        device,
                        SDL.SDL_GameControllerAxis.SDL_CONTROLLER_AXIS_RIGHTY
                    ) / -32768.0f * invertAxis
                ),
                deadZoneMode
            );
            gc_buttonState |= READ_StickToButtons(
                gc_sticks.Left,
                Buttons.LeftThumbstickLeft,
                Buttons.LeftThumbstickRight,
                Buttons.LeftThumbstickUp,
                Buttons.LeftThumbstickDown,
                DeadZoneSize
            );
            gc_buttonState |= READ_StickToButtons(
                gc_sticks.Right,
                Buttons.RightThumbstickLeft,
                Buttons.RightThumbstickRight,
                Buttons.RightThumbstickUp,
                Buttons.RightThumbstickDown,
                DeadZoneSize
            );

            // Triggers
            GamePadTriggers gc_triggers = new GamePadTriggers(
                (float)SDL.SDL_GameControllerGetAxis(
                    device,
                    SDL.SDL_GameControllerAxis.SDL_CONTROLLER_AXIS_TRIGGERLEFT
                ) / 32768.0f,
                (float)SDL.SDL_GameControllerGetAxis(
                    device,
                    SDL.SDL_GameControllerAxis.SDL_CONTROLLER_AXIS_TRIGGERRIGHT
                ) / 32768.0f
            );
            gc_buttonState |= READ_TriggerToButton(
                gc_triggers.Left,
                Buttons.LeftTrigger,
                DeadZoneSize
            );
            gc_buttonState |= READ_TriggerToButton(
                gc_triggers.Right,
                Buttons.RightTrigger,
                DeadZoneSize
            );

            // Buttons
            if (SDL.SDL_GameControllerGetButton(device, SDL.SDL_GameControllerButton.SDL_CONTROLLER_BUTTON_A) != 0)
            {
                gc_buttonState |= Buttons.A;
            }
            if (SDL.SDL_GameControllerGetButton(device, SDL.SDL_GameControllerButton.SDL_CONTROLLER_BUTTON_B) != 0)
            {
                gc_buttonState |= Buttons.B;
            }
            if (SDL.SDL_GameControllerGetButton(device, SDL.SDL_GameControllerButton.SDL_CONTROLLER_BUTTON_X) != 0)
            {
                gc_buttonState |= Buttons.X;
            }
            if (SDL.SDL_GameControllerGetButton(device, SDL.SDL_GameControllerButton.SDL_CONTROLLER_BUTTON_Y) != 0)
            {
                gc_buttonState |= Buttons.Y;
            }
            if (SDL.SDL_GameControllerGetButton(device, SDL.SDL_GameControllerButton.SDL_CONTROLLER_BUTTON_BACK) != 0)
            {
                gc_buttonState |= Buttons.Back;
            }
            if (SDL.SDL_GameControllerGetButton(device, SDL.SDL_GameControllerButton.SDL_CONTROLLER_BUTTON_GUIDE) != 0)
            {
                gc_buttonState |= Buttons.BigButton;
            }
            if (SDL.SDL_GameControllerGetButton(device, SDL.SDL_GameControllerButton.SDL_CONTROLLER_BUTTON_START) != 0)
            {
                gc_buttonState |= Buttons.Start;
            }
            if (SDL.SDL_GameControllerGetButton(device, SDL.SDL_GameControllerButton.SDL_CONTROLLER_BUTTON_LEFTSTICK) != 0)
            {
                gc_buttonState |= Buttons.LeftStick;
            }
            if (SDL.SDL_GameControllerGetButton(device, SDL.SDL_GameControllerButton.SDL_CONTROLLER_BUTTON_RIGHTSTICK) != 0)
            {
                gc_buttonState |= Buttons.RightStick;
            }
            if (SDL.SDL_GameControllerGetButton(device, SDL.SDL_GameControllerButton.SDL_CONTROLLER_BUTTON_LEFTSHOULDER) != 0)
            {
                gc_buttonState |= Buttons.LeftShoulder;
            }
            if (SDL.SDL_GameControllerGetButton(device, SDL.SDL_GameControllerButton.SDL_CONTROLLER_BUTTON_RIGHTSHOULDER) != 0)
            {
                gc_buttonState |= Buttons.RightShoulder;
            }

            // DPad
            GamePadDPad gc_dpad;
            if (SDL.SDL_GameControllerGetButton(device, SDL.SDL_GameControllerButton.SDL_CONTROLLER_BUTTON_DPAD_UP) != 0)
            {
                gc_buttonState |= Buttons.DPadUp;
            }
            if (SDL.SDL_GameControllerGetButton(device, SDL.SDL_GameControllerButton.SDL_CONTROLLER_BUTTON_DPAD_DOWN) != 0)
            {
                gc_buttonState |= Buttons.DPadDown;
            }
            if (SDL.SDL_GameControllerGetButton(device, SDL.SDL_GameControllerButton.SDL_CONTROLLER_BUTTON_DPAD_LEFT) != 0)
            {
                gc_buttonState |= Buttons.DPadLeft;
            }
            if (SDL.SDL_GameControllerGetButton(device, SDL.SDL_GameControllerButton.SDL_CONTROLLER_BUTTON_DPAD_RIGHT) != 0)
            {
                gc_buttonState |= Buttons.DPadRight;
            }
            gc_dpad = new GamePadDPad(gc_buttonState);

            // Compile the master buttonstate
            GamePadButtons gc_buttons = new GamePadButtons(gc_buttonState);

            // Build the GamePadState, increment PacketNumber if state changed.
            GamePadState gc_builtState = new GamePadState(
                gc_sticks,
                gc_triggers,
                gc_buttons,
                gc_dpad
            );
            gc_builtState.IsConnected = true;
            gc_builtState.PacketNumber = _states[index].PacketNumber;
            if (gc_builtState != _states[index])
            {
                gc_builtState.PacketNumber += 1;
                _states[index] = gc_builtState;
            }

            return gc_builtState;
        }

        internal static bool SetGamePadVibration(int index, float leftMotor, float rightMotor)
        {
            IntPtr haptic = _haptics[index];
            HapticType type = _hapticTypes[index];

            if (haptic == IntPtr.Zero)
            {
                return false;
            }

            if (leftMotor <= 0.0f && rightMotor <= 0.0f)
            {
                SDL.SDL_HapticStopAll(haptic);
            }
            else if (type == HapticType.LeftRight)
            {
                _leftRightEffect.leftright.large_magnitude = (ushort)(65535.0f * leftMotor);
                _leftRightEffect.leftright.small_magnitude = (ushort)(65535.0f * rightMotor);
                SDL.SDL_HapticUpdateEffect(
                    haptic,
                    0,
                    ref _leftRightEffect
                );
                SDL.SDL_HapticRunEffect(
                    haptic,
                    0,
                    1
                );
            }
            else if (type == HapticType.LeftRightMacHack)
            {
                leftRightMacHackData[0] = (ushort)(65535.0f * leftMotor);
                leftRightMacHackData[1] = (ushort)(65535.0f * rightMotor);
                SDL.SDL_HapticUpdateEffect(
                    haptic,
                    0,
                    ref _leftRightMacHackEffect
                );
                SDL.SDL_HapticRunEffect(
                    haptic,
                    0,
                    1
                );
            }
            else
            {
                SDL.SDL_HapticRumblePlay(
                    haptic,
                    Math.Max(leftMotor, rightMotor),
                    SDL.SDL_HAPTIC_INFINITY // Oh dear...
                );
            }
            return true;
        }

        internal static string GetGamePadGUID(int index)
        {
            return _guids[index];
        }

        internal static void SetGamePadLightBar(int index, Color color)
        {
            if (String.IsNullOrEmpty(_lightBars[index]))
            {
                return;
            }

            string baseDir = _lightBars[index];
            try
            {
                File.WriteAllText(baseDir + "red/brightness", color.R.ToString());
                File.WriteAllText(baseDir + "green/brightness", color.G.ToString());
                File.WriteAllText(baseDir + "blue/brightness", color.B.ToString());
            }
            catch
            {
                // If something went wrong, assume the worst and just remove it.
                _lightBars[index] = String.Empty;
            }
        }

        private static void _AddInstance(int dev)
        {
            int which = -1;
            for (int i = 0; i < _devices.Length; i += 1)
            {
                if (_devices[i] == IntPtr.Zero)
                {
                    which = i;
                    break;
                }
            }
            if (which == -1)
            {
                return; // Ignoring more than 4 controllers.
            }

            // Clear the error buffer. We're about to do a LOT of dangerous stuff.
            SDL.SDL_ClearError();

            // Open the device!
            _devices[which] = SDL.SDL_GameControllerOpen(dev);

            // We use this when dealing with Haptic/GUID initialization.
            IntPtr thisJoystick = SDL.SDL_GameControllerGetJoystick(_devices[which]);

            // Pair up the instance ID to the player index.
            // FIXME: Remove check after 2.0.4? -flibit
            int thisInstance = SDL.SDL_JoystickInstanceID(thisJoystick);
            if (_instanceList.ContainsKey(thisInstance))
            {
                // Duplicate? Usually this is OSX being dumb, but...?
                _devices[which] = IntPtr.Zero;
                return;
            }
            _instanceList.Add(thisInstance, which);

            // Start with a fresh state.
            _states[which] = new GamePadState();
            _states[which].IsConnected = true;

            // Initialize the haptics for the joystick, if applicable.
            if (SDL.SDL_JoystickIsHaptic(thisJoystick) == 1)
            {
                _haptics[which] = SDL.SDL_HapticOpenFromJoystick(thisJoystick);
                if (_haptics[which] == IntPtr.Zero)
                {
                    Log.Write("HAPTIC OPEN ERROR: " + SDL.SDL_GetError());
                }
            }
            if (_haptics[which] != IntPtr.Zero)
            {
                if (OSVersion.Equals("Mac OS X") &&
                    SDL.SDL_HapticEffectSupported(_haptics[which], ref _leftRightMacHackEffect) == 1)
                {
                    _hapticTypes[which] = HapticType.LeftRightMacHack;
                    SDL.SDL_HapticNewEffect(_haptics[which], ref _leftRightMacHackEffect);
                }
                else if (!OSVersion.Equals("Mac OS X") &&
                        SDL.SDL_HapticEffectSupported(_haptics[which], ref _leftRightEffect) == 1)
                {
                    _hapticTypes[which] = HapticType.LeftRight;
                    SDL.SDL_HapticNewEffect(_haptics[which], ref _leftRightEffect);
                }
                else if (SDL.SDL_HapticRumbleSupported(_haptics[which]) == 1)
                {
                    _hapticTypes[which] = HapticType.Simple;
                    SDL.SDL_HapticRumbleInit(_haptics[which]);
                }
                else
                {
                    // We can't even play simple rumble, this haptic device is useless to us.
                    SDL.SDL_HapticClose(_haptics[which]);
                    _haptics[which] = IntPtr.Zero;
                }
            }

            // An SDL_GameController _should_ always be complete...
            _capabilities[which] = new GamePadCapabilities()
            {
                IsConnected = true,
                HasAButton = true,
                HasBButton = true,
                HasXButton = true,
                HasYButton = true,
                HasBackButton = true,
                HasStartButton = true,
                HasDPadDownButton = true,
                HasDPadLeftButton = true,
                HasDPadRightButton = true,
                HasDPadUpButton = true,
                HasLeftShoulderButton = true,
                HasRightShoulderButton = true,
                HasLeftStickButton = true,
                HasRightStickButton = true,
                HasLeftTrigger = true,
                HasRightTrigger = true,
                HasLeftXThumbStick = true,
                HasLeftYThumbStick = true,
                HasRightXThumbStick = true,
                HasRightYThumbStick = true,
                HasBigButton = true,
                HasLeftVibrationMotor = _haptics[which] != IntPtr.Zero,
                HasRightVibrationMotor = _haptics[which] != IntPtr.Zero,
                HasVoiceSupport = false
            };

            // Store the GUID string for this device
            StringBuilder result = new StringBuilder();
            byte[] resChar = new byte[33]; // FIXME: Sort of arbitrary.
            SDL.SDL_JoystickGetGUIDString(
                SDL.SDL_JoystickGetGUID(thisJoystick),
                resChar,
                resChar.Length
            );
            if (OSVersion.Equals("Linux"))
            {
                result.Append((char)resChar[8]);
                result.Append((char)resChar[9]);
                result.Append((char)resChar[10]);
                result.Append((char)resChar[11]);
                result.Append((char)resChar[16]);
                result.Append((char)resChar[17]);
                result.Append((char)resChar[18]);
                result.Append((char)resChar[19]);
            }
            else if (OSVersion.Equals("Mac OS X"))
            {
                result.Append((char)resChar[0]);
                result.Append((char)resChar[1]);
                result.Append((char)resChar[2]);
                result.Append((char)resChar[3]);
                result.Append((char)resChar[16]);
                result.Append((char)resChar[17]);
                result.Append((char)resChar[18]);
                result.Append((char)resChar[19]);
            }
            else if (OSVersion.Equals("Windows"))
            {
                bool isXInput = true;
                foreach (byte b in resChar)
                {
                    if (((char)b) != '0' && b != 0)
                    {
                        isXInput = false;
                        break;
                    }
                }
                if (isXInput)
                {
                    result.Append("xinput");
                }
                else
                {
                    result.Append((char)resChar[0]);
                    result.Append((char)resChar[1]);
                    result.Append((char)resChar[2]);
                    result.Append((char)resChar[3]);
                    result.Append((char)resChar[4]);
                    result.Append((char)resChar[5]);
                    result.Append((char)resChar[6]);
                    result.Append((char)resChar[7]);
                }
            }
            else
            {
                throw new NotSupportedException("Unhandled SDL2 platform!");
            }
            _guids[which] = result.ToString();

            // Initialize light bar
            if (OSVersion.Equals("Linux") &&
                _guids[which].Equals("4c05c405"))
            {
                // Get all of the individual PS4 LED instances
                List<string> ledList = new List<string>();
                string[] dirs = Directory.GetDirectories("/sys/class/leds/");
                foreach (string dir in dirs)
                {
                    if (dir.Contains("054C:05C4") &&
                        dir.EndsWith("blue"))
                    {
                        ledList.Add(dir.Substring(0, dir.LastIndexOf(':') + 1));
                    }
                }
                // Find how many of these are already in use
                int numLights = 0;
                for (int i = 0; i < _lightBars.Length; i += 1)
                {
                    if (!String.IsNullOrEmpty(_lightBars[i]))
                    {
                        numLights += 1;
                    }
                }
                // If all are not already in use, use the first unused light
                if (numLights < ledList.Count)
                {
                    _lightBars[which] = ledList[numLights];
                }
            }

            // Print controller information to stdout.
            Log(
                "Controller " + which.ToString() + ": " +
                SDL.SDL_GameControllerName(_devices[which])
            );
        }

        private static void _RemoveInstance(int dev)
        {
            int output;
            if (!_instanceList.TryGetValue(dev, out output))
            {
                // Odds are, this is controller 5+ getting removed.
                return;
            }
            _instanceList.Remove(dev);
            if (_haptics[output] != IntPtr.Zero)
            {
                SDL.SDL_HapticClose(_haptics[output]);
                _haptics[output] = IntPtr.Zero;
            }
            SDL.SDL_GameControllerClose(_devices[output]);
            _devices[output] = IntPtr.Zero;
            _states[output] = new GamePadState();
            _guids[output] = String.Empty;

            // A lot of errors can happen here, but honestly, they can be ignored...
            SDL.SDL_ClearError();

            Log("Removed device, player: " + output.ToString());
        }

        // GetState can convert stick values to button values
        private static Buttons READ_StickToButtons(Vector2 stick, Buttons left, Buttons right, Buttons up, Buttons down, float DeadZoneSize)
        {
            Buttons b = (Buttons)0;

            if (stick.X > DeadZoneSize)
            {
                b |= right;
            }
            if (stick.X < -DeadZoneSize)
            {
                b |= left;
            }
            if (stick.Y > DeadZoneSize)
            {
                b |= up;
            }
            if (stick.Y < -DeadZoneSize)
            {
                b |= down;
            }

            return b;
        }

        // GetState can convert trigger values to button values
        private static Buttons READ_TriggerToButton(float trigger, Buttons button, float DeadZoneSize)
        {
            Buttons b = (Buttons)0;

            if (trigger > DeadZoneSize)
            {
                b |= button;
            }

            return b;
        }

        private static string[] GenStringArray()
        {
            string[] result = new string[GamePad.GAMEPAD_COUNT];
            for (int i = 0; i < result.Length; i += 1)
            {
                result[i] = String.Empty;
            }
            return result;
        }
        */
        #endregion

        #region Private Static SDL_Surface Interop

        private static unsafe IntPtr _convertSurfaceFormat(IntPtr surface)
        {
            IntPtr result = surface;
            unsafe
            {
                SDL.SDL_Surface* surPtr = (SDL.SDL_Surface*)surface;
                SDL.SDL_PixelFormat* pixelFormatPtr = (SDL.SDL_PixelFormat*)surPtr->format;

                // SurfaceFormat.Color is SDL_PIXELFORMAT_ABGR8888
                if (pixelFormatPtr->format != SDL.SDL_PIXELFORMAT_ABGR8888)
                {
                    // Create a properly formatted copy, free the old surface
                    result = SDL.SDL_ConvertSurfaceFormat(surface, SDL.SDL_PIXELFORMAT_ABGR8888, 0);
                    SDL.SDL_FreeSurface(surface);
                }
            }
            return result;
        }

        #endregion
    }

    /// <summary>
    /// Defines how <see cref="GraphicsDevice.Present"/> updates the game window.
    /// </summary>
    public enum PresentInterval
    {
        /// <summary>
        /// Equivalent to <see cref="PresentInterval.One"/>.
        /// </summary>
        Default = 0,
        /// <summary>
        /// The driver waits for the vertical retrace period, before updating window client area. Present operations are not affected more frequently than the screen refresh rate.
        /// </summary>
        One = 1,
        /// <summary>
        /// The driver waits for the vertical retrace period, before updating window client area. Present operations are not affected more frequently than every second screen refresh.
        /// </summary>
        Two = 2,
        /// <summary>
        /// The driver updates the window client area immediately. Present operations might be affected immediately. There is no limit for framerate.
        /// </summary>
        Immediate = 3,
    }
}
