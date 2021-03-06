﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
    /// ENUMS ///
    public enum SceneState { AllInOne, Unload, AdditiveLoad, SingleLoad };
    public enum NameOrID { Name, ID };
    public enum RotationType { Default, Custom };

    /// CUSTOM INSPECTOR PART ///
    // Door settings
    public int ID;
    public bool unlocked;
    public bool usingDefaultRotation;
    public Transform defaultDestination;
    public bool alwaysReturnToLastPosition;
    public Vector3 teleportCameraRotation;

    // Scene settings
    public SceneState doorMode;
    public NameOrID nameOrID;
    public RotationType rotType;
    public string sceneName; public int sceneID;

    /// SCRIPT PROPERTIES ///
    CharacterController character;
    GameObject blackPanel;
    ViewMode viewMode;
    ManipulatePlayer manipulation;

    /// EVENTS ///
    public static event Action<int> WalkThrough;

    /// FUNCTIONS ///
    void Awake()
    {
        character = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterController>();
        viewMode = GameObject.FindGameObjectWithTag("Player").GetComponent<ViewMode>();
        blackPanel = GameObject.FindGameObjectWithTag("BlackPanel");
        manipulation = GetComponent<ManipulatePlayer>();
    }

    // TODO: nie przypisywać ciągle zmiennych w Update, przypisać raz, a potem czekać na kolejną zmianę
    // Unlocked/locked - visuals
    void Update()
    {
        if (unlocked)
        {
            GetComponent<Interactable>().crosshairInteractivity = CrosshairInteractivity.interactive;
            GetComponent<Hints>().nearCrosshairHint = "Otwórz [LPM]";
            GetComponent<ManipulatePlayer>().enableGoTo = true;
            GetComponent<ManipulatePlayer>().enableLookAt = true;
        }
        else
        {
            GetComponent<Interactable>().crosshairInteractivity = CrosshairInteractivity.interactive;
            GetComponent<Hints>().nearCrosshairHint = "Zamknięte";
            GetComponent<ManipulatePlayer>().enableGoTo = false;
            GetComponent<ManipulatePlayer>().enableLookAt = false;
        }
    }

    public void OpenDoor(bool b)
    {
        unlocked = b;
    }

    public void Interact()
    {
        if (unlocked)
            FadeIn();
    }

    Animator a; BlackScreen b;
    void FadeIn()
    {
        a = blackPanel.GetComponent<Animator>();
        b = blackPanel.GetComponent<BlackScreen>();
        b.Animating(true);
        a.runtimeAnimatorController = b.Fade;
        a.Play(0);
        character.GetComponent<PlayerActions>().inventoryAllowed = false;
        character.GetComponent<PlayerActions>().LockInteracting(true);
        StartCoroutine(Entering());
    }

    IEnumerator Entering()
    {
        yield return new WaitForSeconds(2f);
        Teleport();
    }

    void Teleport()
    {
        viewMode.ToggleViewMode(null, false);
        manipulation.StopManipulating();
        character.enabled = false;
        WalkThrough?.Invoke(ID);

        switch (doorMode)
        {
            case SceneState.AllInOne:
                Warp();
                a.runtimeAnimatorController = b.Unfade;
                break;

            case SceneState.Unload:
                Warp();
                a.runtimeAnimatorController = b.Unfade;

                if (nameOrID == NameOrID.ID)
                    SceneManager.UnloadSceneAsync(sceneID);
                else
                    SceneManager.UnloadSceneAsync(sceneName);
                break;

            case SceneState.SingleLoad:
                if (nameOrID == NameOrID.ID)
                    SceneManager.LoadSceneAsync(sceneID, LoadSceneMode.Single);
                else
                    SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);

                StartCoroutine(CheckSceneLoad(nameOrID));
                break;

            case SceneState.AdditiveLoad:
                if (nameOrID == NameOrID.ID)
                    SceneManager.LoadSceneAsync(sceneID, LoadSceneMode.Additive);
                else
                    SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

                StartCoroutine(CheckSceneLoad(nameOrID));
                break;

        }
        character.GetComponent<PlayerActions>().inventoryAllowed = true;
        character.GetComponent<PlayerActions>().LockInteracting(false);
        
    }

    IEnumerator CheckSceneLoad(NameOrID nameOrID)
    {
        if (nameOrID == NameOrID.ID)
        {
            while (!SceneManager.GetSceneByBuildIndex(sceneID).isLoaded)
            {
                a.runtimeAnimatorController = a.runtimeAnimatorController;
                yield return new WaitForEndOfFrame();
                a.runtimeAnimatorController = a.runtimeAnimatorController;
            }
            SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(sceneID));
        }
        else
        {
            while (!SceneManager.GetSceneByName(sceneName).isLoaded)
            {
                a.runtimeAnimatorController = a.runtimeAnimatorController;
                yield return new WaitForEndOfFrame();
                a.runtimeAnimatorController = a.runtimeAnimatorController;
            }
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));
        }

        Warp();
        yield return new WaitForEndOfFrame();
        LightProbes.TetrahedralizeAsync();
        a.runtimeAnimatorController = b.Unfade;
        StopCoroutine(CheckSceneLoad(nameOrID));
    }

    void Warp()
    {
        character.transform.position = defaultDestination.position;

        //if (usingDefaultRotation)
          //  GameObject.FindGameObjectWithTag("MainCamera").transform.localEulerAngles = defaultDestination.localEulerAngles;
       // else
        //    GameObject.FindGameObjectWithTag("MainCamera").transform.localEulerAngles = teleportCameraRotation;
        // MODIFYING CAMERA ROTATION BREAKS INTERACTION WITH OBJECTS IN BUILD

        character.enabled = true;
    }
}
