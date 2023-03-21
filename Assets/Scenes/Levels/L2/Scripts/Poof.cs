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
    }
    public void PoofEnd()
    {
        // Disable parent
        transform.parent.gameObject.SetActive(false);
    }
}
