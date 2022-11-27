using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickUps : MonoBehaviour
{
    public Transform healthPrefab;
    public int fieldRadius = 100;
    public int count = 60;     
    

    // Start is called before the first frame update
    void Start()
    {
        for (int loop = 0; loop < count; loop++)
        {
            Vector3 randomSpawn = new Vector3(Random.Range(-300, 301), Random.Range(-200, 201), Random.Range(-100, 3000));

            Transform temp = Instantiate(healthPrefab, randomSpawn, Random.rotation);

            //Transform temp = Instantiate(healthPrefab, Random.insideUnitSphere * fieldRadius, Random.rotation);
            temp.localScale = temp.localScale;
            //temp.transform.parent = gameObject.transform;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    
}
