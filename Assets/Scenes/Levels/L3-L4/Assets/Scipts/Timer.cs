using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.PlayerLoop.PreLateUpdate;

public class Timer : MonoBehaviour
{
    public Text timer;
    public int time = 0;
    private int nextUpdate = 1;
    ScoreManager scoreManager;
    public static float minutes = 0;
    public static float seconds = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Time.timeSinceLevelLoad >= nextUpdate)
        {
            //Debug.Log(Time.time + ">=" + nextUpdate);

            nextUpdate = Mathf.FloorToInt(Time.timeSinceLevelLoad) + 1;

            minutes = Mathf.FloorToInt(nextUpdate / 60);

            seconds = Mathf.FloorToInt(nextUpdate % 60);

            timer.text = string.Format("{0:00}:{1:00}", minutes, seconds);            

        }
    }
}
