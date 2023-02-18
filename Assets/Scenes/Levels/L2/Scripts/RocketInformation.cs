using InterWorld.Shared.Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    private HashSet<short> collected;

    public static RocketInformation instance { get; private set; }

    public void Collect(short part)
    {
        collected.Add(part);
    }

    public void Reset()
    {
        collected.Clear();
    }

    /// <summary>
    /// Checks if the supplied type and tier is collected
    /// </summary>
    /// <param name="type">the type to check</param>
    /// <param name="teir">the tier to collect</param>
    /// <returns>true if the part is collected</returns>
    public bool IsCollectedTypeAndTier(RocketPartType type, RocketPartTeir teir)
    {
        short expected = (short)((short)teir | (short)type);
        return collected.Contains(expected);
    }

    /// <summary>
    /// Checks if the supplied part is collected
    /// </summary>
    /// <param name="type">the type to check</param>
    /// <returns>true if the type is collected</returns>
    public bool IsCollectedType(RocketPartType type)
    {
        short expected = (short)type;
        foreach (short part in collected)
        {
            if((expected & part) != 0)
            {
                return true;
            }
        }
        return false;
    }

   
    /// <summary>
    /// Checks if all the supplied types are collected
    /// </summary>
    /// <param name="types">the types to check</param>
    /// <returns>true if all types are collected</returns>
    public bool IsCollectedTypes(params RocketPartType[] types)
    {
        return types.All(IsCollectedType);
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
