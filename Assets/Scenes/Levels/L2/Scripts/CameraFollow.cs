using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject cam;
    public bool followX = true;
    public bool followY = true;

    void Update()
    {
        if (followX && followY)
        {
            transform.position = new Vector3(cam.transform.position.x, cam.transform.position.y, transform.position.z);
        }
        else if (followX)
        {
            transform.position = new Vector3(cam.transform.position.x, transform.position.y, transform.position.z);
        }
        else if (followY)
        {
            transform.position = new Vector3(transform.position.x, cam.transform.position.y, transform.position.z);
        }
    }
}
