using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTransform : MonoBehaviour
{
    public Transform target;
    [SerializeField] private List<bool> _axesToFollowPosition = new List<bool>(3);
    [SerializeField] private Vector3 _offsetPosition;
    
    void Update()
    {
        var position = transform.position;
        for (int i = 0; i < _axesToFollowPosition.Count; i++)
        {
            var axis = _axesToFollowPosition[i];
            if (axis)
            {
                position[i] = target.position[i];
            }
        }
        transform.position = position + _offsetPosition;
        transform.rotation = target.rotation;
    }
}
