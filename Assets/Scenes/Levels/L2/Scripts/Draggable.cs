using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Draggable : MonoBehaviour
{
    public delegate void DragEndedDelegate(Draggable draggableObj);
    [System.NonSerialized]
    public SpriteRenderer spriteRenderer;
    public DragEndedDelegate dragEndedCallback;
    private Vector3 _dragOffset;
    private Camera _cam;
    public bool isDraggable;
    private RocketPart _rocketPartScript;
    private int _numberOfConnectedParts = 0;

    void Awake()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        _rocketPartScript = gameObject.GetComponent<RocketPart>();
        _cam = Camera.main;
        isDraggable = true;     // Unless set to true in Awake, it defaults to false no matter what
    }

    public void OnMouseDown()
    {
        _numberOfConnectedParts = 0;
        if (_rocketPartScript.rocketPartOnBottom)
        {
            _numberOfConnectedParts++;
        }
        if (_rocketPartScript.rocketPartOnTop)
        {
            _numberOfConnectedParts++;
        }
        if (_rocketPartScript.rocketPartOnLeft)
        {
            _numberOfConnectedParts++;
        }
        if (_rocketPartScript.rocketPartOnRight)
        {
            _numberOfConnectedParts++;
        }
        // Cannot move rocket parts with more than 1 connections
        if (isDraggable && _numberOfConnectedParts <= 1)
        {
            _rocketPartScript.OnDragStart();
            SnapManager.instance.ToggleSnappingPoints(gameObject);
            _dragOffset = transform.position - GetMousePos();
        }
    }
    public void OnMouseDrag()
    {
        if (isDraggable && _numberOfConnectedParts <= 1)
        {
            transform.position = GetMousePos() + _dragOffset;
        }
    }

    public void OnMouseUp()
    {
        if (isDraggable && _numberOfConnectedParts <= 1)
        {
            SnapManager.instance.ToggleSnappingPoints(gameObject);
            dragEndedCallback(this);
        }
    }
    Vector3 GetMousePos()
    {
        Vector3 mousePos = _cam.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        return mousePos;
    }
}
