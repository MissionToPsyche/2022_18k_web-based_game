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
    RocketPart rocketPartScript;
    private bool _applyDrag = false;
    private string _tooltipHeader;
    private string _tooltipBody;
    private RocketInformation _rocketInstance;
    private int _remainingParts = 0;
    public bool firstRocketPartPlaced = false;
    public UIManager uIManager;
    void Start()
    {
        _rocketInstance = RocketInformation.instance;

        // test
        _rocketInstance.tier3Capsule = 1;
        _rocketInstance.tier2Capsule = 1;
        _rocketInstance.tier1Capsule = 1;

        _rocketInstance.mediumFuelTank = 2;
        _rocketInstance.largeFuelTank = 3;
        _rocketInstance.smallFuelTank = 3;
        _rocketInstance.tier1NoseCone = 1;
        _rocketInstance.tier2NoseCone = 1;
        _rocketInstance.tier1Engine = 2;
        _rocketInstance.tier2Engine = 1;
        _rocketInstance.tier3Engine = 1;

        _rocketInstance.separator = 6;
        _rocketInstance.sideSeparator = 2;

        // capsules
        _tooltipHeader = "Capsule (tier 1)";
        _tooltipBody = "Durable, modern, and lightweight capsule containing a spacecraft.\nMass: 2t";

        _remainingParts = _rocketInstance.tier1Capsule;
        _tooltipBody = _tooltipBody + "\nRemaining: " + _remainingParts;
        createRocketPart(tier1Capsule, _rocketInstance.tier1Capsule);

        _tooltipHeader = "Capsule (tier 2)";
        _tooltipBody = "Although large, the interior makes up for the mass. A capsule containing a spacecraft.\nMass: 4t";
        _remainingParts = _rocketInstance.tier2Capsule;
        _tooltipBody = _tooltipBody + "\nRemaining: " + _remainingParts;
        createRocketPart(tier2Capsule, _rocketInstance.tier2Capsule);

        _tooltipHeader = "Capsule (tier 3)";
        _tooltipBody = "An old and chunky capsule containing a spacecraft.\nMass: 6t";
        _remainingParts = _rocketInstance.tier3Capsule;
        _tooltipBody = _tooltipBody + "\nRemaining: " + _remainingParts;
        createRocketPart(tier3Capsule, _rocketInstance.tier3Capsule);

        // nose cones
        _tooltipHeader = "Aerodynamic Nose Cone (tier 1)";
        _tooltipBody = "An aerodynamic nose cone used to improve the aerodynamics of the side boosters.\nMass: 0.5t";
        _remainingParts = _rocketInstance.tier1NoseCone;
        _tooltipBody = _tooltipBody + "\nRemaining: " + _remainingParts;
        createRocketPart(tier1NoseCone, _rocketInstance.tier1NoseCone);

        _tooltipHeader = "Aerodynamic Nose Cone (tier 2)";
        _tooltipBody = "An aerodynamic nose cone used to improve the aerodynamics of the side boosters.\nMass: 2.5t";
        _remainingParts = _rocketInstance.tier2NoseCone;
        _tooltipBody = _tooltipBody + "\nRemaining: " + _remainingParts;
        createRocketPart(tier2NoseCone, _rocketInstance.tier2NoseCone);

        // engines
        _tooltipHeader = "Engine (tier 1)";
        _tooltipBody = "High efficiency and high thrust engine used to make the rocket fly.\nMass: 2t\nThrust: 95t \nFuel Consumption: 0.5t/sec";
        _remainingParts = _rocketInstance.tier1Engine;
        _tooltipBody = _tooltipBody + "\nRemaining: " + _remainingParts;
        createRocketPart(tier1Engine, _rocketInstance.tier1Engine);

        _tooltipHeader = "Engine (tier 2)";
        _tooltipBody = "High efficiency but low thrust engine used to make the rocket fly.\nMass: 3t\nThrust: 65t \nFuel Consumption: 0.6t/sec";
        _remainingParts = _rocketInstance.tier2Engine;
        _tooltipBody = _tooltipBody + "\nRemaining: " + _remainingParts;
        createRocketPart(tier2Engine, _rocketInstance.tier2Engine);

        _tooltipHeader = "Engine (tier 3)";
        _tooltipBody = "Low efficiency and low thrust engine used to make the rocket fly.\nMass: 5t\nThrust: 40t \nFuel Consumption: 0.7t/sec";
        _remainingParts = _rocketInstance.tier3Engine;
        _tooltipBody = _tooltipBody + "\nRemaining: " + _remainingParts;
        createRocketPart(tier3Engine, _rocketInstance.tier3Engine);

        // separators
        _tooltipHeader = "Separator";
        _tooltipBody = "A vertical separator used for detaching empty stages.\nMass: 0.4t";
        _remainingParts = _rocketInstance.separator;
        _tooltipBody = _tooltipBody + "\nRemaining: " + _remainingParts;
        createRocketPart(separator, _rocketInstance.separator);

        _tooltipHeader = "Side separator";
        _tooltipBody = "A horizontal separator used for detaching side boosters\nMass: 0.2t";
        _remainingParts = _rocketInstance.sideSeparator;
        _tooltipBody = _tooltipBody + "\nRemaining: " + _remainingParts;
        createRocketPart(sideSeparator, _rocketInstance.sideSeparator);

        // fuel tanks
        _tooltipHeader = "Fuel tank";
        _tooltipBody = "A fuel tank carrying liquid fuel and liquid oxygen\nMass: 5t\nFuel: 4.5t";
        _remainingParts = _rocketInstance.smallFuelTank;
        _tooltipBody = _tooltipBody + "\nRemaining: " + _remainingParts;
        createRocketPart(smallFuelTank, _rocketInstance.smallFuelTank, 0.5f, 0.25f);

        _tooltipHeader = "Fuel tank";
        _tooltipBody = "A fuel tank carrying liquid fuel and liquid oxygen\nMass: 10t\nFuel: 9t";
        _remainingParts = _rocketInstance.mediumFuelTank;
        _tooltipBody = _tooltipBody + "\nRemaining: " + _remainingParts;
        createRocketPart(mediumFuelTank, _rocketInstance.mediumFuelTank, y: 0.5f);

        _tooltipHeader = "Fuel tank";
        _tooltipBody = "A fuel tank carrying liquid fuel and liquid oxygen\nMass: 20t\nFuel: 18t";
        _remainingParts = _rocketInstance.largeFuelTank;
        _tooltipBody = _tooltipBody + "\nRemaining: " + _remainingParts;
        createRocketPart(largeFuelTank, _rocketInstance.largeFuelTank);
    }
    void Update()
    {
        if (_applyDrag)
        {
            rocketPartDraggableScript.OnMouseDrag();
        }
    }
    private void instantiateRocketPart(rocketPart rocketPart)
    {
        Vector3 mousePosition = Input.mousePosition;
        Vector3 btnLocation = Camera.main.ScreenToWorldPoint(mousePosition);
        Vector3 spawnPosition = new Vector3(btnLocation.x, btnLocation.y, 0);
        instantiatedRocketPart = Instantiate(rocketPart.prefab, spawnPosition, Quaternion.identity);

        // set the parent to the rocket gameobject
        instantiatedRocketPart.transform.SetParent(rocket);

        // add draggable callback
        rocketPartDraggableScript = instantiatedRocketPart.GetComponent<Draggable>();
        SnapManager.instance.AddDraggableObjCallback(rocketPartDraggableScript);

        // Get script and set its count
        rocketPartScript = instantiatedRocketPart.GetComponent<RocketPart>();
        rocketPartScript.count = rocketPart.count;

        // drag focus the instantiated rocket part
        _applyDrag = true;

        // reduce count on btn click
        rocketPartScript.count--;

        // simulate mouse click
        rocketPartDraggableScript.OnMouseDown();
    }
    private void createRocketPartBtn(rocketPart currentPart, string tooltipHeader, string tooltipBody, float x_scaleImgBy = 1, float y_scaleImgBy = 1)
    {
        // instantiate buttons 
        currentPart.btn = Instantiate(btnPrefab);
        currentPart.btn.transform.SetParent(content);
        GameObject btnImgObj = currentPart.btn.transform.GetChild(0).gameObject;
        RectTransform btnImgRectTransform = btnImgObj.GetComponent<RectTransform>();
        btnImgRectTransform.localScale = new Vector3(btnImgRectTransform.localScale.x * x_scaleImgBy, btnImgRectTransform.localScale.y * y_scaleImgBy, btnImgRectTransform.localScale.z);
        Image btnImg = btnImgObj.GetComponent<Image>();
        btnImg.sprite = currentPart.prefab.GetComponent<SpriteRenderer>().sprite;

        // add event triggers to the button
        EventTrigger trigger = currentPart.btn.gameObject.AddComponent<EventTrigger>();

        // on rocket part UI button pointer down, instantiate the rocket part and apply drag on it
        var pointerDown = new EventTrigger.Entry();
        pointerDown.eventID = EventTriggerType.PointerDown;
        pointerDown.callback.AddListener((e) => instantiateRocketPart(currentPart));
        trigger.triggers.Add(pointerDown);

        // on rocket part UI button pointer up, do stuff
        var pointerUp = new EventTrigger.Entry();
        pointerUp.eventID = EventTriggerType.PointerUp;
        pointerUp.callback.AddListener((e) => currentPartPlaced(currentPart));
        trigger.triggers.Add(pointerUp);

        // add tooltip triggering function to the button
        currentPart.btn.gameObject.AddComponent<TooltipTrigger>();
        TooltipTrigger btnToolTipTrigger = currentPart.btn.gameObject.GetComponent<TooltipTrigger>();
        btnToolTipTrigger.header = tooltipHeader;
        btnToolTipTrigger.body = tooltipBody;
    }
    private void currentPartPlaced(rocketPart currentPart)
    {
        // stop the object from following the mouse
        _applyDrag = false;

        // Place first rocket part even if it doesn snap
        if (!firstRocketPartPlaced)
        {
            Rocket rocketScript = rocket.GetComponent<Rocket>();
            SnapManager.instance.UpdateRocketProperties(rocketPartScript, rocketScript);
            firstRocketPartPlaced = true;
            rocketPartScript.isFirstRocketPart = true;
        }

        // trigger the drag ended callback on the referenced script which places the snaps the part to snap point 
        rocketPartDraggableScript.OnMouseUp();

        // Disable the rocket part button 
        if (rocketPartScript.count < 1)
        {
            currentPart.btn.gameObject.SetActive(false);
        }
    }
    private void createRocketPart(rocketPart rocketPart, int count, float x = 1, float y = 1)
    {
        if (count > 0)
        {
            rocketPart.count = count;
            createRocketPartBtn(rocketPart, tooltipHeader: _tooltipHeader, tooltipBody: _tooltipBody, x_scaleImgBy: x, y_scaleImgBy: y);
        }
    }
    public void DestroyMisplacedRocketPart()
    {
        rocketPartScript.count++;
        rocketPartScript.isPartOfTheRocket = false;
        Destroy(instantiatedRocketPart);
    }
}
