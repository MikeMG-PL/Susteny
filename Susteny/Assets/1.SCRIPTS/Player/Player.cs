using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameObject viewModePosition;
    public new Camera camera;
    public GameObject focusCamera;

    void Awake()
    {
        PrepareFocusCamera();
    }

    // Czy to działa?
    void PrepareFocusCamera()
    {
        focusCamera.SetActive(true);
        focusCamera.SetActive(false);
    }
}
