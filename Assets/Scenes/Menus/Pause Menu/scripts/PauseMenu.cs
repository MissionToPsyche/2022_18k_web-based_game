using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    GameObject canvas;
    private AudioSource[] audioSources;
    private Canvas[] canvases;
    // Start is called before the first frame update
    void Start()
    {
        audioSources = FindObjectsOfType(typeof(AudioSource)) as AudioSource[];
        canvases = FindObjectsOfType(typeof(Canvas)) as Canvas[];
        canvas = gameObject.transform.GetChild(0).gameObject;
        canvas.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (canvas.activeInHierarchy)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

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
        setAllCanvas(false);
        Time.timeScale = 0f;
    }

    private void setAllAudio(bool enabled)
    {
        foreach (AudioSource audioS in audioSources)
        {
            audioS.mute = !enabled;
        }
    }

    private void setAllCanvas(bool enabled)
    {
        foreach (Canvas c in canvases)
        {
            bool isPuaseMenuCanvas = c.GetComponentInParent<PauseMenu>() != null;
            if (isPuaseMenuCanvas)
            {
                continue;
            }
            c.enabled = enabled;
        }
    }

    public void Resume()
    {
        canvas.SetActive(false);
        setAllAudio(true);
        setAllCanvas(true);
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
