using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayFollow : MonoBehaviour
{
    public float stiffness;
    public float clamp;
    public Vector3 offset;
    Transform playerTransform;
    private void Start()
    {
        playerTransform = transform.parent;
        transform.parent = null;
    }

    private void LateUpdate()
    {
        transform.position =  Vector3.Lerp(transform.position, playerTransform.position + offset , Time.deltaTime * stiffness * (Vector3.Distance(transform.position, playerTransform.position) * clamp));
    }
}
