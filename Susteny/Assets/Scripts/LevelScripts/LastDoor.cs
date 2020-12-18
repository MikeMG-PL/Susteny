using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LastDoor : MonoBehaviour
{
    public GameObject Vlad;
    public Anna anna;
    Inventory inv;
    public ItemInventory keys;
    bool unlocked;
    public AudioClip Sustain;
    public Material newSkybox;
    public Material nightMaterial;
    public List<MeshRenderer> objectsToChangeMaterial;
    public List<GameObject> objectsToDisable;
    public GameObject corpses;
    public BlackScreen black;

    void Start()
    {
        AnyDoor.WalkThrough += PassingDoor;
        DialogueInteraction.Conversation += EventConversation;
        inv = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();
    }

    void Update()
    {
        if (inv.inventory.Count > 0 && !unlocked)
        {
            for (int i = 0; i < inv.inventory.Count; i++)
            {
                if (inv.inventory[i].item.name == "Klucz")
                {
                    GetComponent<AnyDoor>().UnlockDoor();
                    unlocked = true;
                }
            }
        }
    }

    void OnDisable()
    {
        DialogueInteraction.Conversation -= EventConversation;
        AnyDoor.WalkThrough += PassingDoor;
    }

    void EventConversation(bool b, string n, int i)
    {
        if (b == false && n == "Vlad" && i == 0)
        {
            var t = GameObject.FindGameObjectWithTag("TaskSystem").GetComponent<TaskSystem>();
            Vlad.GetComponent<LoadDialogue>().currentDialogueID = 1;
            t.hideTask("3");
            t.addTask(t.availableTasks[4]);
        }
    }

    void PassingDoor(bool b, int n)
    {
        if (b && n == 10)
        {
            var p = GameObject.FindGameObjectWithTag("Player");
            p.transform.localEulerAngles = new Vector3(0, 202, 0);
            var a = p.GetComponent<AudioSource>();
            a.Stop();
            a.clip = Sustain;
            a.volume = 0.6f;
            a.Play();
            a.time = 46f;

            foreach (MeshRenderer m in objectsToChangeMaterial) m.material = nightMaterial;
            foreach (GameObject g in objectsToDisable) g.SetActive(false);
            RenderSettings.skybox = newSkybox;
            corpses.SetActive(true);
            var cars = GameObject.FindGameObjectsWithTag("Car");
            foreach (GameObject c in cars) c.SetActive(false);

            GameObject.FindGameObjectWithTag("GameController").GetComponent<Prototype>().LevelStarted(true);
            GameObject.FindGameObjectWithTag("GameController").GetComponent<Prototype>().UnfreezeLooking(true);
            anna.transform.localPosition = new Vector3(18, 9.66f, 49);
            anna.GetComponent<Anna>().agent.SetDestination(anna.GetComponent<Anna>().destinations[2].position);
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerActions>().inventoryAllowed = false;
            StartCoroutine(Fade());
        }

        IEnumerator Fade()
        {
            yield return new WaitForSeconds(15);
            black.enabled = true;
            black.GetComponent<Animator>().runtimeAnimatorController = black.Fade;
            yield return new WaitForSeconds(5);
            Application.Quit();
        }
    }
}
