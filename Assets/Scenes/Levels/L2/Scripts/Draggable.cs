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


    void Awake()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        _cam = Camera.main;
    }

    void OnMouseDown()
    {
        _dragOffset = transform.position - GetMousePos();
    }

    public void OnMouseDrag()
    {
        transform.position = GetMousePos() + _dragOffset;
    }

    public void OnMouseUp()
    {
        dragEndedCallback(this);
    }

    Vector3 GetMousePos()
    {
        Vector3 mousePos = _cam.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        return mousePos;
    }
}
