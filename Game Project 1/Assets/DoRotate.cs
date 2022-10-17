using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DoRotate : MonoBehaviour
{
    [SerializeField] private bool spinZAxis;

    private void Start()
    {
        DOTween.Init();
    }

    private void Update()
    {
        if (spinZAxis)
        {
            transform.DORotate(transform.rotation.eulerAngles + new Vector3(0, 0, 10), 1);
        }
    }
}
