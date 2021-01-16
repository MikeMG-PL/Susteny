using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using UnityEngine.AI;

public class Prototype : MonoBehaviour
{
    public TaskSystem taskSystem;
    public PlayableDirector timeline5;
    public PlayableDirector timeline8;
    public PlayableDirector timeline12;
    public Item item;
    public AudioClip Sustain;
    public Material newSkybox;
    public Material nightMaterial;
    public List<MeshRenderer> objectsToChangeMaterial;
    public Anna anna;
    /////////////////////////////////////
    Item inventoryItem;
    AudioSource audioSource;
    BlackScreen black;

    /// SUBSCRIBING EVENTS ///
    void Awake()
    {
        ViewMode.ViewingDetails += OnViewModeSwitch;
        Door.WalkThrough += WalkThroughDoor;
    }

    /// UNSUBSCRIBING EVENTS ///
    void OnDisable()
    {
        ViewMode.ViewingDetails -= OnViewModeSwitch;
        Door.WalkThrough -= WalkThroughDoor;
    }

    /// FUNCTIONS ///
    public void AddTaskFromList(int i)
    {
        taskSystem.addTask(taskSystem.availableTasks[i]);
    }

    public void HideTask(int i)
    {
        taskSystem.hideTask($"{i}");
    }

    void OnViewModeSwitch(bool b, GameObject o)
    {
        if (b && o != null && o.GetComponent<ItemID>() != null)
            inventoryItem = o.GetComponent<ItemID>().thisItem;

        if (!b && inventoryItem == item)
        {
            timeline5.Play();
        }
    }

    void WalkThroughDoor(int ID)
    {
        if (ID == 0)
        {
            foreach (TaskScriptableObject so in taskSystem.tasks) taskSystem.hideTask(so.id.ToString());
            AddTaskFromList(3);
            timeline8.Play();
        }
        if(ID == 3)
        {
            foreach (TaskScriptableObject so in taskSystem.tasks) taskSystem.hideTask(so.id.ToString());
            timeline12.Play();
        }
    }

    public void FadeAmble()
    {
        audioSource = GameObject.FindGameObjectWithTag("Player").GetComponent<AudioSource>();
        StartCoroutine(Fade());
    }

    IEnumerator Fade()
    {
        var buffer = audioSource.volume;
        while (audioSource.volume > 0)
        {
            audioSource.volume -= 0.00175f;
            yield return new WaitForSeconds(Time.deltaTime);
        }
        audioSource.clip = null;
        audioSource.volume = buffer + 0.075f;
        StopCoroutine(Fade());
    }

    public void Balcony()
    {
        GameObject.FindGameObjectWithTag("MainCamera").transform.parent.transform.localEulerAngles = new Vector3(0, 202, 0);
        audioSource.Stop();
        audioSource.clip = Sustain;
        audioSource.volume = 0.6f;
        audioSource.Play();
        audioSource.time = 46f;

        foreach (MeshRenderer m in objectsToChangeMaterial) m.material = nightMaterial;

        var cars = GameObject.FindGameObjectsWithTag("Car");
        foreach (GameObject c in cars) c.SetActive(false);

        anna.transform.position = new Vector3(55, 9.66f, 100);
        anna.GetComponent<Anna>().agent.SetDestination(anna.GetComponent<Anna>().destinations[2].position);
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerActions>().inventoryAllowed = false;
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("Prototype"));
        RenderSettings.skybox = newSkybox;
        StartCoroutine(EndFade());
    }

    IEnumerator EndFade()
    {
        black = GameObject.FindGameObjectWithTag("BlackPanel").GetComponent<BlackScreen>();
        yield return new WaitForSeconds(15);
        black.GetComponent<Animator>().runtimeAnimatorController = black.Fade;
        black.Animating(true);
        yield return new WaitForSeconds(5);
        Application.Quit();
    }
}
