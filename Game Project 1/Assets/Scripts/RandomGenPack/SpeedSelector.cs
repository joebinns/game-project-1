using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedSelector : MonoBehaviour
{
    public float StartSpeed;
    public float StopSpeed;
    public float AbsoluteMaxSpeed = 100f;
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
        _roadGenerator.roadSpeed = Mathf.Lerp(StartSpeed , StopSpeed, _t * _rateOfIncrease);
    }
}
