using Managers.Camera;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class StraightenRoad : MonoBehaviour
{

    [SerializeField]
    private Material[] mats;


    private bool isStraight = false;
    private bool isWorking = false;

    public static StraightenRoad Instance { get; private set; }
    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.

        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;
        foreach (var mat in mats)
            mat.SetFloat("_Wonk", 0);
    }
    public void Straighten(float duration)
    {

        if (isStraight) return;
        isStraight = true;
        if (isWorking) return;

        StartCoroutine(Lerp(new Vector3(0,1,duration)));
    }

    public void Curve(float duration)
    {
        if (!isStraight) return;
        isStraight = false;
        if (isWorking) return;
        StartCoroutine(Lerp(new Vector3(1,0, duration)));
    }

    IEnumerator Lerp(Vector3 lerp)
    {
        isWorking = true;
        float t = 0;
        while (t < lerp.z)
        {
            t += Time.deltaTime;

            foreach (var mat in mats)
                mat.SetFloat("_Wonk", Mathf.Lerp(lerp.x, lerp.y, t / lerp.z));

            yield return null;

        }
        isWorking = false;

    }
}
