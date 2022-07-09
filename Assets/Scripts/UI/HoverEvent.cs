using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HoverEvent : MonoBehaviour, IPointerEnterHandler {

    public GameObject Selector;

    public void OnPointerEnter(PointerEventData eventData) {
        Selector.transform.position = new Vector3(Selector.transform.position.x, gameObject.transform.position.y, Selector.transform.position.z);
    }
}
