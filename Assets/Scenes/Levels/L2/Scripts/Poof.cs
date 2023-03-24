using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poof : MonoBehaviour
{
    public GameObject PoofText;
    void Start()
    {
        PoofText.SetActive(false);
    }

    public void PoofActivate()
    {
        PoofText.SetActive(true);
        // Play poof audio
        AudioManager.instance.Play("Poof");
    }
    public void PoofEnd()
    {
        // Disable parent
        // If it is the capsule, game is over
        if (transform.parent.gameObject.tag == "Capsule")
        {
            GameOver();
        }
        transform.parent.gameObject.SetActive(false);
    }
    void GameOver()
    {
        SendMessageUpwards("OnGameOver", SendMessageOptions.RequireReceiver);
    }

}
