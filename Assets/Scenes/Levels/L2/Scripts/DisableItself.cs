using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableItself : MonoBehaviour
{
    public float disableTime = 5.0f; // the number of seconds before disabling the script

    void OnEnable()
    {
        Invoke("DisableSelf", disableTime); // invoke the DisableSelf method after the specified number of seconds
    }

    void DisableSelf()
    {
        gameObject.SetActive(false); // disable the script's game object
    }
}
