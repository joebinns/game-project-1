using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ForceNoise : MonoBehaviour
{
    private Rigidbody _rb;
    [SerializeField] private float _forceMagnitude = 10f;
    
    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        _rb.AddForce(_forceMagnitude*Time.fixedDeltaTime,0f, 0f);
    }
}
