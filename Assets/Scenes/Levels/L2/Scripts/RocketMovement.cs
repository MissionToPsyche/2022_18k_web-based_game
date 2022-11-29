using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketMovement : MonoBehaviour
{
    private float _acceleration = 20f;
    private bool _enginesOn = false;
    private List<Transform> rocketParts = new List<Transform>();
    private List<Rigidbody2D> rocketPartRigidbodies = new List<Rigidbody2D>();
    public RocketFollowThis rocketFollowThisScript;
    void Start()
    {
        Init();
    }

    void FixedUpdate()
    {
        // if engines are on, fly
        if (_enginesOn)
        {
            foreach (Transform child in rocketParts)
            {
                Rigidbody2D childRigidbody = child.GetComponent<Rigidbody2D>();
                childRigidbody.AddForce(new Vector2(0, _acceleration));
            }
        }
    }
    void Init()
    {
        _enginesOn = false;
    }
    public void BuildFinished()
    {
        GetReferenceToRocketParts();
        MakeRocketPartsDynamic();
    }
    private void GetReferenceToRocketParts()
    {
        foreach (Transform child in transform)
        {
            if (child.tag != "NotARocketPart")
            {
                rocketParts.Add(child);
                rocketPartRigidbodies.Add(child.GetComponent<Rigidbody2D>());
                rocketFollowThisScript.FindAndFollowCapsule(child);
            }
        }
    }
    public void EnginesOn()
    {
        _enginesOn = true;
    }
    public void EnginesOff()
    {
        _enginesOn = false;
    }
    private void MakeRocketPartsDynamic()
    {
        foreach (Transform child in rocketParts)
        {
            Rigidbody2D childRigidbody = child.GetComponent<Rigidbody2D>();
            childRigidbody.bodyType = RigidbodyType2D.Dynamic;
            childRigidbody.simulated = true;
            childRigidbody.drag = 0.3f;
            childRigidbody.gravityScale = 1f;
        }
    }
}
