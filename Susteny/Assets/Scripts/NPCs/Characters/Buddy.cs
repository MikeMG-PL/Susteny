using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buddy : MonoBehaviour
{
    [HideInInspector()]
    public bool talked;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !talked)
        {
            GetComponent<DialogueInteraction>().Talk(true);
            GetComponent<LoadDialogue>().currentDialogueID = 1;
            talked = true;
        }
    }
}
