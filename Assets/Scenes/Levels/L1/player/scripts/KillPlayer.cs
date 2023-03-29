using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class KillPlayer : MonoBehaviour
{
    //setup for audio on death
    public AudioSource audios;
    public AudioClip death;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        bool isPlayer = other.CompareTag("Player");

        // Reload the scene when you touch a spike
        if(isPlayer)
        {
            //play sound before right before restart
            audios.clip = death;
            audios.volume = 1f;
            audios.pitch = 1f;
            audios.Play();

            //RestartCurrentScene();
            //had to change this line to invoke after three seconds to allow the sound to play
            //otherwise the level restarts too quikcly which cuts the sound out
            Invoke("RestartCurrentScene", .5f);
        }        
    }

    public void RestartCurrentScene()
    {
        int currentScene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentScene);
    }
}
