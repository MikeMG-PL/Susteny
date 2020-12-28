using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buddy : MonoBehaviour
{
    [HideInInspector()]
    public bool talked;
    public bool incrementDialogueID;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !talked)
        {
            GetComponent<DialogueInteraction>().Talk(true);
            if (incrementDialogueID)
                GetComponent<LoadDialogue>().currentDialogueID = 1;
            talked = true;
        }
    }
}
