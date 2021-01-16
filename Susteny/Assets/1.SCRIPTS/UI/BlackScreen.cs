using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackScreen : MonoBehaviour
{
    public RuntimeAnimatorController Fade;
    public RuntimeAnimatorController Unfade;

    public void Animating(bool b)
    {
        GetComponent<Animator>().enabled = b;
    }
}
