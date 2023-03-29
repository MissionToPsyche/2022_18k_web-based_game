using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Level1AudioManager : MonoBehaviour
{
    public MovementController player;
    

    public AudioSource audios;
    public AudioClip walking;
    public AudioClip land;
    public AudioClip jump;
    public AudioClip collected;
    // Start is called before the first frame update
    void Start()
    {
        audios = GetComponent<AudioSource>();
        player = GetComponent<MovementController>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!audios.isPlaying)
        {
                if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.LeftArrow))
                {
                        audios.clip = walking;
                        audios.volume = .75f;
                        audios.pitch = .8f;
                        audios.Play();
                    

                }
                else
                {
                    audios.Stop();
                }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            audios.Stop();
            audios.clip = jump;
            audios.volume = 1f;
            audios.pitch = 1f;
            audios.Play();
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        

        if (col.gameObject.tag == "collectible")
        {
            Debug.Log("hi");
            audios.Stop();
            audios.clip = collected;
            audios.pitch = 1f;
            audios.Play();
        }
         
          
    }
}
