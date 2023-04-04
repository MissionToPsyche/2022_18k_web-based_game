using Assets.Scipts;
using PlasticGui.WorkspaceWindow.PendingChanges;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    public float healthBoost = 50f;

    private float healthLoss = .1f;
    private float nexthealthLoss = .1f;
    private float myTime = .01f;

    public HealthBar healthBar;

    public GameOver gameOver;

    public Lvl3WinLandShip change;

    public IUnityService UnityService;

    private int startCursorLock = 0;

    public GameObject introScreen;

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

        StartCoroutine(Intro(5));
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0)
        {
            startCursorLock = 1;
        }

        if(startCursorLock == 1)
        {
            if (getHealth() <= 0)
            {
                lookinput.x = Input.mousePosition.x;
                lookinput.y = Input.mousePosition.y;

                mouseDistance.x = (lookinput.x - screenCenter.x) / screenCenter.y;
                mouseDistance.y = (lookinput.y - screenCenter.y) / screenCenter.y;

                rollinput = Mathf.Lerp(rollinput, UnityService.GetAxisRaw("Roll"), rollAcceleration * UnityService.GetDeltaTime());

                mouseDistance = Vector2.ClampMagnitude(mouseDistance, 1f);

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

                mouseDistance = Vector2.ClampMagnitude(mouseDistance, 1f);

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
        
        
    }


    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Health")
        {
            if(currentHealth <= maxHealth)
            {
                gainHealth(healthBoost);
                Destroy(collision.gameObject);
                

            }
            
        }

        if(collision.gameObject.tag == "CollideObject")
        {
            TakeDamage(5f);
            Destroy(collision.gameObject);
            
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
    
    void gainHealth(float boost)
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

    IEnumerator Intro (float delay)
    {
        introScreen.SetActive(true);
        yield return new WaitForSeconds(delay);
        introScreen.SetActive(false);

    }
}
