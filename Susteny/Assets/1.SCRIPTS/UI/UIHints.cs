using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIHints : MonoBehaviour
{
    [Header("Hints during exploration, that are show near the crosshair")]
    public string defaultNearCrosshairHint;
    public TMP_Text nearCrosshairHint;
}
