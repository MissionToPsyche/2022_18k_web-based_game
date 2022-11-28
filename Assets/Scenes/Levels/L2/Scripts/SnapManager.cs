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
                float tmp;
                SpriteRenderer closestSnapObjSpriteRenderer = _closestSnapPoint.transform.parent.GetComponent<SpriteRenderer>();

                // snap to top
                if (_closestSnapPoint.transform.localPosition.z == 1)
                {
                    tmp = _closestSnapPoint.transform.parent.localPosition.y + closestSnapObjSpriteRenderer.bounds.size.y / 2 + _currentDraggedObj.spriteRenderer.bounds.size.y / 2;
                    _currentDraggedObj.transform.localPosition = new Vector2(_closestSnapPoint.transform.parent.localPosition.x, tmp);
                }

                // snap to bottom
                else if (_closestSnapPoint.transform.localPosition.z == -1)
                {
                    tmp = _closestSnapPoint.transform.parent.localPosition.y - closestSnapObjSpriteRenderer.bounds.size.y / 2 - _currentDraggedObj.spriteRenderer.bounds.size.y / 2;
                    _currentDraggedObj.transform.localPosition = new Vector2(_closestSnapPoint.transform.parent.localPosition.x, tmp);
                }
                // snap to left
                else if (_closestSnapPoint.transform.localPosition.z == -2)
                {
                    tmp = _closestSnapPoint.transform.parent.localPosition.x - closestSnapObjSpriteRenderer.bounds.size.x / 2 - _currentDraggedObj.spriteRenderer.bounds.size.x / 2;
                    _currentDraggedObj.transform.localPosition = new Vector2(tmp, -_closestSnapPoint.transform.parent.localPosition.y);
                }
                // snap to right
                else if (_closestSnapPoint.transform.localPosition.z == 2)
                {
                    tmp = _closestSnapPoint.transform.parent.localPosition.x + closestSnapObjSpriteRenderer.bounds.size.x / 2 + _currentDraggedObj.spriteRenderer.bounds.size.x / 2;
                    _currentDraggedObj.transform.localPosition = new Vector2(tmp, _closestSnapPoint.transform.parent.localPosition.y);
                }
                else
                {
                    Debug.Log("Snap point invali, check the z index of the snap point.");
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
