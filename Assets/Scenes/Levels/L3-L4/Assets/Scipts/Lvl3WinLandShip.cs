using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Lvl3WinLandShip : MonoBehaviour
{
    public void Setup()
    {
        gameObject.SetActive(true);
    }

    public void LandButton()
    {
        SceneManager.LoadScene("Lvl4");
    }
}
