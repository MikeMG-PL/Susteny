using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildTriggerItemAction : MonoBehaviour
{
    private void OnMouseDown()
    {
        ItemWorld parent = GetComponentInParent<ItemWorld>();
        float distance = Vector3.Distance(transform.position, parent.player.transform.position);
        if (distance > parent.interactionDistance) return;

        parent.TriggerAction();
    }
}
