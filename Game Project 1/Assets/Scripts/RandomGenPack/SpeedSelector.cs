using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedSelector : MonoBehaviour
{
    private RoadGenerator _roadGenerator;
    private float _t;

    private void Awake()
    {
        _roadGenerator = this.GetComponent<RoadGenerator>();
    }

    void Update()
    {
        _t += Time.deltaTime;
        _roadGenerator.roadSpeed = Mathf.Lerp(5f ,_roadGenerator.maxRoadSpeed , _t * 0.02f);
    }
}
