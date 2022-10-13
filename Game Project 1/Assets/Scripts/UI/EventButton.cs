using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EventButton : MonoBehaviour
{
    public GameObject eventPrefab;

    [Space(10)]
    public GameObject fadeImage;

    private ActiveEvents activeEvents;


    bool isSelected;

    private void Awake()
    {
        // Not good but works. we're on a tight deadline alright!!
        // EventGroup - DropDown - Content
        activeEvents = transform.parent.parent.parent.GetComponent<ActiveEvents>();

        Selected();
    }

    public void Selected()
    {
        isSelected = !isSelected;

        fadeImage.SetActive(!isSelected);

        if(isSelected) activeEvents.AddToEventToList(eventPrefab);
        else activeEvents.RemoveEventFromList(eventPrefab);
    }
}
