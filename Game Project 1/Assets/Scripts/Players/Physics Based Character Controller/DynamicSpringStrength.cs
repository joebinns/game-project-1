using System;
using System.Collections;
using System.Collections.Generic;
using Players.Physics_Based_Character_Controller;
using UnityEngine;

public class DynamicSpringStrength : MonoBehaviour
{
    public bool ShouldSpringBeStiff = false;
    
    [SerializeField] private float _minRideSpringStrength;
    [SerializeField] private float _maxRideSpringStrength;
    [SerializeField] private float _minRideSpringDamper;
    [SerializeField] private float _maxRideSpringDamper;
    
    private RoadGenerator _roadGenerator;
    private SpeedSelector _speedSelector;
    private PhysicsBasedCharacterController _physicsBasedCharacterController;
    
    private void Awake()
    {
        _roadGenerator = FindObjectOfType<RoadGenerator>();
        _speedSelector = FindObjectOfType<SpeedSelector>();
        _physicsBasedCharacterController = GetComponent<PhysicsBasedCharacterController>();
    }

    private void Update()
    {
        if (ShouldSpringBeStiff)
        {
            _physicsBasedCharacterController._rideSpringStrength = _maxRideSpringStrength;
            _physicsBasedCharacterController._rideSpringDamper = _maxRideSpringDamper;
            return;
        }

        var t = Mathf.Pow(_roadGenerator.roadSpeed / _speedSelector.PlayerCollisionMaxCalibratedSpeed, 2f);
        var springStrength = Mathf.Lerp(_minRideSpringStrength, _maxRideSpringStrength, t);
        var springDamper = Mathf.Lerp(_minRideSpringDamper, _maxRideSpringDamper, t);
        _physicsBasedCharacterController._rideSpringStrength = springStrength;
        _physicsBasedCharacterController._rideSpringDamper = springDamper;
    }
}
