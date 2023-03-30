using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public float shakeAmount = 0.7f;
    public float shakeSpeed = 10f;

    private Vector3 originalPosition;
    private bool shaking = false;

    void Awake()
    {
        // Save the original position of the camera
        originalPosition = transform.position;
    }
    public void Shake()
    {
        // Save original position of the camera
        originalPosition = shaking ? originalPosition : transform.position;

        // Start shaking the camera
        shaking = true;
        StartCoroutine(ShakeCoroutine());
    }

    public void StopShake()
    {
        // Stop shaking the camera and reset its position
        shaking = false;
    }

    private IEnumerator ShakeCoroutine()
    {
        while (shaking)
        {
            // Calculate a random offset for the camera position
            float offsetX = Random.Range(-shakeAmount, shakeAmount);
            float offsetY = Random.Range(-shakeAmount, shakeAmount);
            Vector3 offset = new Vector3(offsetX, offsetY, transform.position.z);

            // Apply the offset to the camera position
            transform.position = new Vector3(0, 0, 0) + offset;

            // Wait for a short time before shaking again
            yield return new WaitForSeconds(1 / shakeSpeed);
        }

        // Reset the camera position once shaking has stopped
        transform.position = originalPosition;
    }
}
