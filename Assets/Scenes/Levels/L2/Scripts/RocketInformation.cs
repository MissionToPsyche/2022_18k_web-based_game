using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketInformation : MonoBehaviour
{
    public int
        tier1Capsule,
        tier2Capsule,
        tier3Capsule,
        tier1NoseCone,
        tier2NoseCone,
        tier1Engine,
        tier2Engine,
        tier3Engine,
        separator,
        sideSeparator,
        smallFuelTank,
        mediumFuelTank,
        largeFuelTank;

    private Dictionary<short, bool> collected;

    public static RocketInformation instance { get; private set; }

    public void Collect(short part)
    {
        collected[part] = true;
        printCollected();
    }

    private void printCollected()
    {
        foreach(var pair in collected)
        {
            Debug.Log(pair.Key + " : " + pair.Value);
        }
    }

    void Awake()
    {
        // if there is already an instance, destroy it
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        // else, keep instance of this object
        instance = this;
        collected = new();
        DontDestroyOnLoad(gameObject);
    }
}
