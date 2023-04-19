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

    public Renderer meshRenderer;
    public Material gold;
    public Material iron;
    public Material nickel;
    public Material ice;

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

    //Attempt to add cooldown to scanner but due to the affect being an intantiation this doesnt really work
    //public float scannerCdTimer;
    //public float scannerCd;


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
        if (GameObject.Find("scanner") == null)
        {
            if (Input.GetKeyDown(scannerKey)) StartScanner();

        }

        if (temp == null)
        {

        }
        else if (temp.transform.localScale.x >= maxScannerDistance)
        {
            temp.SetActive(false);
            //temp = null;
        }


        //if (scannerCdTimer > 0)
        //    scannerCdTimer -= Time.deltaTime;
    }

    private void StartScanner()
    {
        //if (scannerCdTimer > 0) return;

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

        temp.name = "scanner";

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


        //scannerCdTimer = scannerCd;
        yield return new WaitForSeconds(2);

    }

    IEnumerator Dissolve(GameObject other, string type)
    {
        float timer = 0;
        float x = 0;
        if (type == "gold")
        {
            while (gold.GetFloat("_Dissolve") != 1)
            {
                timer += Time.deltaTime;
                gold.SetFloat("_Dissolve", x);
                x += .02f;
                yield return null;

            }
        }
        else if (type == "iron")
        {
            while (iron.GetFloat("_Dissolve") != 1)
            {
                timer += Time.deltaTime;
                iron.SetFloat("_Dissolve", x);
                x += .02f;
                yield return null;

            }
        }
        else if (type == "nickel")
        {
            while (nickel.GetFloat("_Dissolve") != 1)
            {
                timer += Time.deltaTime;
                nickel.SetFloat("_Dissolve", x);
                x += .02f;
                yield return null;

            }
        }
        else if (type == "ice")
        {
            while (ice.GetFloat("_Dissolve") != 1)
            {
                timer += Time.deltaTime;
                ice.SetFloat("_Dissolve", x);
                x += .02f;
                yield return null;

            }
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
            //StartCoroutine(Wait());
            

        }

        if (other.gameObject.tag == "iron")
        {
            Debug.Log("collided");

            meshRenderer = other.GetComponent<Renderer>();
            iron = meshRenderer.material;

            ScoreManager.instance.AddPoints("iron");
            //StartCoroutine(Wait());
            StartCoroutine(Dissolve((other.gameObject), "iron"));
            Destroy(other.gameObject, 2f);
        }

        if (other.gameObject.tag == "nickel")
        {
            Debug.Log("collided");

            meshRenderer = other.GetComponent<Renderer>();
            nickel = meshRenderer.material;

            ScoreManager.instance.AddPoints("nickel");
            //StartCoroutine(Wait());
            StartCoroutine(Dissolve((other.gameObject), "nickel"));
            Destroy(other.gameObject, 2f);
        }

        if (other.gameObject.tag == "gold")
        {
            Debug.Log("collided");

            meshRenderer = other.GetComponent<Renderer>();
            gold = meshRenderer.material;

            ScoreManager.instance.AddPoints("gold");
            //StartCoroutine(Wait());
            StartCoroutine(Dissolve((other.gameObject), "gold"));
            Destroy(other.gameObject, 2f);
        }

        if (other.gameObject.tag == "Ice")
        {
            Debug.Log("collided");

            meshRenderer = other.GetComponent<Renderer>();
            ice = meshRenderer.material;

            ScoreManager.instance.AddPoints("ice");
            //StartCoroutine(Wait());
            StartCoroutine(Dissolve((other.gameObject), "ice"));
            Destroy(other.gameObject, 2f);
        }
    }

    IEnumerator Wait()
    {
        
        scanUI.SetActive(true);
        yield return new WaitForSeconds(.5f);
        scanUI.SetActive(false);
    }

    



}
