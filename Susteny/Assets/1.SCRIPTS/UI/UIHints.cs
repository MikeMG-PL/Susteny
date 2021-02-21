using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIHints : MonoBehaviour
{
    [Header("Hints during exploration, that are shown near the crosshair")]
    public string defaultNearCrosshairHint;
    public TMP_Text nearCrosshairHint;

    public Transform cornerHintsParent;
    public GameObject cornerHint;

    public float cornerHintsShowSpeed = 5f;
    public float crosshairHintShowSpeed = 7f;

    List<GameObject> cornerHints = new List<GameObject>();
    bool showCornerHints;
    bool hideCornerHints;
    bool showCrosshairHint;
    bool hideCrosshairHint;

    public void ShowCornerHints(List<string> hints)
    {
        hideCornerHints = false;
        DestroyCornerHints();

        if (hints.Count == 0) return;

        for (int i = hints.Count - 1; i >= 0; i--)
        {
            if (!string.IsNullOrEmpty(hints[i]))
            {
                GameObject obj = Instantiate(cornerHint, cornerHintsParent);
                TMP_Text objHint = obj.GetComponent<TMP_Text>();
                objHint.text = hints[i];
                cornerHints.Add(obj);
            }
        }

        showCornerHints = true;
    }

    void Update()
    {
        if (showCornerHints) GradualShowCornerHints();
        if (hideCornerHints) GradualHideCornerHints();
        if (showCrosshairHint) GradualShowCrosshairHint();
        if (hideCrosshairHint) GradualHideCrosshairHint();
    }

    // TODO: teoretycznie można by zamiast usuwania przedmiotów jakoś je używać ponownie, może byłoby to szybsze?
    // Przy tak niewielu elementach wątpię jednak by różnica była duża
    public void DestroyCornerHints()
    {
        for (int i = cornerHints.Count - 1; i >= 0; i--)
        {
            Destroy(cornerHints[i]);
        }

        cornerHints.Clear();
    }

    public void HideCornerHints()
    {
        showCornerHints = false;
        hideCornerHints = true;
    }

    void GradualShowCornerHints()
    {
        for (int i = cornerHints.Count - 1; i >= 0; i--)
        {
            Color color = cornerHints[i].GetComponent<TMP_Text>().color;
            cornerHints[i].GetComponent<TMP_Text>().color = new Color(color.r, color.g, color.b, color.a + (cornerHintsShowSpeed * Time.deltaTime));

            if (i == cornerHints.Count - 1 && color.a >= 1)
            {
                cornerHints[i].GetComponent<TMP_Text>().color = new Color(color.r, color.g, color.b, 1);
                showCornerHints = false;
            }
        }
    }

    void GradualHideCornerHints()
    {
        for (int i = cornerHints.Count - 1; i >= 0; i--)
        {
            Color color = cornerHints[i].GetComponent<TMP_Text>().color;
            cornerHints[i].GetComponent<TMP_Text>().color = new Color(color.r, color.g, color.b, color.a - (cornerHintsShowSpeed * Time.deltaTime));

            if (i == cornerHints.Count - 1 && color.a <= 0)
            {
                cornerHints[i].GetComponent<TMP_Text>().color = new Color(color.r, color.g, color.b, 0);
                hideCornerHints = false;
            }
        }
    }

    public void ShowCrosshairHint(string hint, bool canActuallyInteract)
    {
        if (string.IsNullOrEmpty(hint)) hint = defaultNearCrosshairHint;
        nearCrosshairHint.text = hint;
        if (canActuallyInteract)
        {
            nearCrosshairHint.gameObject.SetActive(true);
            showCrosshairHint = true;
            hideCrosshairHint = false;  
        }
    }

    public void HideCrosshairHint()
    {
        showCrosshairHint = false;
        hideCrosshairHint = true;
    }

    void GradualShowCrosshairHint()
    {
        Color color = nearCrosshairHint.color;
        nearCrosshairHint.color = new Color(color.r, color.g, color.b, color.a + (crosshairHintShowSpeed * Time.deltaTime));

        if (nearCrosshairHint.color.a >= 1)
        {
            showCrosshairHint = false;
            nearCrosshairHint.color = new Color(color.r, color.g, color.b, 1);
        }
    }

    void GradualHideCrosshairHint()
    {
        Color color = nearCrosshairHint.color;
        nearCrosshairHint.color = new Color(color.r, color.g, color.b, color.a - (crosshairHintShowSpeed * Time.deltaTime));

        if (nearCrosshairHint.color.a <= 0)
        {
            hideCrosshairHint = false;
            nearCrosshairHint.color = new Color(color.r, color.g, color.b, 0);

            nearCrosshairHint.gameObject.SetActive(false);
            nearCrosshairHint.text = string.Empty;
        }
    }
}
