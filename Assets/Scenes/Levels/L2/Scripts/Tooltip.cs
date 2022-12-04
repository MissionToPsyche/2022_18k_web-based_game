using TMPro;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode()]
public class Tooltip : MonoBehaviour
{
    public TextMeshProUGUI headerText;
    public TextMeshProUGUI bodyText;
    private LayoutElement _layoutElement;
    private RectTransform _rectTransform;
    public IntegratedSubsystem characterWrapLimit;
    void Awake()
    {
        _layoutElement = this.GetComponent<LayoutElement>();
        _rectTransform = this.GetComponent<RectTransform>();
    }
    public void SetText(string body, string header = "")
    {
        if (string.IsNullOrEmpty(header))
        {
            headerText.gameObject.SetActive(false);
        }
        else
        {
            headerText.gameObject.SetActive(true);
            headerText.text = header;
        }
        bodyText.text = body;
        int headerLength = headerText.text.Length;
        int bodyLength = bodyText.text.Length;
        _layoutElement.enabled = Mathf.Max(headerText.preferredWidth, bodyText.preferredWidth) >= _layoutElement.preferredWidth;
    }
    // Update is called once per frame
    void Update()
    {
        Vector2 mousePosition = Input.mousePosition;

        float pivotX = mousePosition.x / Screen.width;
        float pivotY = mousePosition.y / Screen.height;

        _rectTransform.pivot = new Vector2(pivotX, pivotY);

        transform.position = mousePosition;
    }
}
