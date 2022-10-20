using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utilities;

public class CustomCentreOfMass : MonoBehaviour
{
    private Rigidbody _rb;

    private void Awake()
    {
        _rb = GetComponentInParent<Rigidbody>();
        _rb.centerOfMass = _rb.transform.InverseTransformPoint(transform.position);
    }
}
