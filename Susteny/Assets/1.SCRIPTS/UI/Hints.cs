using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hints : MonoBehaviour
{
    [Header("Hint shown when player is pointing at the object, while not interacting")]
    public string nearCrosshairHint;
    public KeybindType crosshairKeybindType;

    [Header("Hints shown in the corner of the screen, while interacting")]
    public List<string> cornerHints;
    public List<KeybindType> cornerKeybindTypes;

    // TODO: podpowiedzi powinny się aktualizować jeśli przypisanie klawiszy zostanie zmienione podczas gry
    private void Start()
    {
        InitCrosshairHint();
        InitCornerHints();
    }

    void InitCrosshairHint()
    {
        if (crosshairKeybindType != KeybindType.none)
        {
            Keybind crosshairKeybindHint;
            crosshairKeybindHint = InputManager.instance.EnumToKeybind(crosshairKeybindType);
            nearCrosshairHint = nearCrosshairHint + " [" + InputManager.KeyToCaption(crosshairKeybindHint.main) + "]";
        }
    }

    void InitCornerHints()
    {
        if (cornerHints.Count == 0) return;
        if (cornerKeybindTypes.Count == 0) return;
        Keybind keybindHint;
        for (int i = 0; i < cornerHints.Count; i++)
        {
            if (cornerKeybindTypes[i] != KeybindType.none)
            {
                keybindHint = InputManager.instance.EnumToKeybind(cornerKeybindTypes[i]);
                cornerHints[i] = cornerHints[i] + " [" + InputManager.KeyToCaption(keybindHint.main) + "]";
            }
        }
    }
}
