using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TooltipShower : MonoBehaviour
{
    public static TooltipShower instance;
    public Tooltip tooltip;
    void Awake()
    {
        // if there is already an instance, destroy it
        if (instance != null)
        {
            Destroy(gameObject);
        }
        // else, keep instance of this object
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    void Start()
    {
        tooltip.gameObject.SetActive(false);
    }
    public void Show(string body, string header = "")
    {
        tooltip.SetText(header, body);
        tooltip.gameObject.SetActive(true);
    }
    public void Hide()
    {
        tooltip.gameObject.SetActive(false);
    }
}
