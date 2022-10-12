using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropDownMenu : MonoBehaviour
{
    GameObject EventGroup;

    RectTransform rectTransform;

    RectTransform contenRect;

    public float closedHeight, openHeight;
    bool state;

    private void Awake()
    {
        EventGroup = transform.GetChild(0).gameObject;
        rectTransform = GetComponent<RectTransform>();
        contenRect = transform.parent.GetComponent<RectTransform>();

    }

    public void Open()
    {
        state = !state;
        rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, state ? openHeight : closedHeight);
        EventGroup.SetActive(state);

        contenRect.sizeDelta = new Vector2(rectTransform.sizeDelta.x, contenRect.sizeDelta.y + (state ? 240 : -240));
    }
}
