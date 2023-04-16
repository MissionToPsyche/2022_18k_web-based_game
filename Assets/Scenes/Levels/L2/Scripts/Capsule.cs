using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Capsule : MonoBehaviour
{
    public GameObject prefab;
    public GameObject instantiatedShip;

    private void Start()
    {
        float prefabSize = 0.05f;
        Vector3 prefabScale = new Vector3(prefabSize, prefabSize, prefabSize);

        // Instantiate the prefab
        instantiatedShip = Instantiate(prefab, transform.position, Quaternion.Euler(0, 0, 0));
        instantiatedShip.SetActive(false);
        instantiatedShip.transform.localScale = prefabScale;

        // Make the prefab a child of this game object
        instantiatedShip.transform.parent = transform;
    }
    private void OnWin()
    {
        Vector3 prefabRotation = new Vector3(-90, 0, 0);
        instantiatedShip.transform.SetParent(null);
        instantiatedShip.transform.rotation = Quaternion.Euler(prefabRotation);
        instantiatedShip.SetActive(true);

        // Play capsule opening animation first
        SendMessageUpwards("OnWinGame", SendMessageOptions.RequireReceiver);
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Lvl2End")
        {
            OnWin();
        }
    }
}
