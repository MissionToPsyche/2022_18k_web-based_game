using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public float speed = 3f;
    public Transform[] points;

    private int currentIndex;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = points[0].position;
    }

    // Update is called once per frame
    void Update()
    {
        float distanceFromPlatformToNextPoint = Vector2.Distance(transform.position, points[currentIndex].position);
        bool hasReachedNextPoint = distanceFromPlatformToNextPoint < 0.02f;
        if (hasReachedNextPoint)
        {
            currentIndex++;
        }

        // reset to zero if this is the last index
        currentIndex %= points.Length;

        transform.position = Vector2.MoveTowards(transform.position, points[currentIndex].position, speed * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(transform);
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && collision.transform.parent == transform)
        {
            collision.transform.SetParent(null);
        }
    }
}
