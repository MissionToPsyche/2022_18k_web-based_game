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
            if (!snappingPointScript.isAttached && snappingPointScript.isEnabled && snapPoint.transform.parent.gameObject != _currentDraggedObj.gameObject)
            {
                float currentDistance = Vector2.Distance(_currentDraggedObj.transform.position, snapPoint.transform.position);
                Debug.Log(currentDistance, snapPoint);

                if (_closestSnapPoint == null || currentDistance < closestDistance)
                {
                    _closestSnapPoint = snapPoint.transform;
                    closestDistance = currentDistance;
                }

                if (_closestSnapPoint != null && closestDistance <= _snapRange)
                {
                    // reset parent to rocket (or the local position translation won't be correct)
                    _currentDraggedObj.transform.SetParent(rocket.transform);

                    float snapPosition;
                    SpriteRenderer closestSnapObjSpriteRenderer = _closestSnapPoint.transform.parent.GetComponent<SpriteRenderer>();
                    SnappingPoint closestSnapPointScript = _closestSnapPoint.GetComponent<SnappingPoint>();
                    // snap to top
                    if (closestSnapPointScript.direction == "top")
                    {
                        snapPosition = _closestSnapPoint.transform.parent.localPosition.y + closestSnapObjSpriteRenderer.bounds.size.y / 2 + _currentDraggedObj.spriteRenderer.bounds.size.y / 2;
                        SnapToPoint(snapPosition, "top");
                    }

                    // snap to bottom
                    else if (closestSnapPointScript.direction == "bottom")
                    {
                        snapPosition = _closestSnapPoint.transform.parent.localPosition.y - closestSnapObjSpriteRenderer.bounds.size.y / 2 - _currentDraggedObj.spriteRenderer.bounds.size.y / 2;
                        SnapToPoint(snapPosition, "bottom");
                    }
                    // snap to left
                    else if (closestSnapPointScript.direction == "left")
                    {
                        snapPosition = _closestSnapPoint.transform.parent.localPosition.x - closestSnapObjSpriteRenderer.bounds.size.x / 2 - _currentDraggedObj.spriteRenderer.bounds.size.x / 2;
                        SnapToPoint(snapPosition, "left");
                    }
                    // snap to right
                    else if (closestSnapPointScript.direction == "right")
                    {
                        snapPosition = _closestSnapPoint.transform.parent.localPosition.x + closestSnapObjSpriteRenderer.bounds.size.x / 2 + _currentDraggedObj.spriteRenderer.bounds.size.x / 2;
                        SnapToPoint(snapPosition, "right");
                    }
                    else
                    {
                        Debug.Log("Snap point invalid, check the direction of the snap point.");
                    }

                }
            }
        }
    }
    void SnapToPoint(float snapPosition, string direction)
    {
        // Snap the current dragged object to the snapping point
        _currentDraggedObj.transform.localPosition = new Vector2(_closestSnapPoint.transform.parent.localPosition.x, snapPosition);

        // Give reference to the dragged rocket part which rocket part it was snapped to and vice-versa
        GameObject snappedRocketPart = _closestSnapPoint.transform.parent.gameObject;
        RocketPart snappedRocketPartScript = snappedRocketPart.GetComponent<RocketPart>();
        RocketPart currentRocketPartScript = _currentDraggedObj.GetComponent<RocketPart>();
        if (direction == "top")
        {
            currentRocketPartScript.rocketPartOnBottom = snappedRocketPart;
            if (currentRocketPartScript.snappingPointOnBottom)
            {
                currentRocketPartScript.snappingPointOnBottom.GetComponent<SnappingPoint>().isAttached = true;
            }
            snappedRocketPartScript.rocketPartOnTop = _currentDraggedObj.gameObject;
        }
        else if (direction == "bottom")
        {
            currentRocketPartScript.rocketPartOnTop = snappedRocketPart;
            if (currentRocketPartScript.snappingPointOnTop)
            {
                currentRocketPartScript.snappingPointOnTop.GetComponent<SnappingPoint>().isAttached = true;
            }
            snappedRocketPartScript.rocketPartOnBottom = _currentDraggedObj.gameObject;
        }
        else if (direction == "left")
        {
            currentRocketPartScript.rocketPartOnRight = snappedRocketPart;
            if (currentRocketPartScript.snappingPointOnRight)
            {
                currentRocketPartScript.snappingPointOnRight.GetComponent<SnappingPoint>().isAttached = true;
            }
            snappedRocketPartScript.rocketPartOnLeft = _currentDraggedObj.gameObject;
        }
        else if (direction == "right")
        {
            currentRocketPartScript.rocketPartOnLeft = snappedRocketPart;
            if (currentRocketPartScript.snappingPointOnLeft)
            {
                currentRocketPartScript.snappingPointOnLeft.GetComponent<SnappingPoint>().isAttached = true;
            }
            snappedRocketPartScript.rocketPartOnRight = _currentDraggedObj.gameObject;
        }

        // Disable snapping points that were connected
        SnappingPoint closestSnapPointScript = _closestSnapPoint.GetComponent<SnappingPoint>();
        closestSnapPointScript.isAttached = true;

        // Assign parent after delay
        Invoke("AssignParent", 0.01f);
    }
    void AssignParent()
    {
        // feature for later. Need to group rocket parts so that they detatch together on separation.
        // _currentDraggedObj.transform.SetParent(_closestSnapPoint.transform.parent.transform);
        ObjectSnappedInPlace();
    }
    void ObjectSnappedInPlace()
    {
        _closestSnapPoint = null;
        _currentDraggedObj.GetComponent<RocketPart>().isPartOfTheRocket = true;
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
