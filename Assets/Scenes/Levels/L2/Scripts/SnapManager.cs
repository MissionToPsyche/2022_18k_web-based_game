using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnapManager : MonoBehaviour
{
    public List<Transform> snapPoints;
    public Transform rocket;
    public List<Draggable> draggableObjects;
    public float snapRange = 0.5f;
    private Draggable _currentDraggedObj;
    private Transform _closestSnapPoint = null;

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

        foreach (Transform snapPoint in snapPoints)
        {
            float currentDistance = Vector2.Distance(_currentDraggedObj.transform.position, snapPoint.position);
            if (_closestSnapPoint == null || currentDistance < closestDistance)
            {
                _closestSnapPoint = snapPoint;
                closestDistance = currentDistance;
            }

            if (_closestSnapPoint != null && closestDistance <= snapRange)
            {
                // reset parent to rocket (or the local position translation won't be correct)
                _currentDraggedObj.transform.SetParent(rocket.transform);

                // we will use the snap object's z position to know whether or not to put the dragging object on top or bottom of the snap point
                // snap to top
                if (_closestSnapPoint.transform.localPosition.z == 1)
                {
                    _currentDraggedObj.transform.localPosition = new Vector2(_closestSnapPoint.transform.parent.localPosition.x, _currentDraggedObj.spriteRenderer.bounds.size.y / 2);
                }

                // snap to bottom
                else if (_closestSnapPoint.transform.localPosition.z == -1)
                {
                    _currentDraggedObj.transform.localPosition = new Vector2(_closestSnapPoint.transform.parent.localPosition.x, -_currentDraggedObj.spriteRenderer.bounds.size.y / 2);
                }
                else
                {
                    Debug.Log("Snap point neither top or bottoms");
                }

                // assign parent after delay
                Invoke("assignParent", 0.01f);
            }
        }
    }
    void assignParent()
    {
        _currentDraggedObj.transform.SetParent(_closestSnapPoint.transform.parent.transform);
        objectSnappedInPlace();
    }
    void objectSnappedInPlace()
    {
        _closestSnapPoint = null;
    }
    public void addDraggableObjCallback(Draggable draggableObj)
    {
        draggableObj.dragEndedCallback = OnDragEnded;
    }
}
