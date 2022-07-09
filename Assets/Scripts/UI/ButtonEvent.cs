using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonEvent : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

    public Text TextElement;

    public void OnPointerEnter(PointerEventData eventData) {
        TextElement.color = Color.white;
    }

    public void OnPointerExit(PointerEventData eventData) {
        TextElement.color = Color.black;
    }
}
