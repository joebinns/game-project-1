using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    public Transform target;
    
    void Update()
    {
        target.position = transform.position;
        target.rotation = transform.rotation;
    }
}
