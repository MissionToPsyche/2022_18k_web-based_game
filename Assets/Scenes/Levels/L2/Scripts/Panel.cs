using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Panel : MonoBehaviour
{
    public UIManager uiManager;

    public void OnFinishedBuildingTheRocket()
    {
        uiManager.OnFinishedBuilding();
    }
    public void OnWinGame()
    {
        uiManager.OnWinGame();
        Invoke("LoadNextLevel", 8f);
    }
    private void LoadNextLevel()
    {
        SceneManager.LoadScene("Lvl3", LoadSceneMode.Single);
    }
}
