using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class TooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public string header;
    [Multiline()]
    public string body;
    private Coroutine _delayedShowCoroutine;
    public void OnPointerEnter(PointerEventData eventData)
    {
        _delayedShowCoroutine = StartCoroutine(DelayedShow());

    }
    public void OnPointerExit(PointerEventData eventData)
    {
        hide();
    }
    IEnumerator DelayedShow()
    {
        yield return new WaitForSeconds(0.6f);
        TooltipShower.instance.Show(header, body);
    }

    void OnDisable()
    {
        hide();
    }
    private void hide()
    {
        if (_delayedShowCoroutine != null)
        {
            StopCoroutine(_delayedShowCoroutine);
            TooltipShower.instance.Hide();
        }
    }
}
