using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFocusPosition : MonoBehaviour
{
    [SerializeField] private List<Transform> _transforms;
    [SerializeField] private List<bool> _averagedAxes = new List<bool>(3);

    public Transform FocusPosition;

    private void Update()
    {
        CalculateAverageFocusPosition();
    }

    private void CalculateAverageFocusPosition()
    {
        var focusPosition = Vector3.zero;
        foreach (Transform transform in _transforms)
        {
            for (int i = 0; i < 3; i++)
            {
                var axis = _averagedAxes[i];
                if (axis)
                {
                    focusPosition[i] += transform.position[i];
                }
            }
        }
        focusPosition /= _transforms.Count;
        FocusPosition.position = focusPosition;
    }
}
