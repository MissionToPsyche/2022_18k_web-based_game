using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float smoothTime = 0;

    private Vector3 velocity = Vector3.zero;

    private void LateUpdate()
    {
        // If target is not set, return
        if (target == null)
        {
            return;
        }

        // Smoothly move the camera towards the target
        Vector3 targetPosition = target.position;
        targetPosition.z = transform.position.z;
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
    }
}