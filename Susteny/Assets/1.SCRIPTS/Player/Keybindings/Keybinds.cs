using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Keybinds", menuName = "Input system/Keybinds")]
public class Keybinds : ScriptableObject
{
	/*public Keybind moveForward;
	public Keybind moveBack;
	public Keybind moveLeft;
	public Keybind moveRight;*/
	public Keybind run;
	public Keybind jump;
	public Keybind interact;
	public Keybind cancel;
	public Keybind pause;
	public Keybind inventory;
}

// Uwaga!!! Zawsze dodawać nowe typy na sam koniec. Inaczej przypisane klawisze w podpowiedziach zostaną źle zaktualizowane!
public enum KeybindType
{
	none,
	run,
	jump,
	interact,
	cancel,
	pause,
	inventory,
	/*moveForward,
	moveBack,
	moveLeft,
	moveRight,*/
}

[Serializable]
public class Keybind
{
    public KeyCode main;
    public KeyCode alt;
}