using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameObject viewModePosition;
    public new GameObject camera;
    public GameObject focusCamera;

    [HideInInspector] public Inventory inventory;

    private void Awake()
    {
        inventory = GetComponent<Inventory>();

        PrepareFocusCamera();
    }

    // Czy to działa?
    void PrepareFocusCamera()
    {
        focusCamera.SetActive(true);
        focusCamera.SetActive(false);
    }
}
