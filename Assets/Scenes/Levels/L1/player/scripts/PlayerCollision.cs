using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    private Level1AudioManager audioManager;
    private VCameraShake cameraShake;

    public Vector3 lastCheckpoint;

    // Start is called before the first frame update
    void Start()
    {
        audioManager = GetComponent<Level1AudioManager>();
        cameraShake = FindFirstObjectByType<VCameraShake>();
        Debug.Log($"camera shake {cameraShake}");
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = getOverLapCircle((Vector2)transform.position + bottomOffset);
        isOnTopWall = getOverLapCircle((Vector2)transform.position + topOffset);
        isOnRightWall = getOverLapCircle((Vector2)transform.position + rightOffset);
        isOnLeftWall = getOverLapCircle((Vector2)transform.position + leftOffset);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.tag.ToLower())
        {
            case "checkpoint":
                lastCheckpoint = new Vector3(collision.transform.position.x, collision.transform.position.y);
                Destroy(collision.gameObject);
                break;
            case "spikes":
                cameraShake.Shake();
                audioManager.PlayDeath();
                transform.position = lastCheckpoint;
                break;
        }
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
