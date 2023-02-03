using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnapManager : MonoBehaviour
{
    private List<GameObject> _snappingPointObjs = new List<GameObject>();
    public Transform rocket;
    private float _snapRange = 0.5f;
    private Draggable _currentDraggedObj;
    private Transform _closestSnapPoint = null;
    public static SnapManager instance;     // Needs to create an instance because all the instantiated rocket parts use this script
    public RocketPartsShopManager rocketPartsShopManager;
    void Awake()
    {
        // if there is already an instance, destroy it
        if (instance != null)
        {
            Destroy(gameObject);
        }
        // else, keep instance of this object
        else
        {
            instance = this;
        }
    }
    void Start()
    {
        // foreach (Draggable draggableObj in draggableObjs)
        // {
        //     draggableObj.dragEndedCallback = OnDragEnded;
        // }
    }

    private void OnDragEnded(Draggable draggedObj)
    {
        _currentDraggedObj = draggedObj;
        float closestDistance = 100000f;

        foreach (GameObject snapPoint in _snappingPointObjs)
        {
            // should not snap to currently dragged object's own snapping points
            SnappingPoint snappingPointScript = snapPoint.GetComponent<SnappingPoint>();
            if (!snappingPointScript.isAttached && snapPoint.transform.parent.gameObject != _currentDraggedObj.gameObject && snapPoint.transform.parent.gameObject.GetComponent<RocketPart>().isPartOfTheRocket)
            {
                RocketPart currentRocketPartScript = _currentDraggedObj.GetComponent<RocketPart>();

                // Get transform positions of the snapping points and calculate the distance of each snapping points
                string currentRocketPartClosestSnappingPointDirection = "";
                void calculateClosestDistance(Vector2 snappingPointPosition, string direction)
                {
                    float currentDistance = Vector2.Distance(snappingPointPosition, snapPoint.transform.position);
                    if (_closestSnapPoint == null || currentDistance < closestDistance)
                    {
                        _closestSnapPoint = snapPoint.transform;
                        closestDistance = currentDistance;
                        currentRocketPartClosestSnappingPointDirection = direction;
                    }
                }
                if (currentRocketPartScript.snappingPointOnBottom)
                {
                    Vector2 snappingPointPosition = currentRocketPartScript.snappingPointOnBottom.transform.position;
                    calculateClosestDistance(snappingPointPosition, "bottom");
                }
                if (currentRocketPartScript.snappingPointOnTop)
                {
                    Vector2 snappingPointPosition = currentRocketPartScript.snappingPointOnTop.transform.position;
                    calculateClosestDistance(snappingPointPosition, "top");
                }
                if (currentRocketPartScript.snappingPointOnLeft)
                {
                    Vector2 snappingPointPosition = currentRocketPartScript.snappingPointOnLeft.transform.position;
                    calculateClosestDistance(snappingPointPosition, "left");
                }
                if (currentRocketPartScript.snappingPointOnRight)
                {
                    Vector2 snappingPointPosition = currentRocketPartScript.snappingPointOnRight.transform.position;
                    calculateClosestDistance(snappingPointPosition, "right");
                }

                if (_closestSnapPoint != null && closestDistance <= _snapRange)
                {
                    // reset parent to rocket (or the local position translation won't be correct)
                    _currentDraggedObj.transform.SetParent(rocket.transform);

                    float snapPosition;
                    SpriteRenderer closestSnapObjSpriteRenderer = _closestSnapPoint.transform.parent.GetComponent<SpriteRenderer>();
                    SnappingPoint closestSnapPointScript = _closestSnapPoint.GetComponent<SnappingPoint>();

                    // snap to top
                    if (closestSnapPointScript.direction == "top" && currentRocketPartClosestSnappingPointDirection == "bottom")
                    {
                        snapPosition = _closestSnapPoint.transform.parent.localPosition.y + closestSnapObjSpriteRenderer.bounds.size.y / 2 + _currentDraggedObj.spriteRenderer.bounds.size.y / 2;
                        SnapToPoint(snapPosition, "top");
                    }
                    // snap to bottom
                    else if (closestSnapPointScript.direction == "bottom" && currentRocketPartClosestSnappingPointDirection == "top")
                    {
                        snapPosition = _closestSnapPoint.transform.parent.localPosition.y - closestSnapObjSpriteRenderer.bounds.size.y / 2 - _currentDraggedObj.spriteRenderer.bounds.size.y / 2;
                        SnapToPoint(snapPosition, "bottom");
                    }
                    // If it is horizontal snapping, at least one of the rocket part should be a side separator and both cannot be side separators
                    else if ((_currentDraggedObj.gameObject.tag == "SideSeparator" || _closestSnapPoint.transform.parent.gameObject.tag == "SideSeparator") && !(_currentDraggedObj.gameObject.tag == "SideSeparator" && _closestSnapPoint.transform.parent.gameObject.tag == "SideSeparator"))
                    {
                        // snap to left
                        if (closestSnapPointScript.direction == "left" && currentRocketPartClosestSnappingPointDirection == "right")
                        {
                            snapPosition = _closestSnapPoint.transform.parent.localPosition.x - closestSnapObjSpriteRenderer.bounds.size.x / 2 - _currentDraggedObj.spriteRenderer.bounds.size.x / 2;
                            SnapToPoint(snapPosition, "left");
                        }
                        // snap to right
                        else if (closestSnapPointScript.direction == "right" && currentRocketPartClosestSnappingPointDirection == "left")
                        {
                            snapPosition = _closestSnapPoint.transform.parent.localPosition.x + closestSnapObjSpriteRenderer.bounds.size.x / 2 + _currentDraggedObj.spriteRenderer.bounds.size.x / 2;
                            SnapToPoint(snapPosition, "right");
                        }
                        else
                        {
                            // Debug.Log("When horizontally snapping, one rocket part must be a side separator");
                        }
                    }
                    else
                    {
                        // Debug.Log("Snap point invalid, make sure that the snapping points are in opposite direction and have correct directions set");
                    }
                }
                else
                {
                    _currentDraggedObj.GetComponent<RocketPart>().isPartOfTheRocket = false;
                }
            }
        }
    }
    void SnapToPoint(float snapPosition, string direction)
    {
        // Give reference to the dragged rocket part which rocket part it was snapped to and vice-versa
        GameObject snappedRocketPart = _closestSnapPoint.transform.parent.gameObject;
        RocketPart snappedRocketPartScript = snappedRocketPart.GetComponent<RocketPart>();
        RocketPart currentRocketPartScript = _currentDraggedObj.GetComponent<RocketPart>();
        if (direction == "top")
        {
            if (snappedRocketPartScript.isPartOfLeftBooster)
            {
                currentRocketPartScript.isPartOfLeftBooster = true;
            }
            else if (snappedRocketPartScript.isPartOfRightBooster)
            {
                currentRocketPartScript.isPartOfRightBooster = true;
            }
            // Snap the current dragged object to the snapping point
            _currentDraggedObj.transform.localPosition = new Vector2(_closestSnapPoint.transform.parent.localPosition.x, snapPosition);

            currentRocketPartScript.rocketPartOnBottom = snappedRocketPart;
            if (currentRocketPartScript.snappingPointOnBottom)
            {
                currentRocketPartScript.snappingPointOnBottom.GetComponent<SnappingPoint>().isAttached = true;
            }
            snappedRocketPartScript.rocketPartOnTop = _currentDraggedObj.gameObject;
        }
        else if (direction == "bottom")
        {
            if (snappedRocketPartScript.isPartOfLeftBooster)
            {
                currentRocketPartScript.isPartOfLeftBooster = true;
            }
            else if (snappedRocketPartScript.isPartOfRightBooster)
            {
                currentRocketPartScript.isPartOfRightBooster = true;
            }
            // Snap the current dragged object to the snapping point
            _currentDraggedObj.transform.localPosition = new Vector2(_closestSnapPoint.transform.parent.localPosition.x, snapPosition);

            currentRocketPartScript.rocketPartOnTop = snappedRocketPart;
            if (currentRocketPartScript.snappingPointOnTop)
            {
                currentRocketPartScript.snappingPointOnTop.GetComponent<SnappingPoint>().isAttached = true;
            }
            snappedRocketPartScript.rocketPartOnBottom = _currentDraggedObj.gameObject;
        }
        else if (direction == "left")
        {
            currentRocketPartScript.isPartOfLeftBooster = true;

            // Snap the current dragged object to the snapping point
            _currentDraggedObj.transform.localPosition = new Vector2(snapPosition, _closestSnapPoint.transform.parent.localPosition.y);

            currentRocketPartScript.rocketPartOnRight = snappedRocketPart;
            if (currentRocketPartScript.snappingPointOnRight)
            {
                currentRocketPartScript.snappingPointOnRight.GetComponent<SnappingPoint>().isAttached = true;
            }
            snappedRocketPartScript.rocketPartOnLeft = _currentDraggedObj.gameObject;
        }
        else if (direction == "right")
        {
            currentRocketPartScript.isPartOfRightBooster = true;

            // Snap the current dragged object to the snapping point
            _currentDraggedObj.transform.localPosition = new Vector2(snapPosition, _closestSnapPoint.transform.parent.localPosition.y);

            currentRocketPartScript.rocketPartOnLeft = snappedRocketPart;
            if (currentRocketPartScript.snappingPointOnLeft)
            {
                currentRocketPartScript.snappingPointOnLeft.GetComponent<SnappingPoint>().isAttached = true;
            }
            snappedRocketPartScript.rocketPartOnRight = _currentDraggedObj.gameObject;
        }

        // Disable dragging of snapping points that are connected to more than 1 other parts
        SnappingPoint closestSnapPointScript = _closestSnapPoint.GetComponent<SnappingPoint>();
        closestSnapPointScript.isAttached = true;

        // Assign parent after delay
        Invoke("AssignParent", 0.01f);
    }
    void AssignParent()
    {
        ObjectSnappedInPlace();
    }
    void ObjectSnappedInPlace()
    {
        RocketPart rocketPartScript = _currentDraggedObj.GetComponent<RocketPart>();
        GameObject connectedRocketPart = _closestSnapPoint.transform.parent.gameObject;
        RocketPart connectedRocketPartScript = connectedRocketPart.GetComponent<RocketPart>();

        rocketPartScript.isPartOfTheRocket = true;

        if (connectedRocketPart.tag == "Separator")
        {
            rocketPartScript.attachedSeparator = connectedRocketPart;
        }
        else if (connectedRocketPart.tag == "SideSeparator")
        {
            rocketPartScript.attachedSideSeparator = connectedRocketPart;
        }
        else if (connectedRocketPartScript.attachedSeparator)
        {
            rocketPartScript.attachedSeparator = connectedRocketPartScript.attachedSeparator;
        }
        else if (connectedRocketPartScript.attachedSideSeparator)
        {
            rocketPartScript.attachedSideSeparator = connectedRocketPartScript.attachedSideSeparator;
        }
        _closestSnapPoint = null;
    }
    public void AddDraggableObjCallback(Draggable draggableObj)
    {
        draggableObj.dragEndedCallback = OnDragEnded;
    }
    public void AddSnappingPointObj(GameObject snappingPointObj)
    {
        _snappingPointObjs.Add(snappingPointObj);
    }
    public void ToggleSnappingPoints(GameObject currentDraggedObj)
    {
        // If side separator show all snapping points available on the left or right
        foreach (GameObject snappingPointObj in _snappingPointObjs)
        {
            SnappingPoint snappingPointScript = snappingPointObj.GetComponent<SnappingPoint>();
            if (!snappingPointScript.isAttached)
            {
                string snappingPointDirection = snappingPointScript.direction;
                if (currentDraggedObj.tag == "SideSeparator")
                {
                    if (snappingPointDirection == "left")
                    {
                        toggleSnappingPoint(snappingPointObj);
                    }
                    else if (snappingPointDirection == "right")
                    {
                        toggleSnappingPoint(snappingPointObj);
                    }
                }
                else
                {
                    // Fuel tanks should see horizontal snapping points if side separator is present
                    if (currentDraggedObj.gameObject.tag == "FuelTank")
                    {
                        if (snappingPointObj.transform.parent.gameObject.tag == "SideSeparator")
                        {
                            if (snappingPointDirection == "left")
                            {
                                toggleSnappingPoint(snappingPointObj);
                            }
                            else if (snappingPointDirection == "right")
                            {
                                toggleSnappingPoint(snappingPointObj);
                            }
                        }
                    }
                    if (snappingPointDirection == "top")
                    {
                        toggleSnappingPoint(snappingPointObj);
                    }
                    else if (snappingPointDirection == "bottom")
                    {
                        toggleSnappingPoint(snappingPointObj);
                    }
                }
            }
        }
    }
    void toggleSnappingPoint(GameObject snappingPointObj)
    {
        if (snappingPointObj.activeSelf)
        {
            snappingPointObj.SetActive(false);
        }
        else
        {
            snappingPointObj.SetActive(true);
        }
    }
}
