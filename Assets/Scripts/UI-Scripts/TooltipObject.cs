using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class TooltipObject : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
    [TextArea(3, 10)]
    public string text;

    public void OnPointerEnter(PointerEventData eventData) {
        if (text == "")
            return;
        GameManager.instance.tooltip.gameObject.SetActive(true);
        GameManager.instance.tooltip.text.text = text;
    }

    public void OnPointerExit(PointerEventData eventData) {
        GameManager.instance.tooltip.gameObject.SetActive(false);
    }
}
