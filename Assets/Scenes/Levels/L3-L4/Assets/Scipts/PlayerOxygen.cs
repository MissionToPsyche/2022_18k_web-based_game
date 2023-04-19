using Assets.Scipts;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.UI;

public class PlayerOxygen : MonoBehaviour
{
    public OxygenBar oxygenbar;
    public int maxOxy = 200;
    public int currentOxy;
    private int nextUpdate = 1;
    private bool nearShip = false;
    public Image fill;
    private bool lowOx = false;
    private float blink = .25f;
    

    public PlayerMovement failBlock;

    private bool stopupdate = false;

    public GameOver gameOver;
    public void GameOver()
    {
        gameOver.Setup();
    }
    // Start is called before the first frame update
    void Start()
    {
        currentOxy = maxOxy;
        oxygenbar.setMaxOxygen(maxOxy);
    }

    // Update is called once per frame
    void Update()
    {
        if (stopupdate)
        {
            return;
        }

        if (Time.time >= nextUpdate)
        {
            //Debug.Log(Time.time + ">=" + nextUpdate);

            nextUpdate = Mathf.FloorToInt(Time.time) + 1;

            if (nearShip == false)
            {
                UpdateEverySecond();
                if (getOxygen() <= 0)
                {
                    stopupdate = true;
                    failBlock.freezeCharacter();
                    GameOver();
                }
            }
            else if (nearShip == true)
                UpdateEverySecondRefill();

        }

        if(Time.time >= blink)
        {
            Debug.Log(Time.time + ">=" + blink);
            
            if (currentOxy <= 80)
            {
                if (fill.color == Color.blue)
                {
                    fill.color = Color.white;
                }
                else
                {
                    fill.color = Color.blue;
                }
            }
            else
            {
                fill.color = Color.blue;
            }

            blink += .25f;
        }

        
        
    }

    private void OnTriggerEnter(Collider other)
    {  

        if (other.gameObject.tag == "Ship")
        {
            nearShip = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Ship")
        {
            nearShip = false;
        }
    }

    void UpdateEverySecond()
    {
        currentOxy = currentOxy - 1;
        if (currentOxy <= 0)
        {
            currentOxy = 0;
        }
        oxygenbar.setOxygen(currentOxy);
    }

    void UpdateEverySecondRefill()
    {
        currentOxy = currentOxy + 10;
        if (currentOxy > 200)
        {
            currentOxy = 200;
        }
        oxygenbar.setOxygen(currentOxy);
    }

    public float getOxygen()
    {
        return currentOxy;
    }

    
}
