using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;

public class ViewMode : MonoBehaviour
{
    public Transform inventoryViewTransform;
    public float rotationSpeed = 0.5f;
    public float focusSpeed = 30f;
    public int paintEffectSpeed = 1;

    [HideInInspector] public GameObject viewedItem;
    [HideInInspector] public bool viewingItem;
    [HideInInspector] public bool viewingFromInventory;
    [HideInInspector] public bool interactingWithItem;

    Vector3 mousePos;
    FloatParameter focalLength;
    GameObject focusCamera;
    PaintEffect postProccessCamPaintEffect;
    PaintEffect mainCamPaintEffect;
    SC_FPSController fpsController;
    Transform cameraTransform;
    bool increasingPaintEffect;
    bool decreasingPaintEffect;
    bool disablingFocus;
    bool enablingFocus;
    bool startedInspecting;
    int maxMainPaintIntensity;
    int maxPostPaintIntensity;

    public static event Action<bool> ViewingItem;
    public static event Action<bool, GameObject> ViewingDetails;
    void V(bool arg1, GameObject arg2) {;} // Empty function - always listens to ViewingDetails event (no null exception)

    void Awake()
    {
        ViewingDetails += V;
        focusCamera = GetComponent<Player>().focusCamera;
        cameraTransform = GetComponent<Player>().camera.transform;
        mainCamPaintEffect = cameraTransform.GetComponent<PaintEffect>();
        postProccessCamPaintEffect = GetComponent<Player>().postProccessCamera.GetComponent<PaintEffect>();
        focalLength = focusCamera.GetComponent<PostProcessVolume>().profile.GetSetting<DepthOfField>().focalLength;
        fpsController = GetComponent<SC_FPSController>();

        maxMainPaintIntensity = mainCamPaintEffect.intensity;
        maxPostPaintIntensity = postProccessCamPaintEffect.intensity;
    }

    /// <summary>
    /// Włącza focus na dany obiekt. Domyślnie pozwala na obracanie przedmiotem przez gracza za pomocą myszki.
    /// </summary>
    /// <param name="item">Oglądany obiekt, podczas wyłączania może przyjąć null.</param>
    /// <param name="b">Włącza/wyłącza viewMode.</param>
    /// <param name="disableRotating">Jeżeli true, włącza sam focus na obiekt, bez możliwości obracania przedmiotem.</param>
    /// <param name="switchLockControlsAndCursorOn">Jeżeli true, podczas rozpoczęcia oglądania obiektu (b == true) wyłączy możliwość poruszania i włączy kursor, a po skończonym oglądaniu (b == false) na odwrót.</param>
    public void ToggleViewMode(GameObject item, bool b, bool disableRotating = false, bool switchLockControlsAndCursorOn = true)
    {
        ViewingItem.Invoke(b);
        ViewingDetails.Invoke(b, item);
        Debug.Log("Called!");
        if (switchLockControlsAndCursorOn) fpsController.LockControlsCursorOn(b);
        else fpsController.LockControlsCursorOff(true);

        viewingItem = b;
        interactingWithItem = disableRotating;
        disablingFocus = !b;
        enablingFocus = b;
        decreasingPaintEffect = b;
        increasingPaintEffect = !b;
        temp = 0;

        if (b)
        {
            viewedItem = item.gameObject;
        }
        else
        {
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
        ManagePaintEffect();

        if (viewingItem && !interactingWithItem)
        {
            InspectItem();
        }

        else if (!viewingItem) startedInspecting = false;

        if (!decreasingPaintEffect) ManageFocus();
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

    float temp = 0;

    void ManagePaintEffect()
    {
        if (decreasingPaintEffect)
        {
            if (mainCamPaintEffect.intensity <= 1)
            {
                mainCamPaintEffect.intensity = 1;
                decreasingPaintEffect = false;
                ChangeLayerToFocus();
            }

            else
            {
                temp += paintEffectSpeed * Time.deltaTime;
                mainCamPaintEffect.intensity -= (int)temp;
            }
        }

        if (increasingPaintEffect)
        {
            if (mainCamPaintEffect.intensity >= maxMainPaintIntensity)
            {
                mainCamPaintEffect.intensity = maxMainPaintIntensity;
                increasingPaintEffect = false;
            }

            else
            {
                temp += paintEffectSpeed * Time.deltaTime;
                mainCamPaintEffect.intensity += (int)temp;
            }
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
