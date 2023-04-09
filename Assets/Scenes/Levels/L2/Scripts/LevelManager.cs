using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{

    
    void Awake()
    {
        Application.targetFrameRate = 60;
        
    }
    void Update()
    {
        Debug.Log(1f / Time.deltaTime);

        
    }
}
