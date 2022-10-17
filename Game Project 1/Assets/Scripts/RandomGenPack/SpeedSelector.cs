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
    }

    private IEnumerator TransitionSpeedMode(SpeedMode a, SpeedMode b)
    {
        _t = 0f;
        while (_t < _transitionDuration)
        {
            _t += Time.deltaTime;
            _t = Mathf.Clamp(_t, 0f, _transitionDuration);
            var speed = Mathf.Lerp((float)(int)a , (float)(int)b, _t * (1f / _transitionDuration));
            _roadGenerator.roadSpeed = speed;
            yield return null;
        }
    }
}

public enum SpeedMode
{
    Stop = 0,
    Low = 20,
    Medium = 40,
    High = 60,
    GodSpeed = 200
}
