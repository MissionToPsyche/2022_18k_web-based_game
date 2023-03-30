using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    GameObject canvas;
    private AudioSource[] audioSources;
    // Start is called before the first frame update
    void Start()
    {
        audioSources = FindObjectsOfType(typeof(AudioSource)) as AudioSource[];
        canvas = gameObject.transform.GetChild(0).gameObject;
        canvas.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (!Input.GetKeyDown(KeyCode.Escape))
        {
            return;
        }

        bool isPaused = Time.timeScale == 0f;
        if (isPaused)
        {
            Resume();
            return;
        }
        Pause();
    }

    public void Pause()
    {
        canvas.SetActive(true);
        setAllAudio(false);
        Time.timeScale = 0f;
    }

    private void setAllAudio(bool enabled)
    {
        foreach (AudioSource audioS in audioSources)
        {
            audioS.enabled = enabled;
        }
    }

    public void Resume()
    {
        canvas.SetActive(false);
        setAllAudio(true);
        Time.timeScale = 1f;
    }

    public void RestartLevel()
    {
        Resume();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void RestartGame()
    {
        Resume();
        SceneManager.LoadScene(0);
    }
}
