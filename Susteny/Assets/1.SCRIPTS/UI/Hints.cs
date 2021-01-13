using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hints : MonoBehaviour
{
    [Header("Hint shown when player is pointing at the object, while not interacting")]
    public string nearCrosshairHint;

    [Header("Hints shown in the corner of the screen, while interacting")]
    public List<string> cornerHints;
}
