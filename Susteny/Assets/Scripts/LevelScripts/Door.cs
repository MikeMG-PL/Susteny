using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    ViewMode v;

    void Start()
    {
        v = GameObject.FindGameObjectWithTag("Player").GetComponent<ViewMode>();
    }

    void OnMouseDown()
    {
        v.ToggleViewMode(null, false, false);
    }

    void FadeIn()
    {
        ;
    }

    void EnterTheBuilding()
    {
        ;
    }

}
