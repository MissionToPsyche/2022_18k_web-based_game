using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnappingPointGenerator : MonoBehaviour
{
    public bool generateOnTop = false;
    public bool generateOnBottom = false;
    public bool generateOnLeft = false;
    public bool generateOnRight = false;
    public GameObject snappingPointPrefab;
    private List<GameObject> _instantiatedSnappingPointObjs = new List<GameObject>();
    private Vector3 _snappingPointPosition;
    private RocketPart _rocketPartScript;
    // Start is called before the first frame update
    void Start()
    {
        _rocketPartScript = gameObject.GetComponent<RocketPart>();
        SpriteRenderer parentRocketPartSpriteRenderer = transform.GetComponent<SpriteRenderer>();
        _snappingPointPosition = new Vector3(0, 0, 0);
        if (generateOnTop)
        {
            _snappingPointPosition = new Vector3(transform.localPosition.x, transform.localPosition.y + parentRocketPartSpriteRenderer.bounds.size.y / 2);
            GenerateSnappingPoint("top");
        }
        if (generateOnBottom)
        {
            _snappingPointPosition = new Vector3(transform.localPosition.x, transform.localPosition.y - parentRocketPartSpriteRenderer.bounds.size.y / 2);
            GenerateSnappingPoint("bottom");
        }
        // If left or right, they start of disabled unless it is a side separator
        if (generateOnLeft)
        {
            _snappingPointPosition = new Vector3(transform.localPosition.x - parentRocketPartSpriteRenderer.bounds.size.x / 2, transform.localPosition.y);
            GenerateSnappingPoint("left", false);
        }
        if (generateOnRight)
        {
            _snappingPointPosition = new Vector3(transform.localPosition.x + parentRocketPartSpriteRenderer.bounds.size.x / 2, transform.localPosition.y);
            GenerateSnappingPoint("right", false);
        }
    }
    void GenerateSnappingPoint(string direction, bool isEnabled = true)
    {
        GameObject _instantiatedSnappingPointObj = Instantiate(snappingPointPrefab, Vector3.zero, Quaternion.identity);
        // Assign the snapping point to the rocket part object as a reference
        if (direction == "top")
        {
            _rocketPartScript.snappingPointOnTop = _instantiatedSnappingPointObj;
        }
        else if (direction == "bottom")
        {
            _rocketPartScript.snappingPointOnBottom = _instantiatedSnappingPointObj;
        }
        else if (direction == "left")
        {
            _rocketPartScript.snappingPointOnLeft = _instantiatedSnappingPointObj;
        }
        else if (direction == "right")
        {
            _rocketPartScript.snappingPointOnRight = _instantiatedSnappingPointObj;
        }

        SnappingPoint instantiatedSnappingPointObjScript = _instantiatedSnappingPointObj.GetComponent<SnappingPoint>();
        instantiatedSnappingPointObjScript.direction = direction;
        if (gameObject.tag == "SideSeparator")
        {
            _instantiatedSnappingPointObj.SetActive(true);
        }
        else
        {
            _instantiatedSnappingPointObj.SetActive(isEnabled);
        }
        _instantiatedSnappingPointObjs.Add(_instantiatedSnappingPointObj);
        SnapManager.instance.AddSnappingPointObj(_instantiatedSnappingPointObj);
        _instantiatedSnappingPointObj.transform.SetParent(transform.parent.transform);
        _instantiatedSnappingPointObj.transform.localPosition = _snappingPointPosition;
        Invoke("AssignParent", 0.01f);
    }

    void AssignParent()
    {
        foreach (GameObject _instantiatedSnappingPointObj in _instantiatedSnappingPointObjs)
        {
            _instantiatedSnappingPointObj.transform.SetParent(transform);
        }
    }
}
