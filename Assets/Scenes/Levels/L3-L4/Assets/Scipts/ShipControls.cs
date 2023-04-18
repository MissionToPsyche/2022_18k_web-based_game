using Assets.Scipts;
using Codice.CM.Common.Tree;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ShipControls : MonoBehaviour
{
    public float forwardspeed = 25f, strafespeed = 7f, hoverspeed = 5f;
    private float activeforwardspeed, activestrafespeed, activehoverspeed;
    private float forwardAccelertation = 2.5f, strafespeedAccelertation = 2f, hoveraccelertation = 2f;
    
    public float lookrotatespeed = 90f;
    private Vector2 lookinput, screenCenter, mouseDistance;

    private float rollinput;
    public float rollspeed = 90f, rollAcceleration = 3.5f;

    public int maxHealth = 100;
    public float currentHealth;
    public float healthBoost = 10f;

    private float healthLoss = .1f;
    private float nexthealthLoss = .1f;
    private float myTime = .01f;

    public HealthBar healthBar;

    public GameOver gameOver;

    public Lvl3WinLandShip change;

    public IUnityService UnityService;

    private int startCursorLock = 0;

    public GameObject introScreen;

    public Image fill;
    private float blink = .15f;

    public CamShake camShake;

    public void GameOver()
    {
        gameOver.Setup();
    }

    public void ChangeScene()
    {
        Cursor.visible = true;
        change.Setup();
    }

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;

        if (UnityService == null)
        {
            UnityService = new UnityService();
        }

        screenCenter.x = Screen.width * .5f;
        screenCenter.y = Screen.height * .5f;

        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;

        currentHealth = maxHealth;
        healthBar.setMaxHealth(maxHealth);

        introScreen.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {

        //took out Input.anykeydown for level 3 as I want the first input from the player to be the mouse
        if(Input.anyKeyDown)
        {
            startCursorLock = 1;
            introScreen.SetActive(false);
        }

        if (startCursorLock == 1)
        {
            if (getHealth() <= 0)
            {
                lookinput.x = Input.mousePosition.x;
                lookinput.y = Input.mousePosition.y;

                mouseDistance.x = (lookinput.x - screenCenter.x) / screenCenter.y;
                mouseDistance.y = (lookinput.y - screenCenter.y) / screenCenter.y;

                rollinput = Mathf.Lerp(rollinput, UnityService.GetAxisRaw("Roll"), rollAcceleration * UnityService.GetDeltaTime());

                mouseDistance = Vector2.ClampMagnitude(mouseDistance, .1f);

                transform.Rotate(-mouseDistance.y * lookrotatespeed * UnityService.GetDeltaTime(), mouseDistance.x * lookrotatespeed * UnityService.GetDeltaTime(), rollinput * rollspeed * UnityService.GetDeltaTime(), Space.Self);

                GameOver();

            }
            else
            {
                myTime = myTime + Time.deltaTime;

                if (Input.GetButton("Boost") && myTime > nexthealthLoss)
                {

                    nexthealthLoss = myTime + healthLoss;
                    TakeDamage(.75f);
                    nexthealthLoss = nexthealthLoss - myTime;
                    myTime = .01f;
                }
                if (Input.GetButton("Horizontal") && myTime > nexthealthLoss)
                {

                    nexthealthLoss = myTime + healthLoss;
                    TakeDamage(.1f);
                    nexthealthLoss = nexthealthLoss - myTime;
                    myTime = .01f;
                }
                if (Input.GetButton("Vertical") && myTime > nexthealthLoss)
                {

                    nexthealthLoss = myTime + healthLoss;
                    TakeDamage(.1f);
                    nexthealthLoss = nexthealthLoss - myTime;
                    myTime = .01f;
                }
                if (Input.GetButton("Hover") && myTime > nexthealthLoss)
                {

                    nexthealthLoss = myTime + healthLoss;
                    TakeDamage(.1f);
                    nexthealthLoss = nexthealthLoss - myTime;
                    myTime = .01f;
                }

                lookinput.x = Input.mousePosition.x;
                lookinput.y = Input.mousePosition.y;

                mouseDistance.x = (lookinput.x - screenCenter.x) / screenCenter.y;
                mouseDistance.y = (lookinput.y - screenCenter.y) / screenCenter.y;

                rollinput = Mathf.Lerp(rollinput, UnityService.GetAxisRaw("Roll"), rollAcceleration * UnityService.GetDeltaTime());

                mouseDistance = Vector2.ClampMagnitude(mouseDistance, .75f);

                transform.Rotate(-mouseDistance.y * lookrotatespeed * UnityService.GetDeltaTime(), mouseDistance.x * lookrotatespeed * UnityService.GetDeltaTime(), rollinput * rollspeed * UnityService.GetDeltaTime(), Space.Self);


                //boost feature
                if (Input.GetKeyDown(KeyCode.LeftShift))
                {
                    forwardspeed = 120f;
                }
                if (Input.GetKeyUp(KeyCode.LeftShift))
                {
                    forwardspeed = 25f;
                }

                activeforwardspeed = Mathf.Lerp(activeforwardspeed, UnityService.GetAxisRaw("Vertical") * forwardspeed, forwardAccelertation * UnityService.GetDeltaTime());
                activestrafespeed = Mathf.Lerp(activestrafespeed, UnityService.GetAxisRaw("Horizontal") * strafespeed, strafespeedAccelertation * UnityService.GetDeltaTime());
                activehoverspeed = Mathf.Lerp(activehoverspeed, UnityService.GetAxisRaw("Hover") * hoverspeed, hoveraccelertation * UnityService.GetDeltaTime());

                transform.position += (transform.forward * activeforwardspeed * UnityService.GetDeltaTime());
                transform.position += (transform.right * activestrafespeed * UnityService.GetDeltaTime());
                transform.position += (transform.up * activehoverspeed * UnityService.GetDeltaTime());
            }
        }

        if (Time.time >= blink)
        {
            Debug.Log(Time.time + ">=" + blink);

            if (getHealth() <= 40)
            {
                if (fill.color == Color.red)
                {
                    fill.color = Color.white;
                }
                else
                {
                    fill.color = Color.red;
                }
            }
            else
            {
                fill.color = Color.red;
            }

            blink += .15f;
        }


    }


    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Health")
        {
            if(currentHealth <= maxHealth)
            {
                gainHealth();
                Destroy(collision.gameObject);
                

            }
            
        }

        if(collision.gameObject.tag == "CollideObject")
        {
            TakeDamage(5f);
            Destroy(collision.gameObject);

            StartCoroutine(camShake.Shake(.15f, .4f));
            
        }

        if(collision.gameObject.tag == "Goal")
        {
            forwardspeed = 0;
            strafespeed = 0;
            rollspeed = 0;
            hoverspeed = 0;
            lookrotatespeed = 0;
            //ChangeScene();

            change.Setup();
        }
    }

    void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if(currentHealth > 100)
        {
            currentHealth = 100;
            healthBar.setHealth(currentHealth);
        }
        
        healthBar.setHealth(currentHealth);
        
        
    }
    
    void gainHealth()
    {
        currentHealth += healthBoost;
        if (currentHealth > 100)
        {
            currentHealth = 100;
            healthBar.setHealth(currentHealth);
        }

        healthBar.setHealth(currentHealth);
    }

    public float getHealth()
    {
        return currentHealth;
    }

    
}
