using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayFollow : MonoBehaviour
{
    public float stiffness;
    public float clamp;
    public Vector3 offset;
    private Vector3 _storedOffset;
    Transform playerTransform;
    private void Start()
    {
        _storedOffset = offset;
        playerTransform = transform.parent;
        transform.parent = null;
    }

    public Vector3 RestoreOffset()
    {
        offset = _storedOffset;
        return offset;
    }

    private void LateUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, playerTransform.position + offset, Time.deltaTime * stiffness * (Vector3.Distance(transform.position, playerTransform.position) * clamp));
    }
}
