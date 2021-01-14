using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuilldingTrigger : MonoBehaviour
{
    public Door door;
    public GameObject pointLight;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            door.unlocked = true;
            pointLight.SetActive(true);
        }
    }
}
