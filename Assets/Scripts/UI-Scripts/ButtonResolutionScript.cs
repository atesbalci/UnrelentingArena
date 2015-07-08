using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class ButtonResolutionScript : MonoBehaviour, IPointerClickHandler {
    public ResolutionsScript resolution;

    public void OnPointerClick(PointerEventData eventData) {
        if (eventData.button == PointerEventData.InputButton.Left)
            resolution.Switch(true);
        else if (eventData.button == PointerEventData.InputButton.Right)
            resolution.Switch(false);
    }
}
