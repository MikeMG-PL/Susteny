using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;

public class ViewMode : MonoBehaviour
{
    public Transform inventoryViewTransform;
    public UIHints UIHints;
    public float rotationSpeed = 0.5f;
    public float focusSpeed = 30f;

    [HideInInspector] public GameObject viewedItem;
    [HideInInspector] public bool viewingItem;
    [HideInInspector] public bool viewingFromInventory;
    [HideInInspector] public bool interactingWithItem;
    [HideInInspector] public bool finishedGoingAndRotatingTowardsObject = true;

    Vector3 mousePos;
    FloatParameter focalLength;
    GameObject focusCamera;
    SC_FPSController fpsController;
    Transform cameraTransform;
    PlayerActions playerActions;
    bool disablingFocus;
    bool enablingFocus;
    bool startedInspecting;

    public static event Action<bool> ViewingItem;
    public static event Action<bool, GameObject> ViewingDetails;
    void V(bool arg1, GameObject arg2) {;} // Empty function - always listens to ViewingDetails event (no null exception)

    void Awake()
    {
        ViewingDetails += V;
        focusCamera = GetComponent<Player>().focusCamera;
        cameraTransform = GetComponent<Player>().camera.transform;
        focalLength = focusCamera.GetComponent<PostProcessVolume>().profile.GetSetting<DepthOfField>().focalLength;
        fpsController = GetComponent<SC_FPSController>();
        playerActions = GetComponent<PlayerActions>();
    }

    /// <summary>
    /// Włącza focus na dany obiekt. Domyślnie pozwala na obracanie przedmiotem przez gracza za pomocą myszki.
    /// </summary>
    /// <param name="item">Oglądany obiekt, podczas wyłączania może przyjąć null.</param>
    /// <param name="b">Włącza/wyłącza viewMode.</param>
    /// <param name="disableRotating">Jeżeli true, włącza sam focus na obiekt, bez możliwości obracania przedmiotem.</param>
    /// <param name="switchLockControlsAndCursorOnImmediately">Jeżeli true, podczas rozpoczęcia oglądania obiektu (b == true) wyłączy możliwość poruszania i włączy kursor, a po skończonym oglądaniu (b == false) na odwrót.</param>
    public void ToggleViewMode(GameObject item, bool b, bool disableRotating = false, bool switchLockControlsAndCursorOnImmediately = true)
    {
        ViewingItem.Invoke(b);
        ViewingDetails.Invoke(b, item);
        if (switchLockControlsAndCursorOnImmediately)
        {
            fpsController.LockControlsCursorOn(b);
            if (b && item.GetComponent<Hints>() != null)
            {
                UIHints.ShowCornerHints(item.GetComponent<Hints>().cornerHints);
            }
        }
        else fpsController.LockControlsCursorOff(true);

        viewingItem = b;
        interactingWithItem = disableRotating;
        disablingFocus = !b;
        enablingFocus = b;

        if (b)
        {
            viewedItem = item.gameObject;
            ChangeLayerToFocus();
        }
        else
        {
            UIHints.HideCornerHints();
            // Zawsze po skończeniu oglądania, będziemy próbować ustawić layer itemu z powrotem na domyślny
            if (viewedItem != null && viewedItem.GetComponent<Interactable>() != null) viewedItem.GetComponent<Interactable>().ViewingEnded();
            viewedItem = null;
        }
    }

    public void ChangeLayerToFocus()
    {
        viewedItem.layer = 9;
        foreach (Transform child in viewedItem.GetComponentsInChildren<Transform>())child.gameObject.layer = 9;
    }

    public void ViewItemFromInventory(GameObject item)
    {
        GameObject obj = CreateItemFromInventory(item);
        viewingFromInventory = true;
        GetComponent<PlayerActions>().ToggleInventoryUI(false);
        ToggleViewMode(obj, true);
    }

    GameObject CreateItemFromInventory(GameObject item)
    {
        GameObject obj = Instantiate(item);
        obj.transform.SetParent(inventoryViewTransform.parent);
        obj.transform.localPosition = inventoryViewTransform.localPosition;
        obj.transform.localScale = item.transform.localScale * 3500;
        obj.transform.localEulerAngles = item.transform.localEulerAngles;
        return obj;
    }

    public void StopViewingItemFromInventory()
    {
        viewingFromInventory = false;
        Destroy(viewedItem);
        ToggleViewMode(null, false);
    }

    void Update()
    {
        CursorEnableAfterOnPosOrRot();

        if (viewingItem && !interactingWithItem)
        {
            InspectItem();
        }

        else if (!viewingItem) startedInspecting = false;

        ManageFocus();
    }

    void CursorEnableAfterOnPosOrRot()
    {
        if (!finishedGoingAndRotatingTowardsObject && !fpsController.lookingAt && !fpsController.goingTo)
        {
            if (viewingItem && playerActions.showCursorOnPosition)
            {
                fpsController.ToggleCursor(true);
                if (viewedItem.GetComponent<Hints>() != null) UIHints.ShowCornerHints(viewedItem.GetComponent<Hints>().cornerHints);
            }
            finishedGoingAndRotatingTowardsObject = true;
        }
    }

    void InspectItem()
    {
        if (Input.GetMouseButtonDown(0))
        {
            mousePos = Input.mousePosition;
            startedInspecting = true;
        }

        if (Input.GetMouseButton(0) && startedInspecting)
        {
            var delta = Input.mousePosition - mousePos;
            mousePos = Input.mousePosition;
            viewedItem.transform.Rotate(cameraTransform.TransformDirection(Vector3.right), delta.y * rotationSpeed, Space.World);
            viewedItem.transform.Rotate(cameraTransform.TransformDirection(Vector3.up), -delta.x * rotationSpeed, Space.World);
        }
    }

    void ManageFocus()
    {
        if (disablingFocus)
        {
            if (focalLength.value <= 24f)
            {
                focalLength.value = 24f;
                disablingFocus = false;
            }

            else focalLength.value -= Time.deltaTime * focusSpeed;
        }

        if (enablingFocus)
        {
            if (focalLength.value >= 35)
            {
                focalLength.value = 35f;
                enablingFocus = false;
            }

            else focalLength.value += Time.deltaTime * focusSpeed;
        }
    }

    // disablingFocus = false - gdy nie oglądamy żadnego przedmiotu / gdy skończyliśmy oglądać przedmiot i focus już się wyłączył
    // gdy skończyliśmy oglądać przedmiot, ale zaczęliśmy szybko oglądać drugi
    public bool DisablingFocusEnded()
    {
        return !disablingFocus;
    }

    void OnDisable()
    {
        ViewingDetails -= V;    
    }
}
