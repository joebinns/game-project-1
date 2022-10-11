using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class followPoint : MonoBehaviour
{
    [SerializeField] private Transform pointToFollow;

    private void Update()
    {
        transform.position = Vector3.Lerp(transform.position, pointToFollow.position, Time.deltaTime * 3f);
    }
}
