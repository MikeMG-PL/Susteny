using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameObject viewModePosition;
    public new GameObject camera;
    public GameObject focusCamera;

    public Inventory inventory;

    private void Awake()
    {
        inventory = GetComponent<Inventory>();
    }
}
