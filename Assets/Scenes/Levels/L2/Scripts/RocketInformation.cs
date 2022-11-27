using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketInformation : MonoBehaviour
{
    private int _tier1Capsule, _tier2Capsule, _tier3Capsule,
    _tier1NoseCone, _tier2NoseCone,
    _tier1Engine, _tier2Engine, _tier3Engine,
    _separator, _sideSeparator,
    _smallFuelTank, _mediumFuelTank, _largeFuelTank;

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
    // getters
    public int getTier1CapsuleCount()
    {
        return _tier1Capsule;
    }
    public int getTier2CapsuleCount()
    {
        return _tier2Capsule;
    }
    public int getTier3CapsuleCount()
    {
        return _tier3Capsule;
    }
    public int getTier1EngineCount()
    {
        return _tier1Engine;
    }
    public int getTier2EngineCount()
    {
        return _tier2Engine;
    }
    public int getTier3EngineCount()
    {
        return _tier3Engine;
    }
    public int getTier1NoseConeCount()
    {
        return _tier1NoseCone;
    }
    public int getTier2NoseConeCount()
    {
        return _tier2NoseCone;
    }
    public int getSeparatorCount()
    {
        return _separator;
    }
    public int getSideSeparatorCount()
    {
        return _sideSeparator;
    }
    public int getSmallFuelTankCount()
    {
        return _smallFuelTank;
    }
    public int getMediumFuelTankCount()
    {
        return _mediumFuelTank;
    }
    public int getLargeFuelTankCount()
    {
        return _largeFuelTank;
    }

    // setters
    public void setTier1CapsuleCount(int val)
    {
        _tier1Capsule = val;
    }
    public void setTier2CapsuleCount(int val)
    {
        _tier2Capsule = val;
    }
    public void setTier3CapsuleCount(int val)
    {
        _tier3Capsule = val;
    }
    public void setTier1EngineCount(int val)
    {
        _tier1Engine = val;
    }
    public void setTier2EngineCount(int val)
    {
        _tier2Engine = val;
    }
    public void setTier3EngineCount(int val)
    {
        _tier3Engine = val;
    }
    public void setTier1NoseConeCount(int val)
    {
        _tier1NoseCone = val;
    }
    public void setTier2NoseConeCount(int val)
    {
        _tier2NoseCone = val;
    }
    public void setSeparatorCount(int val)
    {
        _separator = val;
    }
    public void setSideSeparatorCount(int val)
    {
        _sideSeparator = val;
    }
    public void setSmallFuelTankCount(int val)
    {
        _smallFuelTank = val;
    }
    public void setMediumFuelTankCount(int val)
    {
        _mediumFuelTank = val;
    }
    public void setLargeFuelTankCount(int val)
    {
        _largeFuelTank = val;
    }

}
