using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipAnim : MonoBehaviour
{
    private float startTime;
    private bool isScaling = true;
    private Vector3 startScale;

    public float duration = 3f;
    private float speed = 1f;
    private float _size = 0.3f;
    public AudioClip audioClip;

    private AudioSource audioSource;
    private void Start()
    {
        startScale = transform.localScale;
        startTime = Time.time;
        audioSource = GetComponent<AudioSource>();
        transform.position = new Vector3(transform.position.x, transform.position.y - 5f, transform.position.z - 5f);
        Invoke("SpeedBurst", 7f);
    }

    private void Update()
    {
        if (isScaling)
        {
            float t = (Time.time - startTime) / duration;
            transform.localScale = Vector3.Lerp(startScale, Vector3.one * _size, t);
            if (t >= 1f)
            {
                isScaling = false;
                startTime = Time.time;
                if (audioClip != null)
                {
                    audioSource.clip = audioClip;
                    audioSource.Play();
                }
            }
        }
        else
        {
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }
    }
    private void SpeedBurst()
    {
        speed = 20f;
    }
}
