using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class Movement
{
    
    [Test]
    public void ForwardSimplePasses()
    {
        var ship = new GameObject().AddComponent<ShipControls>();
        ship.forwardspeed = 25f;
        Assert.AreEqual(ship.forwardspeed, ship.transform.position.x, 25.0f);
    }
    [Test]
    public void StrafeSimplePasses()
    {
        var ship = new GameObject().AddComponent<ShipControls>();
        ship.strafespeed = 7f;
        Assert.AreEqual(ship.strafespeed, ship.transform.position.x, 7.0f);
    }
    [Test]
    public void HoverSimplePasses()
    {
        var ship = new GameObject().AddComponent<ShipControls>();
        ship.hoverspeed = 5f;
        Assert.AreEqual(ship.hoverspeed, ship.transform.position.x, 5.0f);
    }
    [Test]
    public void RollSimplePasses()
    {
        var ship = new GameObject().AddComponent<ShipControls>();
        ship.rollspeed = 90f;
        Assert.AreEqual(ship.rollspeed, ship.transform.position.x, 90.0f);
    }


}
