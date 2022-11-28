using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnapManager : MonoBehaviour
{
    private List<GameObject> _snappingPointObjs = new List<GameObject>();
    public List<Draggable> draggableObjects = new List<Draggable>();

    public Transform rocket;
    public float snapRange = 0.5f;
    private Draggable _currentDraggedObj;
    private Transform _closestSnapPoint = null;
    public static SnapManager instance;
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
            DontDestroyOnLoad(gameObject);
        }
    }
    void Start()
    {
        foreach (Draggable draggableObj in draggableObjects)
        {
            draggableObj.dragEndedCallback = OnDragEnded;
        }
    }

    private void OnDragEnded(Draggable draggedObj)
    {
        _currentDraggedObj = draggedObj;
        float closestDistance = 100000f;

        foreach (GameObject snapPoint in _snappingPointObjs)
        {
            float currentDistance = Vector2.Distance(_currentDraggedObj.transform.position, snapPoint.transform.position);
            if (_closestSnapPoint == null || currentDistance < closestDistance)
            {
                _closestSnapPoint = snapPoint.transform;
                closestDistance = currentDistance;
            }

            if (_closestSnapPoint != null && closestDistance <= snapRange)
            {
                // reset parent to rocket (or the local position translation won't be correct)
                _currentDraggedObj.transform.SetParent(rocket.transform);

                // we will use the snap object's z position to know whether or not to put the dragging object on top or bottom of the snap point
                float tmp;
                SpriteRenderer closestSnapObjSpriteRenderer = _closestSnapPoint.transform.parent.GetComponent<SpriteRenderer>();
                SnappingPoint closestSnapPointScript = _closestSnapPoint.GetComponent<SnappingPoint>();
                // snap to top
                if (closestSnapPointScript.direction == "top")
                {
                    tmp = _closestSnapPoint.transform.parent.localPosition.y + closestSnapObjSpriteRenderer.bounds.size.y / 2 + _currentDraggedObj.spriteRenderer.bounds.size.y / 2;
                    _currentDraggedObj.transform.localPosition = new Vector2(_closestSnapPoint.transform.parent.localPosition.x, tmp);
                }

                // snap to bottom
                else if (closestSnapPointScript.direction == "bottom")
                {
                    tmp = _closestSnapPoint.transform.parent.localPosition.y - closestSnapObjSpriteRenderer.bounds.size.y / 2 - _currentDraggedObj.spriteRenderer.bounds.size.y / 2;
                    _currentDraggedObj.transform.localPosition = new Vector2(_closestSnapPoint.transform.parent.localPosition.x, tmp);
                }
                // snap to left
                else if (closestSnapPointScript.direction == "left")
                {
                    tmp = _closestSnapPoint.transform.parent.localPosition.x - closestSnapObjSpriteRenderer.bounds.size.x / 2 - _currentDraggedObj.spriteRenderer.bounds.size.x / 2;
                    _currentDraggedObj.transform.localPosition = new Vector2(tmp, -_closestSnapPoint.transform.parent.localPosition.y);
                }
                // snap to right
                else if (closestSnapPointScript.direction == "right")
                {
                    tmp = _closestSnapPoint.transform.parent.localPosition.x + closestSnapObjSpriteRenderer.bounds.size.x / 2 + _currentDraggedObj.spriteRenderer.bounds.size.x / 2;
                    _currentDraggedObj.transform.localPosition = new Vector2(tmp, _closestSnapPoint.transform.parent.localPosition.y);
                }
                else
                {
                    Debug.Log("Snap point invalid, check the direction of the snap point.");
                }
                // assign parent after delay
                Invoke("AssignParent", 0.01f);
            }
        }
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
    }
    public void AddDraggableObjCallback(Draggable draggableObj)
    {
        draggableObj.dragEndedCallback = OnDragEnded;
    }
    public void AddSnappingPointObj(GameObject snappingPointObj)
    {
        _snappingPointObjs.Add(snappingPointObj);
    }

    public void ToggleSnappingPoints()
    {
        foreach (GameObject snappingPointObj in _snappingPointObjs)
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
}
