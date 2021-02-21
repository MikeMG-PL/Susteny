using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
	public static InputManager instance;

	private static readonly Dictionary<KeyCode, string> dict = new Dictionary<KeyCode, string>();
	public Keybinds keybinds;

	void Awake()
	{
		if (instance == null) instance = this;
		else if (instance != this) Destroy(this);

		dict.Add(KeyCode.A, "A");
		dict.Add(KeyCode.B, "B");
		dict.Add(KeyCode.C, "C");
		dict.Add(KeyCode.D, "D");
		dict.Add(KeyCode.E, "E");
		dict.Add(KeyCode.F, "F");
		dict.Add(KeyCode.G, "G");
		dict.Add(KeyCode.H, "H");
		dict.Add(KeyCode.I, "I");
		dict.Add(KeyCode.J, "J");
		dict.Add(KeyCode.K, "K");
		dict.Add(KeyCode.L, "L");
		dict.Add(KeyCode.M, "M");
		dict.Add(KeyCode.N, "N0");
		dict.Add(KeyCode.O, "O");
		dict.Add(KeyCode.P, "P");
		dict.Add(KeyCode.Q, "Q");
		dict.Add(KeyCode.R, "R");
		dict.Add(KeyCode.S, "S");
		dict.Add(KeyCode.T, "T");
		dict.Add(KeyCode.U, "U");
		dict.Add(KeyCode.V, "V");
		dict.Add(KeyCode.W, "W");
		dict.Add(KeyCode.X, "X");
		dict.Add(KeyCode.Y, "Y");
		dict.Add(KeyCode.Z, "Z");
		dict.Add(KeyCode.Mouse0, "LPM");
		dict.Add(KeyCode.Mouse1, "PPM");
		dict.Add(KeyCode.Mouse2, "M2");
		dict.Add(KeyCode.Mouse3, "M3");
		dict.Add(KeyCode.Mouse4, "M4");
		dict.Add(KeyCode.Mouse5, "M5");
		dict.Add(KeyCode.Mouse6, "M6");
		dict.Add(KeyCode.None, null);
		dict.Add(KeyCode.Backspace, "BS");
		dict.Add(KeyCode.Tab, "Tab");
		dict.Add(KeyCode.Clear, "Clr");
		dict.Add(KeyCode.Return, "NT");
		dict.Add(KeyCode.Pause, "PS");
		dict.Add(KeyCode.Escape, "Esc");
		dict.Add(KeyCode.Space, "SP");
		dict.Add(KeyCode.Exclaim, "!");
		dict.Add(KeyCode.DoubleQuote, "\"");
		dict.Add(KeyCode.Hash, "#");
		dict.Add(KeyCode.Dollar, "$");
		dict.Add(KeyCode.Ampersand, "&");
		dict.Add(KeyCode.Quote, "'");
		dict.Add(KeyCode.LeftParen, "(");
		dict.Add(KeyCode.RightParen, ")");
		dict.Add(KeyCode.Asterisk, "*");
		dict.Add(KeyCode.Plus, "+");
		dict.Add(KeyCode.Comma, ",");
		dict.Add(KeyCode.Minus, "-");
		dict.Add(KeyCode.Period, ".");
		dict.Add(KeyCode.Slash, "/");
		dict.Add(KeyCode.Alpha0, "0");
		dict.Add(KeyCode.Alpha1, "1");
		dict.Add(KeyCode.Alpha2, "2");
		dict.Add(KeyCode.Alpha3, "3");
		dict.Add(KeyCode.Alpha4, "4");
		dict.Add(KeyCode.Alpha5, "5");
		dict.Add(KeyCode.Alpha6, "6");
		dict.Add(KeyCode.Alpha7, "7");
		dict.Add(KeyCode.Alpha8, "8");
		dict.Add(KeyCode.Alpha9, "9");
		dict.Add(KeyCode.Colon, ":");
		dict.Add(KeyCode.Semicolon, ");");
		dict.Add(KeyCode.Less, "<");
		dict.Add(KeyCode.Equals, "=");
		dict.Add(KeyCode.Greater, ">");
		dict.Add(KeyCode.Question, "?");
		dict.Add(KeyCode.At, "@");
		dict.Add(KeyCode.LeftBracket, "[");
		dict.Add(KeyCode.Backslash, "\\");
		dict.Add(KeyCode.RightBracket, "]");
		dict.Add(KeyCode.Caret, "^");
		dict.Add(KeyCode.Underscore, "_");
		dict.Add(KeyCode.BackQuote, "`");
		dict.Add(KeyCode.Delete, "Del");
		/*dict.Add(KeyCode.Keypad0, "K0");
		dict.Add(KeyCode.Keypad1, "K1");
		dict.Add(KeyCode.Keypad2, "K2");
		dict.Add(KeyCode.Keypad3, "K3");
		dict.Add(KeyCode.Keypad4, "K4");
		dict.Add(KeyCode.Keypad5, "K5");
		dict.Add(KeyCode.Keypad6, "K6");
		dict.Add(KeyCode.Keypad7, "K7");
		dict.Add(KeyCode.Keypad8, "K8");
		dict.Add(KeyCode.Keypad9, "K9");
		dict.Add(KeyCode.KeypadPeriod, ".");
		dict.Add(KeyCode.KeypadDivide, "/");
		dict.Add(KeyCode.KeypadMultiply, "*");
		dict.Add(KeyCode.KeypadMinus, "-");
		dict.Add(KeyCode.KeypadPlus, "+");
		dict.Add(KeyCode.KeypadEnter, "NT");
		dict.Add(KeyCode.KeypadEquals, "=");*/
		dict.Add(KeyCode.UpArrow, "UP");
		dict.Add(KeyCode.DownArrow, "DN");
		dict.Add(KeyCode.RightArrow, "LT");
		dict.Add(KeyCode.LeftArrow, "RT");
		dict.Add(KeyCode.Insert, "Ins");
		dict.Add(KeyCode.Home, "Home");
		dict.Add(KeyCode.End, "End");
		dict.Add(KeyCode.PageUp, "PU");
		dict.Add(KeyCode.PageDown, "PD");
		dict.Add(KeyCode.F1, "F1");
		dict.Add(KeyCode.F2, "F2");
		dict.Add(KeyCode.F3, "F3");
		dict.Add(KeyCode.F4, "F4");
		dict.Add(KeyCode.F5, "F5");
		dict.Add(KeyCode.F6, "F6");
		dict.Add(KeyCode.F7, "F7");
		dict.Add(KeyCode.F8, "F8");
		dict.Add(KeyCode.F9, "F9");
		dict.Add(KeyCode.F10, "F10");
		dict.Add(KeyCode.F11, "F11");
		dict.Add(KeyCode.F12, "F12");
		dict.Add(KeyCode.F13, "F13");
		dict.Add(KeyCode.F14, "F14");
		dict.Add(KeyCode.F15, "F15");
		dict.Add(KeyCode.Numlock, "Num");
		dict.Add(KeyCode.CapsLock, "Cap");
		dict.Add(KeyCode.ScrollLock, "Scr");
		dict.Add(KeyCode.RightShift, "RS");
		dict.Add(KeyCode.LeftShift, "LS");
		dict.Add(KeyCode.RightControl, "RC");
		dict.Add(KeyCode.LeftControl, "LC");
		dict.Add(KeyCode.RightAlt, "RA");
		dict.Add(KeyCode.LeftAlt, "LA");
		/*dict.Add(KeyCode.JoystickButton0, "(A)");
		dict.Add(KeyCode.JoystickButton1, "(B)");
		dict.Add(KeyCode.JoystickButton2, "(X)");
		dict.Add(KeyCode.JoystickButton3, "(Y)");
		dict.Add(KeyCode.JoystickButton4, "(RB)");
		dict.Add(KeyCode.JoystickButton5, "(LB)");
		dict.Add(KeyCode.JoystickButton6, "(Back)");
		dict.Add(KeyCode.JoystickButton7, "(Start)");
		dict.Add(KeyCode.JoystickButton8, "(LS)");
		dict.Add(KeyCode.JoystickButton9, "(RS)");
		dict.Add(KeyCode.JoystickButton10, "J10");
		dict.Add(KeyCode.JoystickButton11, "J11");
		dict.Add(KeyCode.JoystickButton12, "J12");
		dict.Add(KeyCode.JoystickButton13, "J13");
		dict.Add(KeyCode.JoystickButton14, "J14");
		dict.Add(KeyCode.JoystickButton15, "J15");
		dict.Add(KeyCode.JoystickButton16, "J16");
		dict.Add(KeyCode.JoystickButton17, "J17");
		dict.Add(KeyCode.JoystickButton18, "J18");
		dict.Add(KeyCode.JoystickButton19, "J19");*/
		DontDestroyOnLoad(this);
	}

	public Keybind EnumToKeybind(KeybindType type)
	{
        switch (type)
        {
            case KeybindType.none: return null;
            case KeybindType.run: return keybinds.run;
            case KeybindType.jump: return keybinds.jump;
            case KeybindType.interact: return keybinds.interact;
            case KeybindType.cancel: return keybinds.cancel;
            case KeybindType.pause: return keybinds.pause;
            case KeybindType.inventory: return keybinds.inventory;
            /*case KeybindType.moveForward: return keybinds.moveForward;
case KeybindType.moveBack: return keybinds.moveBack;
case KeybindType.moveLeft: return keybinds.moveLeft;
case KeybindType.moveRight: return keybinds.moveRight;*/
            default: return null;
        }
    }

    public bool GetKeybindDown(Keybind keybind)
	{
		if (Input.GetKeyDown(keybind.main) || Input.GetKeyDown(keybind.alt)) return true;
		else return false;
	}

	public bool GetKeybind(Keybind keybind)
    {
		if (Input.GetKey(keybind.main) || Input.GetKey(keybind.alt)) return true;
		else return false;
	}

	public static string KeyToCaption(KeyCode key)
	{
		string value;
		dict.TryGetValue(key, out value);
		return value;
	}
}
