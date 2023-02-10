using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidField : MonoBehaviour
{
    public Transform asteriodPrefab;
    public int fieldRadius = 100;
    public int asteriodCount = 500;
    // Start is called before the first frame update
    void Start()
    {
        for (int loop = 0; loop < asteriodCount; loop++)
        {
            Vector3 randomSpawn = new Vector3(Random.Range(-300, 301), Random.Range(-200, 201), Random.Range(-100, 3000));

            Transform temp = Instantiate(asteriodPrefab, randomSpawn, Random.rotation);


            //Transform temp = Instantiate(asteriodPrefab, Random.insideUnitSphere * fieldRadius, Random.rotation);
            temp.localScale = temp.localScale * Random.Range(.05f, 5f);
            //temp.transform.parent = gameObject.transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
