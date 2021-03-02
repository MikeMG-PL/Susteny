using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TutorialHints : MonoBehaviour
{
    public CanvasGroup tutorialCanvas;

    UIUtility utility;

    void Start()
    {
        utility = GetComponent<UIUtility>();
        StartCoroutine(ShowHint("Możesz poruszać się za pomocą klawiszy W, S, A, D", 3));
    }


    public IEnumerator ShowHint(string hint, float duration)
    {
        tutorialCanvas.GetComponentInChildren<TMP_Text>().text = hint;
        utility.FadeIn(tutorialCanvas);
        yield return new WaitForSeconds(duration);
        utility.FadeOut(tutorialCanvas);
    }
}
