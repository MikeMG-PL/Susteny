using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildTriggerItemAction : MonoBehaviour
{
    private void OnMouseDown()
    {
        Interactable interactable = GetComponentInParent<Interactable>();
        if (interactable == null) Debug.LogError("No Interactable script in parent");
        else interactable.TryToTriggerAction();
    }
}
