using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BetterJump : MonoBehaviour
{
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;

    Rigidbody2D body;

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (body.velocity.y == 0)
        {
            return;
        }

        var baseVelocity = Vector2.up * Physics2D.gravity.y * Time.deltaTime;
        switch (body.velocity.y)
        {
            //moving down
            case < 0:
                body.velocity += baseVelocity * (fallMultiplier - 1);
                break;

            //moving up and jump button is still pressed
            case > 0 when !Input.GetButton("Jump"):
                body.velocity += baseVelocity * (lowJumpMultiplier - 1);
                break;
        }
    }

}
