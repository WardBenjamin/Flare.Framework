#region License
/* Flare - A framework by developers, for developers.
 * Copyright 2016 Benjamin Ward
 * 
 * Released under the Creative Commons Attribution 4.0 International Public License
 * See LICENSE.md for details.
 */
#endregion

#region Using Statements
using System.Collections.Generic;

using SDL2;
using Flare.Input;
#endregion

namespace Flare.SDL2
{
	internal static class SDL2_KeyboardUtil
	{
		#region Public Static Key Map Override

		public static bool UseScancodes = false;

		#endregion

		#region Private SDL2->XNA Key Hashmaps

		/* From: http://blogs.msdn.com/b/shawnhar/archive/2007/07/02/twin-paths-to-garbage-collector-nirvana.aspx
		 * "If you use an enum type as a dictionary key, internal dictionary operations will cause boxing.
		 * You can avoid this by using integer keys, and casting your enum values to ints before adding
		 * them to the dictionary."
		 */
		private static Dictionary<int, Keys> _keyMap;
		private static Dictionary<int, Keys> _scanMap;
		private static Dictionary<int, SDL.SDL_Scancode> _xnaMap;

		#endregion

		#region Hashmap Initializer Constructor

		static SDL2_KeyboardUtil()
		{
			// Create the dictionaries...
			_keyMap = new Dictionary<int, Keys>();
			_scanMap = new Dictionary<int, Keys>();
			_xnaMap = new Dictionary<int, SDL.SDL_Scancode>();

			// Then fill them with known keys that match up to XNA Keys.

			_keyMap.Add((int) SDL.SDL_Keycode.SDLK_a,		Keys.A);
			_keyMap.Add((int) SDL.SDL_Keycode.SDLK_b,		Keys.B);
			_keyMap.Add((int) SDL.SDL_Keycode.SDLK_c,		Keys.C);
			_keyMap.Add((int) SDL.SDL_Keycode.SDLK_d,		Keys.D);
			_keyMap.Add((int) SDL.SDL_Keycode.SDLK_e,		Keys.E);
			_keyMap.Add((int) SDL.SDL_Keycode.SDLK_f,		Keys.F);
			_keyMap.Add((int) SDL.SDL_Keycode.SDLK_g,		Keys.G);
			_keyMap.Add((int) SDL.SDL_Keycode.SDLK_h,		Keys.H);
			_keyMap.Add((int) SDL.SDL_Keycode.SDLK_i,		Keys.I);
			_keyMap.Add((int) SDL.SDL_Keycode.SDLK_j,		Keys.J);
			_keyMap.Add((int) SDL.SDL_Keycode.SDLK_k,		Keys.K);
			_keyMap.Add((int) SDL.SDL_Keycode.SDLK_l,		Keys.L);
			_keyMap.Add((int) SDL.SDL_Keycode.SDLK_m,		Keys.M);
			_keyMap.Add((int) SDL.SDL_Keycode.SDLK_n,		Keys.N);
			_keyMap.Add((int) SDL.SDL_Keycode.SDLK_o,		Keys.O);
			_keyMap.Add((int) SDL.SDL_Keycode.SDLK_p,		Keys.P);
			_keyMap.Add((int) SDL.SDL_Keycode.SDLK_q,		Keys.Q);
			_keyMap.Add((int) SDL.SDL_Keycode.SDLK_r,		Keys.R);
			_keyMap.Add((int) SDL.SDL_Keycode.SDLK_s,		Keys.S);
			_keyMap.Add((int) SDL.SDL_Keycode.SDLK_t,		Keys.T);
			_keyMap.Add((int) SDL.SDL_Keycode.SDLK_u,		Keys.U);
			_keyMap.Add((int) SDL.SDL_Keycode.SDLK_v,		Keys.V);
			_keyMap.Add((int) SDL.SDL_Keycode.SDLK_w,		Keys.W);
			_keyMap.Add((int) SDL.SDL_Keycode.SDLK_x,		Keys.X);
			_keyMap.Add((int) SDL.SDL_Keycode.SDLK_y,		Keys.Y);
			_keyMap.Add((int) SDL.SDL_Keycode.SDLK_z,		Keys.Z);
			_keyMap.Add((int) SDL.SDL_Keycode.SDLK_0,		Keys.D0);
			_keyMap.Add((int) SDL.SDL_Keycode.SDLK_1,		Keys.D1);
			_keyMap.Add((int) SDL.SDL_Keycode.SDLK_2,		Keys.D2);
			_keyMap.Add((int) SDL.SDL_Keycode.SDLK_3,		Keys.D3);
			_keyMap.Add((int) SDL.SDL_Keycode.SDLK_4,		Keys.D4);
			_keyMap.Add((int) SDL.SDL_Keycode.SDLK_5,		Keys.D5);
			_keyMap.Add((int) SDL.SDL_Keycode.SDLK_6,		Keys.D6);
			_keyMap.Add((int) SDL.SDL_Keycode.SDLK_7,		Keys.D7);
			_keyMap.Add((int) SDL.SDL_Keycode.SDLK_8,		Keys.D8);
			_keyMap.Add((int) SDL.SDL_Keycode.SDLK_9,		Keys.D9);
			_keyMap.Add((int) SDL.SDL_Keycode.SDLK_KP_0,		Keys.NumPad0);
			_keyMap.Add((int) SDL.SDL_Keycode.SDLK_KP_1,		Keys.NumPad1);
			_keyMap.Add((int) SDL.SDL_Keycode.SDLK_KP_2,		Keys.NumPad2);
			_keyMap.Add((int) SDL.SDL_Keycode.SDLK_KP_3,		Keys.NumPad3);
			_keyMap.Add((int) SDL.SDL_Keycode.SDLK_KP_4,		Keys.NumPad4);
			_keyMap.Add((int) SDL.SDL_Keycode.SDLK_KP_5,		Keys.NumPad5);
			_keyMap.Add((int) SDL.SDL_Keycode.SDLK_KP_6,		Keys.NumPad6);
			_keyMap.Add((int) SDL.SDL_Keycode.SDLK_KP_7,		Keys.NumPad7);
			_keyMap.Add((int) SDL.SDL_Keycode.SDLK_KP_8,		Keys.NumPad8);
			_keyMap.Add((int) SDL.SDL_Keycode.SDLK_KP_9,		Keys.NumPad9);
			_keyMap.Add((int) SDL.SDL_Keycode.SDLK_KP_CLEAR,	Keys.OemClear);
			_keyMap.Add((int) SDL.SDL_Keycode.SDLK_KP_DECIMAL,	Keys.Decimal);
			_keyMap.Add((int) SDL.SDL_Keycode.SDLK_KP_DIVIDE,	Keys.Divide);
			_keyMap.Add((int) SDL.SDL_Keycode.SDLK_KP_ENTER,	Keys.Enter);
			_keyMap.Add((int) SDL.SDL_Keycode.SDLK_KP_MINUS,	Keys.Subtract);
			_keyMap.Add((int) SDL.SDL_Keycode.SDLK_KP_MULTIPLY,	Keys.Multiply);
			_keyMap.Add((int) SDL.SDL_Keycode.SDLK_KP_PERIOD,	Keys.OemPeriod);
			_keyMap.Add((int) SDL.SDL_Keycode.SDLK_KP_PLUS,		Keys.Add);
			_keyMap.Add((int) SDL.SDL_Keycode.SDLK_F1,		Keys.F1);
			_keyMap.Add((int) SDL.SDL_Keycode.SDLK_F2,		Keys.F2);
			_keyMap.Add((int) SDL.SDL_Keycode.SDLK_F3,		Keys.F3);
			_keyMap.Add((int) SDL.SDL_Keycode.SDLK_F4,		Keys.F4);
			_keyMap.Add((int) SDL.SDL_Keycode.SDLK_F5,		Keys.F5);
			_keyMap.Add((int) SDL.SDL_Keycode.SDLK_F6,		Keys.F6);
			_keyMap.Add((int) SDL.SDL_Keycode.SDLK_F7,		Keys.F7);
			_keyMap.Add((int) SDL.SDL_Keycode.SDLK_F8,		Keys.F8);
			_keyMap.Add((int) SDL.SDL_Keycode.SDLK_F9,		Keys.F9);
			_keyMap.Add((int) SDL.SDL_Keycode.SDLK_F10,		Keys.F10);
			_keyMap.Add((int) SDL.SDL_Keycode.SDLK_F11,		Keys.F11);
			_keyMap.Add((int) SDL.SDL_Keycode.SDLK_F12,		Keys.F12);
			_keyMap.Add((int) SDL.SDL_Keycode.SDLK_F13,		Keys.F13);
			_keyMap.Add((int) SDL.SDL_Keycode.SDLK_F14,		Keys.F14);
			_keyMap.Add((int) SDL.SDL_Keycode.SDLK_F15,		Keys.F15);
			_keyMap.Add((int) SDL.SDL_Keycode.SDLK_F16,		Keys.F16);
			_keyMap.Add((int) SDL.SDL_Keycode.SDLK_F17,		Keys.F17);
			_keyMap.Add((int) SDL.SDL_Keycode.SDLK_F18,		Keys.F18);
			_keyMap.Add((int) SDL.SDL_Keycode.SDLK_F19,		Keys.F19);
			_keyMap.Add((int) SDL.SDL_Keycode.SDLK_F20,		Keys.F20);
			_keyMap.Add((int) SDL.SDL_Keycode.SDLK_F21,		Keys.F21);
			_keyMap.Add((int) SDL.SDL_Keycode.SDLK_F22,		Keys.F22);
			_keyMap.Add((int) SDL.SDL_Keycode.SDLK_F23,		Keys.F23);
			_keyMap.Add((int) SDL.SDL_Keycode.SDLK_F24,		Keys.F24);
			_keyMap.Add((int) SDL.SDL_Keycode.SDLK_SPACE,		Keys.Space);
			_keyMap.Add((int) SDL.SDL_Keycode.SDLK_UP,		Keys.Up);
			_keyMap.Add((int) SDL.SDL_Keycode.SDLK_DOWN,		Keys.Down);
			_keyMap.Add((int) SDL.SDL_Keycode.SDLK_LEFT,		Keys.Left);
			_keyMap.Add((int) SDL.SDL_Keycode.SDLK_RIGHT,		Keys.Right);
			_keyMap.Add((int) SDL.SDL_Keycode.SDLK_LALT,		Keys.LeftAlt);
			_keyMap.Add((int) SDL.SDL_Keycode.SDLK_RALT,		Keys.RightAlt);
			_keyMap.Add((int) SDL.SDL_Keycode.SDLK_LCTRL,		Keys.LeftControl);
			_keyMap.Add((int) SDL.SDL_Keycode.SDLK_RCTRL,		Keys.RightControl);
			_keyMap.Add((int) SDL.SDL_Keycode.SDLK_LGUI,		Keys.LeftWindows);
			_keyMap.Add((int) SDL.SDL_Keycode.SDLK_RGUI,		Keys.RightWindows);
			_keyMap.Add((int) SDL.SDL_Keycode.SDLK_LSHIFT,		Keys.LeftShift);
			_keyMap.Add((int) SDL.SDL_Keycode.SDLK_RSHIFT,		Keys.RightShift);
			_keyMap.Add((int) SDL.SDL_Keycode.SDLK_APPLICATION,	Keys.Apps);
			_keyMap.Add((int) SDL.SDL_Keycode.SDLK_SLASH,		Keys.OemQuestion);
			_keyMap.Add((int) SDL.SDL_Keycode.SDLK_BACKSLASH,	Keys.OemBackslash);
			_keyMap.Add((int) SDL.SDL_Keycode.SDLK_LEFTBRACKET,	Keys.OemOpenBrackets);
			_keyMap.Add((int) SDL.SDL_Keycode.SDLK_RIGHTBRACKET,	Keys.OemCloseBrackets);
			_keyMap.Add((int) SDL.SDL_Keycode.SDLK_CAPSLOCK,	Keys.CapsLock);
			_keyMap.Add((int) SDL.SDL_Keycode.SDLK_COMMA,		Keys.OemComma);
			_keyMap.Add((int) SDL.SDL_Keycode.SDLK_DELETE,		Keys.Delete);
			_keyMap.Add((int) SDL.SDL_Keycode.SDLK_END,		Keys.End);
			_keyMap.Add((int) SDL.SDL_Keycode.SDLK_BACKSPACE,	Keys.Back);
			_keyMap.Add((int) SDL.SDL_Keycode.SDLK_RETURN,		Keys.Enter);
			_keyMap.Add((int) SDL.SDL_Keycode.SDLK_ESCAPE,		Keys.Escape);
			_keyMap.Add((int) SDL.SDL_Keycode.SDLK_HOME,		Keys.Home);
			_keyMap.Add((int) SDL.SDL_Keycode.SDLK_INSERT,		Keys.Insert);
			_keyMap.Add((int) SDL.SDL_Keycode.SDLK_MINUS,		Keys.OemMinus);
			_keyMap.Add((int) SDL.SDL_Keycode.SDLK_NUMLOCKCLEAR,	Keys.NumLock);
			_keyMap.Add((int) SDL.SDL_Keycode.SDLK_PAGEUP,		Keys.PageUp);
			_keyMap.Add((int) SDL.SDL_Keycode.SDLK_PAGEDOWN,	Keys.PageDown);
			_keyMap.Add((int) SDL.SDL_Keycode.SDLK_PAUSE,		Keys.Pause);
			_keyMap.Add((int) SDL.SDL_Keycode.SDLK_PERIOD,		Keys.OemPeriod);
			_keyMap.Add((int) SDL.SDL_Keycode.SDLK_EQUALS,		Keys.OemPlus);
			_keyMap.Add((int) SDL.SDL_Keycode.SDLK_PRINTSCREEN,	Keys.PrintScreen);
			_keyMap.Add((int) SDL.SDL_Keycode.SDLK_QUOTE,		Keys.OemQuotes);
			_keyMap.Add((int) SDL.SDL_Keycode.SDLK_SCROLLLOCK,	Keys.Scroll);
			_keyMap.Add((int) SDL.SDL_Keycode.SDLK_SEMICOLON,	Keys.OemSemicolon);
			_keyMap.Add((int) SDL.SDL_Keycode.SDLK_SLEEP,		Keys.Sleep);
			_keyMap.Add((int) SDL.SDL_Keycode.SDLK_TAB,		Keys.Tab);
			_keyMap.Add((int) SDL.SDL_Keycode.SDLK_BACKQUOTE,	Keys.OemTilde);
			_keyMap.Add((int) SDL.SDL_Keycode.SDLK_UNKNOWN,		Keys.None);

			_scanMap.Add((int) SDL.SDL_Scancode.SDL_SCANCODE_A,		Keys.A);
			_scanMap.Add((int) SDL.SDL_Scancode.SDL_SCANCODE_B,		Keys.B);
			_scanMap.Add((int) SDL.SDL_Scancode.SDL_SCANCODE_C,		Keys.C);
			_scanMap.Add((int) SDL.SDL_Scancode.SDL_SCANCODE_D,		Keys.D);
			_scanMap.Add((int) SDL.SDL_Scancode.SDL_SCANCODE_E,		Keys.E);
			_scanMap.Add((int) SDL.SDL_Scancode.SDL_SCANCODE_F,		Keys.F);
			_scanMap.Add((int) SDL.SDL_Scancode.SDL_SCANCODE_G,		Keys.G);
			_scanMap.Add((int) SDL.SDL_Scancode.SDL_SCANCODE_H,		Keys.H);
			_scanMap.Add((int) SDL.SDL_Scancode.SDL_SCANCODE_I,		Keys.I);
			_scanMap.Add((int) SDL.SDL_Scancode.SDL_SCANCODE_J,		Keys.J);
			_scanMap.Add((int) SDL.SDL_Scancode.SDL_SCANCODE_K,		Keys.K);
			_scanMap.Add((int) SDL.SDL_Scancode.SDL_SCANCODE_L,		Keys.L);
			_scanMap.Add((int) SDL.SDL_Scancode.SDL_SCANCODE_M,		Keys.M);
			_scanMap.Add((int) SDL.SDL_Scancode.SDL_SCANCODE_N,		Keys.N);
			_scanMap.Add((int) SDL.SDL_Scancode.SDL_SCANCODE_O,		Keys.O);
			_scanMap.Add((int) SDL.SDL_Scancode.SDL_SCANCODE_P,		Keys.P);
			_scanMap.Add((int) SDL.SDL_Scancode.SDL_SCANCODE_Q,		Keys.Q);
			_scanMap.Add((int) SDL.SDL_Scancode.SDL_SCANCODE_R,		Keys.R);
			_scanMap.Add((int) SDL.SDL_Scancode.SDL_SCANCODE_S,		Keys.S);
			_scanMap.Add((int) SDL.SDL_Scancode.SDL_SCANCODE_T,		Keys.T);
			_scanMap.Add((int) SDL.SDL_Scancode.SDL_SCANCODE_U,		Keys.U);
			_scanMap.Add((int) SDL.SDL_Scancode.SDL_SCANCODE_V,		Keys.V);
			_scanMap.Add((int) SDL.SDL_Scancode.SDL_SCANCODE_W,		Keys.W);
			_scanMap.Add((int) SDL.SDL_Scancode.SDL_SCANCODE_X,		Keys.X);
			_scanMap.Add((int) SDL.SDL_Scancode.SDL_SCANCODE_Y,		Keys.Y);
			_scanMap.Add((int) SDL.SDL_Scancode.SDL_SCANCODE_Z,		Keys.Z);
			_scanMap.Add((int) SDL.SDL_Scancode.SDL_SCANCODE_0,		Keys.D0);
			_scanMap.Add((int) SDL.SDL_Scancode.SDL_SCANCODE_1,		Keys.D1);
			_scanMap.Add((int) SDL.SDL_Scancode.SDL_SCANCODE_2,		Keys.D2);
			_scanMap.Add((int) SDL.SDL_Scancode.SDL_SCANCODE_3,		Keys.D3);
			_scanMap.Add((int) SDL.SDL_Scancode.SDL_SCANCODE_4,		Keys.D4);
			_scanMap.Add((int) SDL.SDL_Scancode.SDL_SCANCODE_5,		Keys.D5);
			_scanMap.Add((int) SDL.SDL_Scancode.SDL_SCANCODE_6,		Keys.D6);
			_scanMap.Add((int) SDL.SDL_Scancode.SDL_SCANCODE_7,		Keys.D7);
			_scanMap.Add((int) SDL.SDL_Scancode.SDL_SCANCODE_8,		Keys.D8);
			_scanMap.Add((int) SDL.SDL_Scancode.SDL_SCANCODE_9,		Keys.D9);
			_scanMap.Add((int) SDL.SDL_Scancode.SDL_SCANCODE_KP_0,		Keys.NumPad0);
			_scanMap.Add((int) SDL.SDL_Scancode.SDL_SCANCODE_KP_1,		Keys.NumPad1);
			_scanMap.Add((int) SDL.SDL_Scancode.SDL_SCANCODE_KP_2,		Keys.NumPad2);
			_scanMap.Add((int) SDL.SDL_Scancode.SDL_SCANCODE_KP_3,		Keys.NumPad3);
			_scanMap.Add((int) SDL.SDL_Scancode.SDL_SCANCODE_KP_4,		Keys.NumPad4);
			_scanMap.Add((int) SDL.SDL_Scancode.SDL_SCANCODE_KP_5,		Keys.NumPad5);
			_scanMap.Add((int) SDL.SDL_Scancode.SDL_SCANCODE_KP_6,		Keys.NumPad6);
			_scanMap.Add((int) SDL.SDL_Scancode.SDL_SCANCODE_KP_7,		Keys.NumPad7);
			_scanMap.Add((int) SDL.SDL_Scancode.SDL_SCANCODE_KP_8,		Keys.NumPad8);
			_scanMap.Add((int) SDL.SDL_Scancode.SDL_SCANCODE_KP_9,		Keys.NumPad9);
			_scanMap.Add((int) SDL.SDL_Scancode.SDL_SCANCODE_KP_CLEAR,	Keys.OemClear);
			_scanMap.Add((int) SDL.SDL_Scancode.SDL_SCANCODE_KP_DECIMAL,	Keys.Decimal);
			_scanMap.Add((int) SDL.SDL_Scancode.SDL_SCANCODE_KP_DIVIDE,	Keys.Divide);
			_scanMap.Add((int) SDL.SDL_Scancode.SDL_SCANCODE_KP_ENTER,	Keys.Enter);
			_scanMap.Add((int) SDL.SDL_Scancode.SDL_SCANCODE_KP_MINUS,	Keys.Subtract);
			_scanMap.Add((int) SDL.SDL_Scancode.SDL_SCANCODE_KP_MULTIPLY,	Keys.Multiply);
			_scanMap.Add((int) SDL.SDL_Scancode.SDL_SCANCODE_KP_PERIOD,	Keys.OemPeriod);
			_scanMap.Add((int) SDL.SDL_Scancode.SDL_SCANCODE_KP_PLUS,	Keys.Add);
			_scanMap.Add((int) SDL.SDL_Scancode.SDL_SCANCODE_F1,		Keys.F1);
			_scanMap.Add((int) SDL.SDL_Scancode.SDL_SCANCODE_F2,		Keys.F2);
			_scanMap.Add((int) SDL.SDL_Scancode.SDL_SCANCODE_F3,		Keys.F3);
			_scanMap.Add((int) SDL.SDL_Scancode.SDL_SCANCODE_F4,		Keys.F4);
			_scanMap.Add((int) SDL.SDL_Scancode.SDL_SCANCODE_F5,		Keys.F5);
			_scanMap.Add((int) SDL.SDL_Scancode.SDL_SCANCODE_F6,		Keys.F6);
			_scanMap.Add((int) SDL.SDL_Scancode.SDL_SCANCODE_F7,		Keys.F7);
			_scanMap.Add((int) SDL.SDL_Scancode.SDL_SCANCODE_F8,		Keys.F8);
			_scanMap.Add((int) SDL.SDL_Scancode.SDL_SCANCODE_F9,		Keys.F9);
			_scanMap.Add((int) SDL.SDL_Scancode.SDL_SCANCODE_F10,		Keys.F10);
			_scanMap.Add((int) SDL.SDL_Scancode.SDL_SCANCODE_F11,		Keys.F11);
			_scanMap.Add((int) SDL.SDL_Scancode.SDL_SCANCODE_F12,		Keys.F12);
			_scanMap.Add((int) SDL.SDL_Scancode.SDL_SCANCODE_F13,		Keys.F13);
			_scanMap.Add((int) SDL.SDL_Scancode.SDL_SCANCODE_F14,		Keys.F14);
			_scanMap.Add((int) SDL.SDL_Scancode.SDL_SCANCODE_F15,		Keys.F15);
			_scanMap.Add((int) SDL.SDL_Scancode.SDL_SCANCODE_F16,		Keys.F16);
			_scanMap.Add((int) SDL.SDL_Scancode.SDL_SCANCODE_F17,		Keys.F17);
			_scanMap.Add((int) SDL.SDL_Scancode.SDL_SCANCODE_F18,		Keys.F18);
			_scanMap.Add((int) SDL.SDL_Scancode.SDL_SCANCODE_F19,		Keys.F19);
			_scanMap.Add((int) SDL.SDL_Scancode.SDL_SCANCODE_F20,		Keys.F20);
			_scanMap.Add((int) SDL.SDL_Scancode.SDL_SCANCODE_F21,		Keys.F21);
			_scanMap.Add((int) SDL.SDL_Scancode.SDL_SCANCODE_F22,		Keys.F22);
			_scanMap.Add((int) SDL.SDL_Scancode.SDL_SCANCODE_F23,		Keys.F23);
			_scanMap.Add((int) SDL.SDL_Scancode.SDL_SCANCODE_F24,		Keys.F24);
			_scanMap.Add((int) SDL.SDL_Scancode.SDL_SCANCODE_SPACE,		Keys.Space);
			_scanMap.Add((int) SDL.SDL_Scancode.SDL_SCANCODE_UP,		Keys.Up);
			_scanMap.Add((int) SDL.SDL_Scancode.SDL_SCANCODE_DOWN,		Keys.Down);
			_scanMap.Add((int) SDL.SDL_Scancode.SDL_SCANCODE_LEFT,		Keys.Left);
			_scanMap.Add((int) SDL.SDL_Scancode.SDL_SCANCODE_RIGHT,		Keys.Right);
			_scanMap.Add((int) SDL.SDL_Scancode.SDL_SCANCODE_LALT,		Keys.LeftAlt);
			_scanMap.Add((int) SDL.SDL_Scancode.SDL_SCANCODE_RALT,		Keys.RightAlt);
			_scanMap.Add((int) SDL.SDL_Scancode.SDL_SCANCODE_LCTRL,		Keys.LeftControl);
			_scanMap.Add((int) SDL.SDL_Scancode.SDL_SCANCODE_RCTRL,		Keys.RightControl);
			_scanMap.Add((int) SDL.SDL_Scancode.SDL_SCANCODE_LGUI,		Keys.LeftWindows);
			_scanMap.Add((int) SDL.SDL_Scancode.SDL_SCANCODE_RGUI,		Keys.RightWindows);
			_scanMap.Add((int) SDL.SDL_Scancode.SDL_SCANCODE_LSHIFT,	Keys.LeftShift);
			_scanMap.Add((int) SDL.SDL_Scancode.SDL_SCANCODE_RSHIFT,	Keys.RightShift);
			_scanMap.Add((int) SDL.SDL_Scancode.SDL_SCANCODE_APPLICATION,	Keys.Apps);
			_scanMap.Add((int) SDL.SDL_Scancode.SDL_SCANCODE_SLASH,		Keys.OemQuestion);
			_scanMap.Add((int) SDL.SDL_Scancode.SDL_SCANCODE_BACKSLASH,	Keys.OemBackslash);
			_scanMap.Add((int) SDL.SDL_Scancode.SDL_SCANCODE_LEFTBRACKET,	Keys.OemOpenBrackets);
			_scanMap.Add((int) SDL.SDL_Scancode.SDL_SCANCODE_RIGHTBRACKET,	Keys.OemCloseBrackets);
			_scanMap.Add((int) SDL.SDL_Scancode.SDL_SCANCODE_CAPSLOCK,	Keys.CapsLock);
			_scanMap.Add((int) SDL.SDL_Scancode.SDL_SCANCODE_COMMA,		Keys.OemComma);
			_scanMap.Add((int) SDL.SDL_Scancode.SDL_SCANCODE_DELETE,	Keys.Delete);
			_scanMap.Add((int) SDL.SDL_Scancode.SDL_SCANCODE_END,		Keys.End);
			_scanMap.Add((int) SDL.SDL_Scancode.SDL_SCANCODE_BACKSPACE,	Keys.Back);
			_scanMap.Add((int) SDL.SDL_Scancode.SDL_SCANCODE_RETURN,	Keys.Enter);
			_scanMap.Add((int) SDL.SDL_Scancode.SDL_SCANCODE_ESCAPE,	Keys.Escape);
			_scanMap.Add((int) SDL.SDL_Scancode.SDL_SCANCODE_HOME,		Keys.Home);
			_scanMap.Add((int) SDL.SDL_Scancode.SDL_SCANCODE_INSERT,	Keys.Insert);
			_scanMap.Add((int) SDL.SDL_Scancode.SDL_SCANCODE_MINUS,		Keys.OemMinus);
			_scanMap.Add((int) SDL.SDL_Scancode.SDL_SCANCODE_NUMLOCKCLEAR,	Keys.NumLock);
			_scanMap.Add((int) SDL.SDL_Scancode.SDL_SCANCODE_PAGEUP,	Keys.PageUp);
			_scanMap.Add((int) SDL.SDL_Scancode.SDL_SCANCODE_PAGEDOWN,	Keys.PageDown);
			_scanMap.Add((int) SDL.SDL_Scancode.SDL_SCANCODE_PAUSE,		Keys.Pause);
			_scanMap.Add((int) SDL.SDL_Scancode.SDL_SCANCODE_PERIOD,	Keys.OemPeriod);
			_scanMap.Add((int) SDL.SDL_Scancode.SDL_SCANCODE_EQUALS,	Keys.OemPlus);
			_scanMap.Add((int) SDL.SDL_Scancode.SDL_SCANCODE_PRINTSCREEN,	Keys.PrintScreen);
			_scanMap.Add((int) SDL.SDL_Scancode.SDL_SCANCODE_APOSTROPHE,	Keys.OemQuotes);
			_scanMap.Add((int) SDL.SDL_Scancode.SDL_SCANCODE_SCROLLLOCK,	Keys.Scroll);
			_scanMap.Add((int) SDL.SDL_Scancode.SDL_SCANCODE_SEMICOLON,	Keys.OemSemicolon);
			_scanMap.Add((int) SDL.SDL_Scancode.SDL_SCANCODE_SLEEP,		Keys.Sleep);
			_scanMap.Add((int) SDL.SDL_Scancode.SDL_SCANCODE_TAB,		Keys.Tab);
			_scanMap.Add((int) SDL.SDL_Scancode.SDL_SCANCODE_GRAVE,		Keys.OemTilde);
			_scanMap.Add((int) SDL.SDL_Scancode.SDL_SCANCODE_UNKNOWN,	Keys.None);

			// Also, fill up another with the reverse, for scancode->keycode lookups

			_xnaMap.Add((int) Keys.A,			SDL.SDL_Scancode.SDL_SCANCODE_A);
			_xnaMap.Add((int) Keys.B,			SDL.SDL_Scancode.SDL_SCANCODE_B);
			_xnaMap.Add((int) Keys.C,			SDL.SDL_Scancode.SDL_SCANCODE_C);
			_xnaMap.Add((int) Keys.D,			SDL.SDL_Scancode.SDL_SCANCODE_D);
			_xnaMap.Add((int) Keys.E,			SDL.SDL_Scancode.SDL_SCANCODE_E);
			_xnaMap.Add((int) Keys.F,			SDL.SDL_Scancode.SDL_SCANCODE_F);
			_xnaMap.Add((int) Keys.G,			SDL.SDL_Scancode.SDL_SCANCODE_G);
			_xnaMap.Add((int) Keys.H,			SDL.SDL_Scancode.SDL_SCANCODE_H);
			_xnaMap.Add((int) Keys.I,			SDL.SDL_Scancode.SDL_SCANCODE_I);
			_xnaMap.Add((int) Keys.J,			SDL.SDL_Scancode.SDL_SCANCODE_J);
			_xnaMap.Add((int) Keys.K,			SDL.SDL_Scancode.SDL_SCANCODE_K);
			_xnaMap.Add((int) Keys.L,			SDL.SDL_Scancode.SDL_SCANCODE_L);
			_xnaMap.Add((int) Keys.M,			SDL.SDL_Scancode.SDL_SCANCODE_M);
			_xnaMap.Add((int) Keys.N,			SDL.SDL_Scancode.SDL_SCANCODE_N);
			_xnaMap.Add((int) Keys.O,			SDL.SDL_Scancode.SDL_SCANCODE_O);
			_xnaMap.Add((int) Keys.P,			SDL.SDL_Scancode.SDL_SCANCODE_P);
			_xnaMap.Add((int) Keys.Q,			SDL.SDL_Scancode.SDL_SCANCODE_Q);
			_xnaMap.Add((int) Keys.R,			SDL.SDL_Scancode.SDL_SCANCODE_R);
			_xnaMap.Add((int) Keys.S,			SDL.SDL_Scancode.SDL_SCANCODE_S);
			_xnaMap.Add((int) Keys.T,			SDL.SDL_Scancode.SDL_SCANCODE_T);
			_xnaMap.Add((int) Keys.U,			SDL.SDL_Scancode.SDL_SCANCODE_U);
			_xnaMap.Add((int) Keys.V,			SDL.SDL_Scancode.SDL_SCANCODE_V);
			_xnaMap.Add((int) Keys.W,			SDL.SDL_Scancode.SDL_SCANCODE_W);
			_xnaMap.Add((int) Keys.X,			SDL.SDL_Scancode.SDL_SCANCODE_X);
			_xnaMap.Add((int) Keys.Y,			SDL.SDL_Scancode.SDL_SCANCODE_Y);
			_xnaMap.Add((int) Keys.Z,			SDL.SDL_Scancode.SDL_SCANCODE_Z);
			_xnaMap.Add((int) Keys.D0,			SDL.SDL_Scancode.SDL_SCANCODE_0);
			_xnaMap.Add((int) Keys.D1,			SDL.SDL_Scancode.SDL_SCANCODE_1);
			_xnaMap.Add((int) Keys.D2,			SDL.SDL_Scancode.SDL_SCANCODE_2);
			_xnaMap.Add((int) Keys.D3,			SDL.SDL_Scancode.SDL_SCANCODE_3);
			_xnaMap.Add((int) Keys.D4,			SDL.SDL_Scancode.SDL_SCANCODE_4);
			_xnaMap.Add((int) Keys.D5,			SDL.SDL_Scancode.SDL_SCANCODE_5);
			_xnaMap.Add((int) Keys.D6,			SDL.SDL_Scancode.SDL_SCANCODE_6);
			_xnaMap.Add((int) Keys.D7,			SDL.SDL_Scancode.SDL_SCANCODE_7);
			_xnaMap.Add((int) Keys.D8,			SDL.SDL_Scancode.SDL_SCANCODE_8);
			_xnaMap.Add((int) Keys.D9,			SDL.SDL_Scancode.SDL_SCANCODE_9);
			_xnaMap.Add((int) Keys.NumPad0,			SDL.SDL_Scancode.SDL_SCANCODE_KP_0);
			_xnaMap.Add((int) Keys.NumPad1,			SDL.SDL_Scancode.SDL_SCANCODE_KP_1);
			_xnaMap.Add((int) Keys.NumPad2,			SDL.SDL_Scancode.SDL_SCANCODE_KP_2);
			_xnaMap.Add((int) Keys.NumPad3,			SDL.SDL_Scancode.SDL_SCANCODE_KP_3);
			_xnaMap.Add((int) Keys.NumPad4,			SDL.SDL_Scancode.SDL_SCANCODE_KP_4);
			_xnaMap.Add((int) Keys.NumPad5,			SDL.SDL_Scancode.SDL_SCANCODE_KP_5);
			_xnaMap.Add((int) Keys.NumPad6,			SDL.SDL_Scancode.SDL_SCANCODE_KP_6);
			_xnaMap.Add((int) Keys.NumPad7,			SDL.SDL_Scancode.SDL_SCANCODE_KP_7);
			_xnaMap.Add((int) Keys.NumPad8,			SDL.SDL_Scancode.SDL_SCANCODE_KP_8);
			_xnaMap.Add((int) Keys.NumPad9,			SDL.SDL_Scancode.SDL_SCANCODE_KP_9);
			_xnaMap.Add((int) Keys.OemClear,		SDL.SDL_Scancode.SDL_SCANCODE_KP_CLEAR);
			_xnaMap.Add((int) Keys.Decimal,			SDL.SDL_Scancode.SDL_SCANCODE_KP_DECIMAL);
			_xnaMap.Add((int) Keys.Divide,			SDL.SDL_Scancode.SDL_SCANCODE_KP_DIVIDE);
			_xnaMap.Add((int) Keys.Multiply,		SDL.SDL_Scancode.SDL_SCANCODE_KP_MULTIPLY);
			_xnaMap.Add((int) Keys.Subtract,		SDL.SDL_Scancode.SDL_SCANCODE_KP_MINUS);
			_xnaMap.Add((int) Keys.Add,			SDL.SDL_Scancode.SDL_SCANCODE_KP_PLUS);
			_xnaMap.Add((int) Keys.F1,			SDL.SDL_Scancode.SDL_SCANCODE_F1);
			_xnaMap.Add((int) Keys.F2,			SDL.SDL_Scancode.SDL_SCANCODE_F2);
			_xnaMap.Add((int) Keys.F3,			SDL.SDL_Scancode.SDL_SCANCODE_F3);
			_xnaMap.Add((int) Keys.F4,			SDL.SDL_Scancode.SDL_SCANCODE_F4);
			_xnaMap.Add((int) Keys.F5,			SDL.SDL_Scancode.SDL_SCANCODE_F5);
			_xnaMap.Add((int) Keys.F6,			SDL.SDL_Scancode.SDL_SCANCODE_F6);
			_xnaMap.Add((int) Keys.F7,			SDL.SDL_Scancode.SDL_SCANCODE_F7);
			_xnaMap.Add((int) Keys.F8,			SDL.SDL_Scancode.SDL_SCANCODE_F8);
			_xnaMap.Add((int) Keys.F9,			SDL.SDL_Scancode.SDL_SCANCODE_F9);
			_xnaMap.Add((int) Keys.F10,			SDL.SDL_Scancode.SDL_SCANCODE_F10);
			_xnaMap.Add((int) Keys.F11,			SDL.SDL_Scancode.SDL_SCANCODE_F11);
			_xnaMap.Add((int) Keys.F12,			SDL.SDL_Scancode.SDL_SCANCODE_F12);
			_xnaMap.Add((int) Keys.F13,			SDL.SDL_Scancode.SDL_SCANCODE_F13);
			_xnaMap.Add((int) Keys.F14,			SDL.SDL_Scancode.SDL_SCANCODE_F14);
			_xnaMap.Add((int) Keys.F15,			SDL.SDL_Scancode.SDL_SCANCODE_F15);
			_xnaMap.Add((int) Keys.F16,			SDL.SDL_Scancode.SDL_SCANCODE_F16);
			_xnaMap.Add((int) Keys.F17,			SDL.SDL_Scancode.SDL_SCANCODE_F17);
			_xnaMap.Add((int) Keys.F18,			SDL.SDL_Scancode.SDL_SCANCODE_F18);
			_xnaMap.Add((int) Keys.F19,			SDL.SDL_Scancode.SDL_SCANCODE_F19);
			_xnaMap.Add((int) Keys.F20,			SDL.SDL_Scancode.SDL_SCANCODE_F20);
			_xnaMap.Add((int) Keys.F21,			SDL.SDL_Scancode.SDL_SCANCODE_F21);
			_xnaMap.Add((int) Keys.F22,			SDL.SDL_Scancode.SDL_SCANCODE_F22);
			_xnaMap.Add((int) Keys.F23,			SDL.SDL_Scancode.SDL_SCANCODE_F23);
			_xnaMap.Add((int) Keys.F24,			SDL.SDL_Scancode.SDL_SCANCODE_F24);
			_xnaMap.Add((int) Keys.Space,			SDL.SDL_Scancode.SDL_SCANCODE_SPACE);
			_xnaMap.Add((int) Keys.Up,			SDL.SDL_Scancode.SDL_SCANCODE_UP);
			_xnaMap.Add((int) Keys.Down,			SDL.SDL_Scancode.SDL_SCANCODE_DOWN);
			_xnaMap.Add((int) Keys.Left,			SDL.SDL_Scancode.SDL_SCANCODE_LEFT);
			_xnaMap.Add((int) Keys.Right,			SDL.SDL_Scancode.SDL_SCANCODE_RIGHT);
			_xnaMap.Add((int) Keys.LeftAlt,			SDL.SDL_Scancode.SDL_SCANCODE_LALT);
			_xnaMap.Add((int) Keys.RightAlt,		SDL.SDL_Scancode.SDL_SCANCODE_RALT);
			_xnaMap.Add((int) Keys.LeftControl,		SDL.SDL_Scancode.SDL_SCANCODE_LCTRL);
			_xnaMap.Add((int) Keys.RightControl,		SDL.SDL_Scancode.SDL_SCANCODE_RCTRL);
			_xnaMap.Add((int) Keys.LeftWindows,		SDL.SDL_Scancode.SDL_SCANCODE_LGUI);
			_xnaMap.Add((int) Keys.RightWindows,		SDL.SDL_Scancode.SDL_SCANCODE_RGUI);
			_xnaMap.Add((int) Keys.LeftShift,		SDL.SDL_Scancode.SDL_SCANCODE_LSHIFT);
			_xnaMap.Add((int) Keys.RightShift,		SDL.SDL_Scancode.SDL_SCANCODE_RSHIFT);
			_xnaMap.Add((int) Keys.Apps,			SDL.SDL_Scancode.SDL_SCANCODE_APPLICATION);
			_xnaMap.Add((int) Keys.OemQuestion,		SDL.SDL_Scancode.SDL_SCANCODE_SLASH);
			_xnaMap.Add((int) Keys.OemBackslash,		SDL.SDL_Scancode.SDL_SCANCODE_BACKSLASH);
			_xnaMap.Add((int) Keys.OemOpenBrackets,		SDL.SDL_Scancode.SDL_SCANCODE_LEFTBRACKET);
			_xnaMap.Add((int) Keys.OemCloseBrackets,	SDL.SDL_Scancode.SDL_SCANCODE_RIGHTBRACKET);
			_xnaMap.Add((int) Keys.CapsLock,		SDL.SDL_Scancode.SDL_SCANCODE_CAPSLOCK);
			_xnaMap.Add((int) Keys.OemComma,		SDL.SDL_Scancode.SDL_SCANCODE_COMMA);
			_xnaMap.Add((int) Keys.Delete,			SDL.SDL_Scancode.SDL_SCANCODE_DELETE);
			_xnaMap.Add((int) Keys.End,			SDL.SDL_Scancode.SDL_SCANCODE_END);
			_xnaMap.Add((int) Keys.Back,			SDL.SDL_Scancode.SDL_SCANCODE_BACKSPACE);
			_xnaMap.Add((int) Keys.Enter,			SDL.SDL_Scancode.SDL_SCANCODE_RETURN);
			_xnaMap.Add((int) Keys.Escape,			SDL.SDL_Scancode.SDL_SCANCODE_ESCAPE);
			_xnaMap.Add((int) Keys.Home,			SDL.SDL_Scancode.SDL_SCANCODE_HOME);
			_xnaMap.Add((int) Keys.Insert,			SDL.SDL_Scancode.SDL_SCANCODE_INSERT);
			_xnaMap.Add((int) Keys.OemMinus,		SDL.SDL_Scancode.SDL_SCANCODE_MINUS);
			_xnaMap.Add((int) Keys.NumLock,			SDL.SDL_Scancode.SDL_SCANCODE_NUMLOCKCLEAR);
			_xnaMap.Add((int) Keys.PageUp,			SDL.SDL_Scancode.SDL_SCANCODE_PAGEUP);
			_xnaMap.Add((int) Keys.PageDown,		SDL.SDL_Scancode.SDL_SCANCODE_PAGEDOWN);
			_xnaMap.Add((int) Keys.Pause,			SDL.SDL_Scancode.SDL_SCANCODE_PAUSE);
			_xnaMap.Add((int) Keys.OemPeriod,		SDL.SDL_Scancode.SDL_SCANCODE_PERIOD);
			_xnaMap.Add((int) Keys.OemPlus,			SDL.SDL_Scancode.SDL_SCANCODE_EQUALS);
			_xnaMap.Add((int) Keys.PrintScreen,		SDL.SDL_Scancode.SDL_SCANCODE_PRINTSCREEN);
			_xnaMap.Add((int) Keys.OemQuotes,		SDL.SDL_Scancode.SDL_SCANCODE_APOSTROPHE);
			_xnaMap.Add((int) Keys.Scroll,			SDL.SDL_Scancode.SDL_SCANCODE_SCROLLLOCK);
			_xnaMap.Add((int) Keys.OemSemicolon,		SDL.SDL_Scancode.SDL_SCANCODE_SEMICOLON);
			_xnaMap.Add((int) Keys.Sleep,			SDL.SDL_Scancode.SDL_SCANCODE_SLEEP);
			_xnaMap.Add((int) Keys.Tab,			SDL.SDL_Scancode.SDL_SCANCODE_TAB);
			_xnaMap.Add((int) Keys.OemTilde,		SDL.SDL_Scancode.SDL_SCANCODE_GRAVE);
			_xnaMap.Add((int) Keys.None,			SDL.SDL_Scancode.SDL_SCANCODE_UNKNOWN);
		}

		#endregion

		#region Public SDL2<->XNA Key Conversion Methods

		public static Keys ToXNA(ref SDL.SDL_Keysym key)
		{
			Keys retVal;
			if (UseScancodes)
			{
				if (_scanMap.TryGetValue((int) key.scancode, out retVal))
				{
					return retVal;
				}
			}
			else
			{
				if (_keyMap.TryGetValue((int) key.sym, out retVal))
				{
					return retVal;
				}
			}
			Log.Write(
				"KEY/SCANCODE MISSING FROM SDL2->XNA DICTIONARY: " +
				key.sym.ToString() + " " +
				key.scancode.ToString()
			);
			return Keys.None;
		}

		public static Keys GetKeyFromScancode(Keys scancode)
		{
			SDL.SDL_Scancode retVal;
			if (_xnaMap.TryGetValue((int) scancode, out retVal))
			{
				return _keyMap[(int) SDL.SDL_GetKeyFromScancode(retVal)];
			}
			else
			{
                Log.Write("SCANCODE MISSING FROM XNA->SDL2 DICTIONARY: " + scancode.ToString());
				return Keys.None;
			}
		}

		#endregion
	}
}

