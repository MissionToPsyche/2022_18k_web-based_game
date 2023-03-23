using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Lvl3WinLandShip : MonoBehaviour
{
    public GameObject ship;
    private Collision asteroid;
    private Collider collider;

    public void Setup()
    {
        //collider = GameObject.Find("PsycheAsteriod").GetComponent<BoxCollider>();
        //collider.enabled = !collider.enabled;
        gameObject.SetActive(true);
        ship = GameObject.Find("Ship");
        StartCoroutine(Land());
        Cursor.visible = true;
    }



    public IEnumerator Land()
    {
        bool rotate = true;
        float t = 0f;
        float z = 2.5f;

        float currentPos = ship.transform.position.z;

        while (t >= -90)
        {
            t -= .8f;
            
            
            Vector3 newRotation = new Vector3(t, 0, 0);
            ship.transform.eulerAngles = newRotation;
            
            

            Vector3 newPosition = new Vector3(0, 0, currentPos += z);
            ship.transform.position = newPosition;

            Debug.Log(t);
                 


            yield return null;
        }

        



        yield return new WaitForEndOfFrame();
    }

    private void OnCollisionEnter(Collision collision)
    {
        StopAllCoroutines();
    }

        public void LandButton()
    {
        SceneManager.LoadScene("Lvl4");
    }
}
