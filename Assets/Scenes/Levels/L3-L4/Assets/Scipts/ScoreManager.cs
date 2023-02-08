using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;

    public Text scoreSys;
    int score = 0;
    private bool shipscanned;
    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        shipscanned = false;
    }


    public void AddPoints(string x)
    {
        if (x == "ship")
        {
            if (shipscanned == false)
            {
                score += 1;
                scoreSys.text = "Ship Scanned +1" + ": Score Total = " + score;
                shipscanned = true;
            }
            
        }

        if (x == "iron")
        {
            score += 10;
            scoreSys.text = "Iron Scanned +10" + ": Score Total = " + score;
        }

        if (x == "nickel")
        {
            score += 5;
            scoreSys.text = "Nickel Scanned +5" + ": Score Total = " + score;
        }

        if (x == "gold")
        {
            score += 25;
            scoreSys.text = "Gold Scanned +25" + ": Score Total = " + score;
        }

        if (x == "ice")
        {
            score += 2;
            scoreSys.text = "Ice Scanned +2" + ": Score Total = " + score;
        }
    }
}
