using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;

    public Text scoreSys;
    public Text showScore;
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
                scoreSys.text = "Ship Scanned +1";
                showScore.text = "Score: " + score;
                shipscanned = true;
            }
            
        }

        if (x == "iron")
        {
            score += 10;
            scoreSys.text = "Iron Scanned +10";
            showScore.text = "Score: " + score;
        }

        if (x == "nickel")
        {
            score += 5;
            scoreSys.text = "Nickel Scanned +5";
            showScore.text = "Score: " + score;
        }

        if (x == "gold")
        {
            score += 25;
            scoreSys.text = "Gold Scanned +25";
            showScore.text = "Score: " + score;
        }

        if (x == "ice")
        {
            score += 2;
            scoreSys.text = "Ice Scanned +2";
            showScore.text = "Score: " + score;
        }
    }
}
