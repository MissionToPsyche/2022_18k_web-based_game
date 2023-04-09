using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Health : MonoBehaviour
{
    public int maxHealth = 150;
    public float currentHealth;

    private float healthLoss = .1f;
    private float nexthealthLoss = .1f;
    private float myTime = .01f;

    public HealthBar healthBar;
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        healthBar.setMaxHealth(maxHealth);
    }

    // Update is called once per frame
    void Update()
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
        
    }

    void TakeDamage(float damage)
    {
        currentHealth -= damage;
        healthBar.setHealth(currentHealth);
    }

    public float getHealth()
    {
        return currentHealth;
    }
}
