using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OreGeneration : MonoBehaviour
{
    public Transform goldPrefab;
    public Transform ironPrefab;
    public Transform nickelPrefab;
    public Transform icePrefab;
    public int fieldRadius = 100;
    public int oreCount = 50;
    // Start is called before the first frame update

    void Start()
    {
        RaycastHit raycastHit;
        Transform temp;

        for (int loop = 0; loop < oreCount; loop++)
        {
            Vector3 randomGold = new Vector3(Random.Range(0, 1000), 0, Random.Range(0, 1000));
            Vector3 randomIron = new Vector3(Random.Range(0, 1000), 0, Random.Range(0, 1000));
            Vector3 randomNickel = new Vector3(Random.Range(0, 1000), 0, Random.Range(0, 1000));
            Vector3 randomIce = new Vector3(Random.Range(0, 1000), 0, Random.Range(0, 1000));

            if (Physics.Raycast(randomGold + new Vector3(0, 100.0f, 0), Vector3.down, out raycastHit, 300.0f))
            {
                temp = Instantiate(goldPrefab, raycastHit.point, Random.rotation);
                temp.localScale = temp.localScale * Random.Range(.05f, 1f);
            }

            if (Physics.Raycast(randomIron + new Vector3(0, 100.0f, 0), Vector3.down, out raycastHit, 300.0f))
            {
                temp = Instantiate(ironPrefab, raycastHit.point, Random.rotation);
                temp.localScale = temp.localScale * Random.Range(.05f, 1f);
            }

            if (Physics.Raycast(randomNickel + new Vector3(0, 100.0f, 0), Vector3.down, out raycastHit, 300.0f))
            {
                temp = Instantiate(nickelPrefab, raycastHit.point, Random.rotation);
                temp.localScale = temp.localScale * Random.Range(.05f, 1f);
            }

            if (Physics.Raycast(randomIce + new Vector3(0, 100.0f, 0), Vector3.down, out raycastHit, 300.0f))
            {
                temp = Instantiate(icePrefab, raycastHit.point, Random.rotation);
                temp.localScale = temp.localScale * Random.Range(.05f, 1f);
            }

            //Transform temp = Instantiate(goldPrefab, randomSpawn, Random.rotation);


            //Transform temp = Instantiate(asteriodPrefab, Random.insideUnitSphere * fieldRadius, Random.rotation);
            //temp.transform.parent = gameObject.transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
