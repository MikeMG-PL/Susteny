using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.AI;

public class Anna : MonoBehaviour
{
    public List<Transform> destinations;
    public GameObject player;
    public Item photoItem;
    NavMeshAgent agent;

    public PlayableDirector timeline2;
    public PlayableDirector timeline3;

    void Awake()
    {
        DialogueInteraction.Conversation += AnnaConversationStart;
        agent = GetComponent<NavMeshAgent>();
    }

    private void OnDisable()
    {
        DialogueInteraction.Conversation -= AnnaConversationStart;
    }

    public void FindPlayer()
    {
        agent.SetDestination(destinations[0].position);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.transform == destinations[0])
            timeline2.Play();
    }

    public void SetDialogue(int i)
    {
        GetComponent<LoadDialogue>().currentDialogueID = i;
    }

    public void AnnaConversationStart(bool start, string objSpeakerName, int ID)
    {
        if(objSpeakerName == "Anna")
        {
            switch(ID)
            {
                default:
                    break;

                case 1:
                    if(!start)
                    {
                        timeline3.Play();
                    }    
                    break;
            }
        }
    }

    public void GivePhoto()
    {
        player.GetComponent<Inventory>().Add(photoItem, 1);
    }

    public void FinishPhotoViewing()
    {
        player.GetComponent<PlayerActions>().UngrabFromInventory();
    }

    // debugging part
}
