using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadEditor : MonoBehaviour
{
    GameObject player;
    Camera cam;
    public GameObject sphere;

    void Start()
    {
        cam = GetComponent<Camera>();
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 pos = cam.ScreenToWorldPoint(Input.mousePosition);
            Instantiate(sphere, new Vector3(pos.x, 12.25f, pos.z), Quaternion.identity);
        }
    }
}
