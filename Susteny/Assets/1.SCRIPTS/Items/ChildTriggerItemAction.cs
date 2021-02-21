using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildTriggerItemAction : MonoBehaviour
{
    public Interactable interactable;

    private void Awake()
    {
        interactable = GetComponentInParent<Interactable>();
        if (interactable == null) Debug.LogError("No Interactable script in parent");
    }
}
