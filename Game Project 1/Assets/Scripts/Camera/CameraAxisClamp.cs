using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAxisClamp : MonoBehaviour
{
    [SerializeField] private List<bool> _averagedAxes = new List<bool>(3);
    
    private void Update()
    {
        var position = transform.position;
        for (int i = 0; i < 3; i++)
        {
            var axis = _averagedAxes[i];
            if (!axis)
            {
                position[i] = 0f;
            }
        }
        transform.position = position;
    }
}
