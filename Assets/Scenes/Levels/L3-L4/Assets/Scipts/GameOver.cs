using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    Scene currentscene;
    string sceneName;
    public Text lossMessage;

    public void Setup()
    {
        currentscene = SceneManager.GetActiveScene();


        sceneName = currentscene.name;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (sceneName == "Lvl4")
        {
            lossMessage.text = "GAME OVER \nKeep an eye on your oxygen supply on the left";
        }
        else if (sceneName == "Lvl3")
        {
            lossMessage.text = "GAME OVER \nAvoid red asteroids, pickup blue fuel sources";
        }

        gameObject.SetActive(true);

    }

    public void RestartButton()
    {
        if (sceneName == "Lvl4")
        {
            SceneManager.LoadScene("Lvl4");
        }
        else
        {
            SceneManager.LoadScene("Lvl3");
        }

        
    }
    
}
