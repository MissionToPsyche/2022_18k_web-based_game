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

    // Start is called before the first frame update
    void Start()
    {
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
        if (generateOnLeft)
        {
            _snappingPointPosition = new Vector3(transform.localPosition.x - parentRocketPartSpriteRenderer.bounds.size.x / 2, transform.localPosition.y);
            GenerateSnappingPoint("left");
        }
        if (generateOnRight)
        {
            _snappingPointPosition = new Vector3(transform.localPosition.x + parentRocketPartSpriteRenderer.bounds.size.x / 2, transform.localPosition.y);
            GenerateSnappingPoint("right");
        }

    }
    void GenerateSnappingPoint(string direction)
    {
        GameObject _instantiatedSnappingPointObj = Instantiate(snappingPointPrefab, Vector3.zero, Quaternion.identity);
        _instantiatedSnappingPointObj.GetComponent<SnappingPoint>().direction = direction;
        _instantiatedSnappingPointObjs.Add(_instantiatedSnappingPointObj);
        SnapManager.instance.AddSnappingPointObj(_instantiatedSnappingPointObj);
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
