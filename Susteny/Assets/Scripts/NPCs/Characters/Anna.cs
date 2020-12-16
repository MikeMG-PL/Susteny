using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Anna : MonoBehaviour
{
    public GameObject photo;
    public Transform[] destinations;

    SC_FPSController player;
    GameManager.Level Level;
    NavMeshAgent agent;
    bool inspectingPhoto;
    

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<SC_FPSController>();
        Level = GameManager.Level.Prototype;
        agent = GetComponent<NavMeshAgent>();
        agent.SetDestination(destinations[0].position);
        DialogueInteraction.Conversation += ConversationEvent;
    }

    void OnTriggerStay(Collider other)
    {

    }

    void OnTriggerEnter(Collider other)
    {
        LookAtAnna(other);
    }

    void ConversationEvent(bool b, string n, int i)
    {
        if(b == false && n == "Anna" && i == 0)
        {
            var p = Instantiate(photo);
            player.GetComponent<PlayerActions>().TakeToInventory(p.GetComponent<ItemWorld>());
            player.GetComponent<PlayerActions>().GrabFromInventory(player.GetComponent<Inventory>().GetInventory()[0].item.model);
            player.GetComponent<ViewMode>().viewingFromInventory = false;
            inspectingPhoto = true;
        }
    }

    void LookAtAnna(Collider c)
    {
        if (c.gameObject.GetComponent<Destination>() != null && c.gameObject.GetComponent<Destination>().ID == 0)
        {
            player.canLook = false;
            Camera.main.transform.LookAt(destinations[0]);
            GetComponent<DialogueInteraction>().Talk(true);
            Destroy(destinations[0].gameObject);
        }
    }

    void OnDisable()
    {
        DialogueInteraction.Conversation -= ConversationEvent;
    }

    float timer = 0;
    void QuitViewMode()
    {
        if(inspectingPhoto)
        {
            timer += Time.deltaTime;

            if (timer >= 8)
            {
                player.GetComponent<PlayerActions>().UngrabFromInventory();
                inspectingPhoto = false;
                GetComponent<LoadDialogue>().currentDialogueID = 1;
                GetComponent<DialogueInteraction>().Talk(true);
            }
        }
    }

    void Update()
    {
        QuitViewMode();       
    }
}
