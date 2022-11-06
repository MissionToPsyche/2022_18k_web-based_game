using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    public bool isGrounded { get; private set; }
    public bool isOnWall { get; private set; }
    public bool isOnRightWall { get; private set; }
    public bool isOnLeftWall { get; private set; }
    public bool isOnTopWall { get; private set; }

    public LayerMask groundLayer;

    [Header("Collision")]
    public float collisionRadius = 0.1f;
    public Vector2 bottomOffset, topOffset, rightOffset, leftOffset;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = getOverLapCircle((Vector2)transform.position + bottomOffset);
        isOnTopWall = getOverLapCircle((Vector2)transform.position + topOffset);
        isOnRightWall = getOverLapCircle((Vector2)transform.position + rightOffset);
        isOnLeftWall = getOverLapCircle((Vector2)transform.position + leftOffset);
    }

    private Collider2D getOverLapCircle(Vector2 point)
    {
        return Physics2D.OverlapCircle(point, collisionRadius, groundLayer);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere((Vector2)transform.position + bottomOffset, collisionRadius);
        Gizmos.DrawWireSphere((Vector2)transform.position + topOffset, collisionRadius);
        Gizmos.DrawWireSphere((Vector2)transform.position + rightOffset, collisionRadius);
        Gizmos.DrawWireSphere((Vector2)transform.position + leftOffset, collisionRadius);
    }
}
