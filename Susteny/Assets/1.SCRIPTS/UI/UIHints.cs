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

    public Transform cornerHintsParent;
    public GameObject cornerHint;

    List<TMP_Text> cornerHints = new List<TMP_Text>();

    public void ShowCornerHints(List<string> hints)
    {
        if (hints.Count == 0) return;

        for (int i = hints.Count - 1; i >= 0; i--)
        {
            if (!string.IsNullOrEmpty(hints[i]))
            {
                GameObject obj = Instantiate(cornerHint, cornerHintsParent);
                TMP_Text objHint = obj.GetComponent<TMP_Text>();
                objHint.text = hints[i];
                cornerHints.Add(objHint);
            }
        }
    }

    public void HideCornerHints()
    {
        for (int i = cornerHints.Count - 1; i >= 0; i--)
        {
            Destroy(cornerHints[i]);
        }
    }

    public void ShowCrosshairHint(string hint, bool canActuallyInteract)
    {
        if (string.IsNullOrEmpty(hint)) hint = defaultNearCrosshairHint;
        nearCrosshairHint.text = hint;
        if (canActuallyInteract) nearCrosshairHint.gameObject.SetActive(true);
    }

    public void HideCrosshairHint()
    {
        nearCrosshairHint.gameObject.SetActive(false);
        nearCrosshairHint.text = string.Empty;
    }
}
