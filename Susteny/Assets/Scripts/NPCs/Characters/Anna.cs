using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Anna : MonoBehaviour
{
    public GameObject photo;
    public Transform[] destinations;
    public List<TaskScriptableObject> AnnaTasks;

    SC_FPSController player;
    GameManager.Level Level;
    NavMeshAgent agent;
    bool inspectingPhoto;
    TaskSystem tasks;
    

    void Awake()
    {
        tasks = GameObject.FindGameObjectWithTag("TaskSystem").GetComponent<TaskSystem>();
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

        if(b == false && n == "Anna" && i == 1)
        {
            GetComponent<LoadDialogue>().currentDialogueID = 2;
            StartCoroutine(AnnaWalkAway());
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

    public IEnumerator AnnaWalkAway()
    {
        yield return new WaitForSeconds(3);
        agent.SetDestination(destinations[1].position);
        tasks.addTask(AnnaTasks[0]);
        tasks.addTask(AnnaTasks[1]);
    }

    void Update()
    {
        QuitViewMode();       
    }
    
    void OnDisable()
    {
        DialogueInteraction.Conversation -= ConversationEvent;
    }
}
