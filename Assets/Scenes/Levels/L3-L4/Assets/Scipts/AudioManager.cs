using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public PlayerMovement player;
    public Grappling grappleGun;

    public AudioSource audios;
    public AudioClip walking;
    public AudioClip land;
    public AudioClip jump;
    public AudioClip running;
    public AudioClip reel;
    public AudioClip grapple;
    public AudioClip scan;


    // Start is called before the first frame update

    private void Start()
    {
        audios = GetComponent<AudioSource>();
        player = GetComponent<PlayerMovement>();
        grappleGun = GetComponent<Grappling>();
    }


    // Update is called once per frame
    void Update()
    {

        if (!audios.isPlaying)
        {

            if (audios.isPlaying == false)
            {

                if (player.state == PlayerMovement.MovementState.walking)
                {
                    audios.clip = walking;
                    audios.pitch = 1.5f;
                    audios.Play();
                }

                if (player.state == PlayerMovement.MovementState.sprinting)
                {
                    audios.clip = running;
                    audios.pitch = 1f;
                    audios.Play();
                }

                if (player.activeGrapple == true && player.state == PlayerMovement.MovementState.air)
                {
                    audios.clip = reel;
                    audios.pitch = 1f;
                    audios.Play();

                    if (player.state != PlayerMovement.MovementState.air)
                    {
                        audios.Stop();
                    }
                }
                
            }
        }

        


        if (Input.GetMouseButton(1))
        {
            audios.clip = grapple;
            audios.pitch = 1f;
            audios.Play();
        }

        if (Input.GetMouseButton(0))
        {
            audios.clip = scan;
            audios.pitch = 1f;
            audios.Play();
        }

        if (Input.GetKey(KeyCode.Space))
        {
            audios.clip = jump;
            audios.pitch = 1f;
            audios.Play();
        }

    }

    void OnCollisionEnter(Collision col)
    {
        
        bool canPlayJumpLandingSound;
        if (col.gameObject.name == "TerrainIsGround")
        {
            canPlayJumpLandingSound = true;
            if (Input.GetKey(KeyCode.C))
            {
                canPlayJumpLandingSound = false;
            }
            if (player.state == PlayerMovement.MovementState.stationary && canPlayJumpLandingSound == true)
            {
                canPlayJumpLandingSound = false;
                audios.Stop();
                audios.clip = land;
                audios.pitch = 1.5f;
                audios.Play();
            }
        }
    }






}
