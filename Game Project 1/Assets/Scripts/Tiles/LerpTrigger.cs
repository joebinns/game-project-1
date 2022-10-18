using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LerpTrigger : MonoBehaviour
{
    [SerializeField] SplineLerp splineLerp;


    private void OnTriggerEnter(Collider other)
    {
        splineLerp.StartLerp();


    }




}
