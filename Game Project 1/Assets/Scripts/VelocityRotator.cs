using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VelocityRotator : MonoBehaviour
{
    [SerializeField] private bool isFlipped;

    private Rigidbody rb;
    private float angle;
    
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        switch (isFlipped)
        {
            case true:

                
                angle = Mathf.Atan2(-rb.velocity.y, rb.velocity.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
                
                break;
            
            case false:

                
                angle = Mathf.Atan2(rb.velocity.y, rb.velocity.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
                
                break;
        }
        
        
    }
}
