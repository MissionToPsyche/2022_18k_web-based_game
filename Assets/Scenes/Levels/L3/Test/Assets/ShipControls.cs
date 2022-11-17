using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

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

    public void GameOver()
    {
        gameOver.Setup();
    }

    // Start is called before the first frame update
    void Start()
    {

        screenCenter.x = Screen.width * .5f;
        screenCenter.y = Screen.height * .5f;

        Cursor.lockState = CursorLockMode.Confined;

        currentHealth = maxHealth;
        healthBar.setMaxHealth(maxHealth);
    }

    // Update is called once per frame
    void Update()
    {
        if(getHealth() <= 0)
        {
            lookinput.x = Input.mousePosition.x;
            lookinput.y = Input.mousePosition.y;

            mouseDistance.x = (lookinput.x - screenCenter.x) / screenCenter.y;
            mouseDistance.y = (lookinput.y - screenCenter.y) / screenCenter.y;

            rollinput = Mathf.Lerp(rollinput, Input.GetAxisRaw("Roll"), rollAcceleration * Time.deltaTime);

            mouseDistance = Vector2.ClampMagnitude(mouseDistance, 1f);

            transform.Rotate(-mouseDistance.y * lookrotatespeed * Time.deltaTime, mouseDistance.x * lookrotatespeed * Time.deltaTime, rollinput * rollspeed * Time.deltaTime, Space.Self);

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

            rollinput = Mathf.Lerp(rollinput, Input.GetAxisRaw("Roll"), rollAcceleration * Time.deltaTime);

            mouseDistance = Vector2.ClampMagnitude(mouseDistance, 1f);

            transform.Rotate(-mouseDistance.y * lookrotatespeed * Time.deltaTime, mouseDistance.x * lookrotatespeed * Time.deltaTime, rollinput * rollspeed * Time.deltaTime, Space.Self);


            //boost feature
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                forwardspeed = 120f;
            }
            if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                forwardspeed = 25f;
            }

            activeforwardspeed = Mathf.Lerp(activeforwardspeed, Input.GetAxisRaw("Vertical") * forwardspeed, forwardAccelertation * Time.deltaTime);
            activestrafespeed = Mathf.Lerp(activestrafespeed, Input.GetAxisRaw("Horizontal") * strafespeed, strafespeedAccelertation * Time.deltaTime);
            activehoverspeed = Mathf.Lerp(activehoverspeed, Input.GetAxisRaw("Hover") * hoverspeed, hoveraccelertation * Time.deltaTime);

            transform.position += (transform.forward * activeforwardspeed * Time.deltaTime);
            transform.position += (transform.right * activestrafespeed * Time.deltaTime);
            transform.position += (transform.up * activehoverspeed * Time.deltaTime);
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
}
