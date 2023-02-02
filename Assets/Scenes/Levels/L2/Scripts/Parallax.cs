using UnityEngine;

public class Parallax : MonoBehaviour
{
    public GameObject cam;
    private float _startPos;
    public float _parallaxSpeed;

    void Start()
    {
        _startPos = transform.position.y;
    }

    void Update()
    {
        float dist = cam.transform.position.y * _parallaxSpeed;
        transform.position = new Vector3(transform.position.x, _startPos + dist, transform.position.z);
    }

}
