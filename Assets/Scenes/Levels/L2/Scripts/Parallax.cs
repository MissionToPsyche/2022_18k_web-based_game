using UnityEngine;

public class Parallax : MonoBehaviour
{ 
    private float length, startPos;
    public GameObject cam;
    public float parallaxSpeed;

    void Start()
    {
        startPos = transform.position.y;
        length = GetComponent<SpriteRenderer>().bounds.size.y;
    }

   void Update(){
        float dist = cam.transform.position.y * parallaxSpeed;
        transform.position = new Vector3(transform.position.x,startPos + dist, transform.position.z);
   }
    
}
