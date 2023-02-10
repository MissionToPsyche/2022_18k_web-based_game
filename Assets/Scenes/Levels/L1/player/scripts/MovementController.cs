using InterWorld.Shared.Enums;
using System;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    #region components
    Animator animator;
    Rigidbody2D body;
    private SpriteRenderer spriteRenderer;
    PlayerParticleSystemController playerParticles;
    #endregion

    #region scripts
    PlayerCollision playerCollision;

    #endregion

    #region props
    public float speedMultiplier = 5f;
    public float jumpMultiplier = 2f;
    #endregion

    private Direction direction = Direction.Right;
    private bool isDoubleJumped = false;

    // Start is called before the first frame update
    void Start()
    {
        this.animator = GetComponent<Animator>();
        this.body = GetComponent<Rigidbody2D>();
        this.spriteRenderer = GetComponent<SpriteRenderer>();
        this.playerCollision = GetComponent<PlayerCollision>();
        this.playerParticles = GetComponent<PlayerParticleSystemController>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Jump();
    }

    private void Jump()
    {
        if (Input.GetButtonDown("Jump"))
        {
            //single jump
            if (playerCollision.isGrounded)
            {
                body.velocity += Vector2.up * jumpMultiplier;
                animator.SetBool("isJumping", true);
                playerParticles.CreateDust();
            }

            //double jump
            else if (isDoubleJumped == false)
            {
                body.velocity = new Vector2(body.velocity.x, 12);
                isDoubleJumped = true;
            }

        }

        //reached ground after jump
        else if (playerCollision.isGrounded && body.velocity.y <= 0 && animator.GetBool("isJumping"))
        {
            animator.SetBool("isJumping", false);
            playerParticles.CreateDust();
            isDoubleJumped = false;
        }
    }

    private void Move()
    {
        float xInput = Input.GetAxis("Horizontal");

        var speed = xInput * speedMultiplier;
        body.velocity = new Vector2(speed, body.velocity.y);

        //was left, is going right
        if (xInput > 0.01f && direction == Direction.Left)
        {
            transform.localScale = new Vector3(Math.Abs(transform.localScale.x), transform.localScale.y, 1);
            direction = Direction.Right;
            playerParticles.CreateDust();
        }
        //was right, is going left
        else if (xInput < -0.01f && direction == Direction.Right)
        {
            transform.localScale = new Vector3(-1 * transform.localScale.x, transform.localScale.y, 1);
            direction = Direction.Left;
            playerParticles.CreateDust();
        }

        animator.SetBool("isRunning", speed != 0);
    }
}
