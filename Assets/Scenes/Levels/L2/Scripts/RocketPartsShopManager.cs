using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
[System.Serializable]
public class rocketPart
{
    [System.NonSerialized]
    public Button btn;
    [System.NonSerialized]

    public int count = 0;
    public GameObject prefab;

}
public class RocketPartsShopManager : MonoBehaviour
{
    public SnapManager snapManager;
    public Transform content;
    public Transform rocket;
    public Button btnPrefab;

    public rocketPart tier1Capsule;
    public rocketPart tier2Capsule;
    public rocketPart tier3Capsule;
    public rocketPart tier1NoseCone;
    public rocketPart tier2NoseCone;
    public rocketPart tier1Engine;
    public rocketPart tier2Engine;
    public rocketPart tier3Engine;
    public rocketPart separator;
    public rocketPart sideSeparator;
    public rocketPart smallFuelTank;
    public rocketPart mediumFuelTank;
    public rocketPart largeFuelTank;
    GameObject instantiatedRocketPart;
    Draggable rocketPartDraggableScript;
    private bool _applyDrag = false;
    RocketInformation rocketInstance;
    void Start()
    {
        rocketInstance = RocketInformation.instance;

        // test
        rocketInstance.tier1Capsule = 2;
        rocketInstance.mediumFuelTank = 2;
        rocketInstance.largeFuelTank = 2;
        rocketInstance.tier1NoseCone = 2;
        rocketInstance.tier2Engine = 3;

        // capsules
        tier1Capsule.count = rocketInstance.tier1Capsule;
        if (tier1Capsule.count > 0)
        {
            createRocketPartBtn(tier1Capsule);
        }

        tier2Capsule.count = rocketInstance.tier2Capsule;
        if (tier2Capsule.count > 0)
        {
            createRocketPartBtn(tier2Capsule);
        }

        tier3Capsule.count = rocketInstance.tier3Capsule;
        if (tier3Capsule.count > 0)
        {
            createRocketPartBtn(tier3Capsule);
        }

        // nose cones
        tier1NoseCone.count = rocketInstance.tier1NoseCone;
        if (tier1NoseCone.count > 0)
        {
            createRocketPartBtn(tier1NoseCone);
        }

        tier2NoseCone.count = rocketInstance.tier2NoseCone;
        if (tier2NoseCone.count > 0)
        {
            createRocketPartBtn(tier2NoseCone);
        }

        // engines
        tier1Engine.count = rocketInstance.tier1Engine;
        if (tier1Engine.count > 0)
        {
            createRocketPartBtn(tier1Engine);
        }

        tier2Engine.count = rocketInstance.tier2Engine;
        if (tier2Engine.count > 0)
        {
            createRocketPartBtn(tier2Engine);
        }

        tier3Engine.count = rocketInstance.tier3Engine;
        if (tier3Engine.count > 0)
        {
            createRocketPartBtn(tier3Engine);
        }

        // separators
        separator.count = rocketInstance.separator;
        if (separator.count > 0)
        {
            createRocketPartBtn(separator);
        }

        sideSeparator.count = rocketInstance.sideSeparator;
        if (sideSeparator.count > 0)
        {
            createRocketPartBtn(sideSeparator);
        }
        // fuel tanks
        smallFuelTank.count = rocketInstance.smallFuelTank;
        if (smallFuelTank.count > 0)
        {
            // reduce height by 4 times that of large fuel tank
            createRocketPartBtn(smallFuelTank, 0.25f);
        }

        mediumFuelTank.count = rocketInstance.mediumFuelTank;
        if (mediumFuelTank.count > 0)
        {
            // reduce height by 2 times that of large fuel tank
            createRocketPartBtn(mediumFuelTank, 0.5f);
        }

        largeFuelTank.count = rocketInstance.largeFuelTank;
        if (largeFuelTank.count > 0)
        {
            createRocketPartBtn(largeFuelTank);
        }
    }
    void Update()
    {
        if (_applyDrag)
        {
            rocketPartDraggableScript.OnMouseDrag();
        }
    }
    private void instantiateRocketPart(GameObject rocketPartPrefab, ref int count)
    {
        Vector3 mousePosition = Input.mousePosition;
        Vector3 btnLocation = Camera.main.ScreenToWorldPoint(mousePosition);
        Vector3 spawnPosition = new Vector3(btnLocation.x, btnLocation.y, 0);
        instantiatedRocketPart = Instantiate(rocketPartPrefab, spawnPosition, Quaternion.identity);

        // set the parent to the rocket gameobject
        instantiatedRocketPart.transform.SetParent(rocket);

        // add draggable callback
        rocketPartDraggableScript = instantiatedRocketPart.GetComponent<Draggable>();
        snapManager.AddDraggableObjCallback(rocketPartDraggableScript);

        // drag focus the instantiated rocket part
        _applyDrag = true;

        // reduce count on btn click
        count--;

        // simulate mouse click
        rocketPartDraggableScript.OnMouseDown();
    }
    private void createRocketPartBtn(rocketPart currentPart, float scaleImgBy = 1)
    {
        // instantiate buttons 
        currentPart.btn = Instantiate(btnPrefab);
        currentPart.btn.transform.SetParent(content);
        GameObject btnImgObj = currentPart.btn.transform.GetChild(0).gameObject;
        RectTransform btnImgRectTransform = btnImgObj.GetComponent<RectTransform>();
        btnImgRectTransform.localScale = new Vector3(btnImgRectTransform.localScale.x, btnImgRectTransform.localScale.y * scaleImgBy, btnImgRectTransform.localScale.z);
        Image btnImg = btnImgObj.GetComponent<Image>();
        btnImg.sprite = currentPart.prefab.GetComponent<SpriteRenderer>().sprite;

        // add event triggers to the button
        EventTrigger trigger = currentPart.btn.gameObject.AddComponent<EventTrigger>();

        // on rocket part UI button pointer down, instantiate the rocket part and apply drag on it
        var pointerDown = new EventTrigger.Entry();
        pointerDown.eventID = EventTriggerType.PointerDown;
        pointerDown.callback.AddListener((e) => instantiateRocketPart(currentPart.prefab, ref currentPart.count));
        trigger.triggers.Add(pointerDown);

        // on rocket part UI button pointer up, do stuff
        var pointerUp = new EventTrigger.Entry();
        pointerUp.eventID = EventTriggerType.PointerUp;
        pointerUp.callback.AddListener((e) => currentPartPlaced(currentPart));
        trigger.triggers.Add(pointerUp);
    }
    private void currentPartPlaced(rocketPart currentPart)
    {
        // stop the object from following the mouse
        _applyDrag = false;

        // trigger the drag ended callback on the referenced script which places the snaps the part to snap point 
        rocketPartDraggableScript.OnMouseUp();
        if (currentPart.count < 1)
        {
            currentPart.btn.gameObject.SetActive(false);
        }
    }
}
