using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveEvents : MonoBehaviour
{
    private List<GameObject> eventPrefabs;

    public void AddToEventToList(GameObject button)
    {
        eventPrefabs.Add(button);
    }

    public void RemoveEventFromList(GameObject button)
    {
        eventPrefabs.Remove(button);
    }

    public GameObject[] GetEvents()
    {
        GameObject[] EventPrefabs = eventPrefabs.ToArray();

        return EventPrefabs;
    }
}
