using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    Scene currentscene;
    string sceneName;

    public void Setup()
    {
        currentscene = SceneManager.GetActiveScene();

        sceneName = currentscene.name;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

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
