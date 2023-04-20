using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEngine.PlayerLoop.PreLateUpdate;

public class EndGame : MonoBehaviour
{
    public Text tex;
    

    void Start()
    {
        tex.text = "Mission Complete\n\nYou have collected 500 resources\n\nin " + string.Format("{0:00}:{1:00}", Timer.minutes, Timer.seconds);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void Update()
    {
        //StartCoroutine(Fadein());
    }

    

    public void ReturnButton()
    {
        SceneManager.LoadScene("Main Menu");
    }
}
