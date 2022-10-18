using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LerpTrigger : MonoBehaviour
{
    [SerializeField] SplineLerp startLerp;


    private void OnTriggerEnter(Collider other)
    {
        startLerp = other.GetComponent<SplineLerp>(shouldLerp);


    }




}
