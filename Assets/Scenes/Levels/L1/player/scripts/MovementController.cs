using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    #region components
    Animator animator;
    Rigidbody2D body;
    Collider2D playerCollision;
    SpriteRenderer spriteRenderer;

    #endregion
    public float speedMultiplier = 5;
    private Direction direction = Direction.Right;

    // Start is called before the first frame update
    void Start()
    {
        this.animator = GetComponent<Animator>();
        this.body = GetComponent<Rigidbody2D>();
        this.playerCollision = GetComponent<Collider2D>();
        this.spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        float xInput = Input.GetAxis("Horizontal");
        Move(xInput);

    }

    private void Move(float xInput)
    {
        var speed = xInput * speedMultiplier;
        body.velocity = new Vector2(speed, body.velocity.y);

        //is going right
        if (xInput > 0.01f && direction == Direction.Left)
        {
            spriteRenderer.flipX = false;
            direction = Direction.Right;
        }
        //is going left
        else if (xInput < -0.01f && direction == Direction.Right)
        {
           spriteRenderer.flipX = true;
            direction = Direction.Left;
        }

        animator.SetBool("isRunning", speed != 0);
    }

    private void FlipCharacter()
    {
        spriteRenderer.flipX = direction == Direction.Right;
        direction = (Direction)((int)direction * -1);
    }
}

public enum Direction
{
    Right = 1,
    Left = -1
}
