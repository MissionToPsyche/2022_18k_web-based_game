using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
[System.Serializable]
public class rocketPart
{
    public Button btnPrefab;
    public GameObject prefab;
    public int count = 0;
}
public class RocketPartsShopManager : MonoBehaviour
{
    public SnapManager snapManager;
    public Transform content;
    public Transform rocket;
    public rocketPart tier1Capsule;
    private Button _tier1CapsuleBtn;
    public rocketPart tier2Capsule;
    public rocketPart tier3Capsule;
    GameObject instantiatedRocketPart;
    Draggable rocketPartDraggableScript;
    private bool bruh = false;
    void Start()
    {
        RocketInformation rocketInstance = RocketInformation.instance;
        rocketInstance.setTier1CapsuleCount(2);
        tier1Capsule.count = rocketInstance.getTier1CapsuleCount();
        if (tier1Capsule.count > 0)
        {
            _tier1CapsuleBtn = Instantiate(tier1Capsule.btnPrefab);
            _tier1CapsuleBtn.transform.SetParent(content);
            EventTrigger trigger = _tier1CapsuleBtn.gameObject.AddComponent<EventTrigger>();
            var pointerDown = new EventTrigger.Entry();
            pointerDown.eventID = EventTriggerType.PointerDown;
            pointerDown.callback.AddListener((e) => instantiateRocketPart(tier1Capsule.prefab, ref tier1Capsule.count));
            trigger.triggers.Add(pointerDown);

            var pointerUp = new EventTrigger.Entry();
            pointerUp.eventID = EventTriggerType.PointerUp;
            pointerUp.callback.AddListener((e) => rocketPartPlaced());
            trigger.triggers.Add(pointerUp);
        }
    }
    void Update()
    {

        if (bruh)
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
        snapManager.addDraggableObjCallback(rocketPartDraggableScript);

        // drag focus the rocket part
        bruh = true;

        // reduce count on btn click
        count--;
    }
    private void rocketPartPlaced()
    {
        bruh = false;
        if (tier1Capsule.count < 1)
        {
            _tier1CapsuleBtn.gameObject.SetActive(false);
        }
    }
}
