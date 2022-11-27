using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketInformation : MonoBehaviour
{
    public int tier1Capsule, tier2Capsule, tier3Capsule,
    tier1NoseCone, tier2NoseCone,
    tier1Engine, tier2Engine, tier3Engine,
    separator, sideSeparator,
    smallFuelTank, mediumFuelTank, largeFuelTank;

    public static RocketInformation instance;
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
}
