using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class BuddyAndVlad : MonoBehaviour
{
    public enum BuddyOrVlad { Buddy, Vlad };
    public BuddyOrVlad buddyOrVlad;
    bool alreadyTriggered;
    public PlayableDirector timeline9;
    public PlayableDirector timeline10;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !alreadyTriggered)
        {
            switch(buddyOrVlad)
            {
                case BuddyOrVlad.Buddy:
                    timeline9.Play();
                    
                    break;

                case BuddyOrVlad.Vlad:
                    timeline10.Play();
                    break;
            }
            alreadyTriggered = true;
        }
    }
}
