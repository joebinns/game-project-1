using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedSelector : MonoBehaviour
{
    public float MinSpeed;
    public float MaxSpeed;
    [SerializeField] private float _rateOfIncrease;
    
    private RoadGenerator _roadGenerator;
    private float _t;

    private void Awake()
    {
        _roadGenerator = this.GetComponent<RoadGenerator>();
    }

    void Update()
    {
        _t += Time.deltaTime;
        _roadGenerator.roadSpeed = Mathf.Lerp(MinSpeed , MaxSpeed, _t * _rateOfIncrease);
    }
}
