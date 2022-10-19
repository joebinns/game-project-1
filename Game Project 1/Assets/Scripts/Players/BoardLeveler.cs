using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardLeveler : MonoBehaviour
{
    [SerializeField] private float _rayToGroundLength = 10f;
    [SerializeField] private float _torqueMagnitude = 3f;
    [SerializeField] private LayerMask _terrainLayer;
    [SerializeField] private Transform _raySourceFront;
    [SerializeField] private Transform _raySourceBack;
    private readonly Vector3 _rayDir = Vector3.down;
    private Rigidbody _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        (bool rayFrontHitGround, RaycastHit rayFrontHit) = RaycastToGround(_raySourceFront.position);
        (bool rayBackHitGround, RaycastHit rayBackHit) = RaycastToGround(_raySourceBack.position);

        if (!rayFrontHitGround | !rayBackHitGround) { return; }

        var deltaDistance = rayFrontHit.distance - rayBackHit.distance;
        var torque = new Vector3(-deltaDistance, 0f, 0f) * _torqueMagnitude;
        _rb.AddTorque(torque, ForceMode.Force);
    }
    
    private (bool, RaycastHit) RaycastToGround(Vector3 sourcePosition)
    {
        RaycastHit rayHit;
        Ray rayToGround = new Ray(sourcePosition, _rayDir);
        bool rayHitGround = Physics.Raycast(rayToGround, out rayHit, _rayToGroundLength, _terrainLayer.value);
        //Debug.DrawRay(transform.position, _rayDir * _rayToGroundLength, Color.blue);
        return (rayHitGround, rayHit);
    }
}
