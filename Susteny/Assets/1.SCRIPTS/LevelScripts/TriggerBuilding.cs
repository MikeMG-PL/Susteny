using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerBuilding : MonoBehaviour
{
    bool buildingUnlocked;

    void Start()
    {
        Prototype.UnlockBuilding += Unlock;    
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && buildingUnlocked)
            transform.GetChild(0).gameObject.SetActive(true);
    }

    void Unlock(bool b)
    {
        buildingUnlocked = b;
    }

    void OnDisable()
    {
        Prototype.UnlockBuilding -= Unlock;
    }
}
