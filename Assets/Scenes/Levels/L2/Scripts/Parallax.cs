using UnityEngine;

public class Parallax : MonoBehaviour
{
    public GameObject cam;
    public float parallaxEffect;
    public bool autoScroll = false;

    private float width, startPosX, startPosY;
    private float lastCamPosX;

    // Start is called before the first frame update
    void Start()
    {
        startPosX = transform.position.x;
        startPosY = transform.position.y;
        width = GetComponent<SpriteRenderer>().bounds.size.x;
        lastCamPosX = cam.transform.position.x;
    }

    // Update is called once per frame
    void Update()
    {
        // Vertical parallax
        float tempY = cam.transform.position.y * (1 - parallaxEffect);
        float distanceY = (cam.transform.position.y * parallaxEffect);
        float desiredYPos = startPosY + distanceY;

        transform.position = new Vector2(startPosX, desiredYPos);

        if (autoScroll)
        {
            if (cam.transform.position.x > startPosX + width)
            {
                startPosX += width * 2;
            }
            else if (cam.transform.position.x < startPosX - width)
            {
                startPosX -= width * 2;
            }
        }
    }
}
