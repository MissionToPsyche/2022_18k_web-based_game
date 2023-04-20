using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VCameraShake : MonoBehaviour
{
    public float duration = 0.3f;
    public float amplitude = 1.2f;
    public float frequency = 2f;

    private float remaining = 0f;

    public CinemachineVirtualCamera cam;
    private CinemachineBasicMultiChannelPerlin noise;
    // Start is called before the first frame update
    void Start()
    {
        noise = cam?.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    // Update is called once per frame
    void Update()
    {
        if (noise == null)
        {
            return;
        }

        switch (remaining)
        {
            case > 0f:
                noise.m_AmplitudeGain = amplitude;
                noise.m_FrequencyGain = frequency;
                remaining -= Time.deltaTime;
                break;
            default:
                noise.m_AmplitudeGain = 0f;
                remaining = 0f;
                break;
        }
    }

    public void Shake()
    {
        remaining = duration;
        Debug.Log("shake occured");
    }
}
