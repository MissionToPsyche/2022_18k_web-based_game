using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UI;
using System.Diagnostics.Tracing;
using UnityEditor;


public class Scanner : MonoBehaviour
{

    [Header("Input")]
    public KeyCode scannerKey = KeyCode.Mouse0; 

    public float scannerDelay;
    public float maxScannerDistance;
    public float growfactor;

    private Vector3 scannerPoint;
    private Vector3 scaleChange;

    public GameObject cam;

    public GameObject scan;
    private GameObject temp;
    public Text scoreSys;
    public GameObject scanUI;
    public bool shipscanned;
    

    // Start is called before the first frame update
    void Start()
    {
        scaleChange = new Vector3(0.1f, .1f, .1f);
        cam = GameObject.Find("Main Camera");
        scan = GameObject.Find("ScannerEffect");
        GameObject parent = GameObject.Find("Canvas");
        scanUI = parent.transform.Find("Scanned").gameObject;
        scoreSys = parent.transform.Find("Scanned").GetComponent<Text>();
        shipscanned = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(scannerKey)) StartScanner();


        if(temp == null)
        {

        }
        else if (temp.transform.localScale.x >= maxScannerDistance)
        {
            temp.SetActive(false);
            //temp = null;
        }
    }

    private void StartScanner()
    {
        RaycastHit hit;

        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, maxScannerDistance))
        {
            scannerPoint = hit.point;

            //if(temp == null)
                Invoke(nameof(ExecuteScanner), scannerDelay);
        }
        
    }

    private void ExecuteScanner()
    {
        temp = Instantiate(scan, scannerPoint, Quaternion.identity);

        temp.transform.localScale = new Vector3(.01f, .01f, 0.01f);

        StartCoroutine(Scanning());

    }

    IEnumerator Scanning()
    {
        float timer = 0;

        while (temp.transform.localScale.x < maxScannerDistance)
        {
            timer += Time.deltaTime;
            temp.transform.localScale += scaleChange * Time.deltaTime * growfactor;
            yield return null;
            
        }

        yield return new WaitForSeconds(2);

    }
    private void OnTriggerEnter(Collider other)
    {
        

        if (other.gameObject.tag == "Ship")
        {
            Debug.Log("collided");

          
            ScoreManager.instance.AddPoints("ship");
            //scoreSys.text = "Ship Scanned +1" + ": Score Total = " + score;
            StartCoroutine(Wait());
            

        }

        if (other.gameObject.tag == "iron")
        {
            Debug.Log("collided");

            ScoreManager.instance.AddPoints("iron");
            StartCoroutine(Wait());
            Destroy(other.gameObject);
        }

        if (other.gameObject.tag == "nickel")
        {
            Debug.Log("collided");

            ScoreManager.instance.AddPoints("nickel");
            StartCoroutine(Wait());
            Destroy(other.gameObject);
        }

        if (other.gameObject.tag == "gold")
        {
            Debug.Log("collided");

            ScoreManager.instance.AddPoints("gold");
            StartCoroutine(Wait());
            Destroy(other.gameObject);
        }

        if (other.gameObject.tag == "Ice")
        {
            Debug.Log("collided");

            ScoreManager.instance.AddPoints("ice");
            StartCoroutine(Wait());
            Destroy(other.gameObject);
        }
    }

    IEnumerator Wait()
    {
        
        scanUI.SetActive(true);
        yield return new WaitForSeconds(1f);
        scanUI.SetActive(false);
    }

    



}
