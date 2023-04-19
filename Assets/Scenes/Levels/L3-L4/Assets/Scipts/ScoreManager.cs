using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;

    public Text scoreSys;
    public Text showScore;
    public int score = 0;
    private bool shipscanned;
    public GameObject scanUI;
    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        shipscanned = false;
    }

    void Update()
    {
        if (score >= 1000)
        {
            Next();
        }
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
                StartCoroutine(Wait());
            }
            
        }

        if (x == "iron")
        {
            score += 15;
            scoreSys.text = "Iron Scanned +15";
            showScore.text = "Score: " + score;
            StartCoroutine(Wait());
        }

        if (x == "nickel")
        {
            score += 10;
            scoreSys.text = "Nickel Scanned +10";
            showScore.text = "Score: " + score;
            StartCoroutine(Wait());
        }

        if (x == "gold")
        {
            score += 30;
            scoreSys.text = "Gold Scanned +30";
            showScore.text = "Score: " + score;
            StartCoroutine(Wait());
        }

        if (x == "ice")
        {
            score += 5;
            scoreSys.text = "Silicate Scanned +5";
            showScore.text = "Score: " + score;
            StartCoroutine(Wait());
        }
    }

    void Next()
    {
        SceneManager.LoadScene("End");
    }

    IEnumerator Wait()
    {

        scanUI.SetActive(true);
        yield return new WaitForSeconds(.5f);
        scanUI.SetActive(false);
    }
}
