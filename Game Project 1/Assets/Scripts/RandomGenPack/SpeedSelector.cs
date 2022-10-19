using System;
using System.Collections;
using UnityEngine;


[RequireComponent(typeof(RoadGenerator))]
public class SpeedSelector : MonoBehaviour
{ 
    [SerializeProperty("SpeedMode")]
    public SpeedMode _speedMode = SpeedMode.Low;
    public SpeedMode SpeedMode
    {
        get => _speedMode;
        set
        {
            StartCoroutine(TransitionSpeedMode(_speedMode, value));
            _speedMode = value;
        }
    } 
    [SerializeField] private float _transitionDuration;
    
    [HideInInspector] public float PlayerCollisionMaxCalibratedSpeed = 100f;
    private RoadGenerator _roadGenerator;
    private float _t;

    private void Awake()
    {
        _roadGenerator = this.GetComponent<RoadGenerator>();
        StartCoroutine(TransitionSpeedMode(SpeedMode.Stop, _speedMode)); // TODO: Re add this, there is an error with switching speed modes to do with the SpeedVFX.
    }

    private IEnumerator TransitionSpeedMode(SpeedMode a, SpeedMode b)
    {
        _t = 0f;
        while (_t < _transitionDuration)
        {
            _t += Time.deltaTime;
            var speed = Mathf.Lerp((float)(int)a , (float)(int)b, _t * (1f / _transitionDuration));
            _roadGenerator.roadSpeed = speed;
            SpeedVFX.Speed(speed);
            yield return null;
        }
    }
}

public enum SpeedMode
{
    Stop = 0,
    Low = 25,
    Medium = 33,
    High = 40,
    GodSpeed = 200
}
