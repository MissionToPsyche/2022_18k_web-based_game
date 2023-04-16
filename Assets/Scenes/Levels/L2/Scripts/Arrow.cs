using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public GameObject wormhole;
    public GameObject rocket;
    public Camera mainCamera;
    public RectTransform canvasTransform;
    private RectTransform arrowRectTransform;
    private Vector2 direction;
    private float angle;

    void Start()
    {
        arrowRectTransform = GetComponent<RectTransform>();
    }

    void Update()
    {
        // Convert the world position of the wormhole and rocket to screen positions
        Vector3 wormholeScreenPosition = mainCamera.WorldToScreenPoint(wormhole.transform.position);
        Vector3 rocketScreenPosition = mainCamera.WorldToScreenPoint(rocket.transform.position);

        // Convert the screen positions of the wormhole and rocket to the canvas's local positions
        Vector2 wormholeCanvasPosition, rocketCanvasPosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasTransform, wormholeScreenPosition, mainCamera, out wormholeCanvasPosition);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasTransform, rocketScreenPosition, mainCamera, out rocketCanvasPosition);

        // Calculate the direction from the rocket to the wormhole in the canvas space
        direction = wormholeCanvasPosition - rocketCanvasPosition;

        // Calculate the angle between the arrow's forward direction (the positive Y-axis) and the direction to the wormhole
        angle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;

        // Rotate the arrow RectTransform to face the wormhole
        arrowRectTransform.localRotation = Quaternion.Euler(0, 0, -angle);
    }
}
