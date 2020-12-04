using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartPanel : MonoBehaviour
{
    void Start()
    {
        Prototype.ShowStartPanel += StartPanelShow;
    }

    void OnDisable()
    {
        Prototype.ShowStartPanel -= StartPanelShow;
    }

    public void StartPanelShow(bool b)
    {
        GetComponent<Animator>().enabled = true;
    }
}
