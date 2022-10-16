using System;
using System.Collections;
using System.Collections.Generic;
using Players.Physics_Based_Character_Controller;
using UnityEngine;

public class DynamicSpringStrength : MonoBehaviour
{
    [SerializeField] private float _minRideSpringStrength;
    [SerializeField] private float _maxRideSpringStrength;
    [SerializeField] private float _minRideSpringDamper;
    [SerializeField] private float _maxRideSpringDamper;
    
    private RoadGenerator _roadGenerator;
    private PhysicsBasedCharacterController _physicsBasedCharacterController;
    
    private void Awake()
    {
        _roadGenerator = FindObjectOfType<RoadGenerator>();
        _physicsBasedCharacterController = GetComponent<PhysicsBasedCharacterController>();
    }

    private void Update()
    {
        var t = Mathf.Pow(_roadGenerator.roadSpeed / _roadGenerator.maxRoadSpeed, 2f);
        var rideSpringStrength = Mathf.Lerp(_minRideSpringStrength, _maxRideSpringStrength, t);
        var rideSpringDamper = Mathf.Lerp(_minRideSpringDamper, _maxRideSpringDamper, t);
        _physicsBasedCharacterController._rideSpringStrength = rideSpringStrength;
        _physicsBasedCharacterController._rideSpringDamper = rideSpringDamper;
    }
}
