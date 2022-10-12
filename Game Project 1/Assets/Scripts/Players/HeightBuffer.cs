using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

namespace Players
{
    public class HeightBuffer : MonoBehaviour
    {
        private Rigidbody _rb;

        [Header("Hover")]
        [SerializeField] private float _desiredHeight = 2f;
        [SerializeField] private float _maxRaycastDist = 3f;
        [SerializeField] private float _springStrength = 100f;
        [SerializeField] private float _springDamper = 10f;

        private RaycastHit _rayHit;

        [Header("Jump")]
        [SerializeField] private float _jumpForceFactor = 15f;
        [SerializeField] private float _approxJumpDuration = 0.5f;

        private float _timeSinceJump;

        private void Awake()
        {
            _timeSinceJump = _approxJumpDuration;
        }

        /// <summary>
        /// Get the rigidbody.
        /// </summary>
        private void Start()
        {
            _rb = GetComponent<Rigidbody>();
        }

        /// <summary>
        /// Hold the rigid body above any underneath surface within range using a spring force.
        /// </summary>
        private void FixedUpdate()
        {
            _timeSinceJump += Time.fixedDeltaTime;
            (bool didRayHit, RaycastHit rayHit) = RaycastToGround();
            _rayHit = rayHit;
            if (didRayHit == false)
            {
                // then there are no forces to be apply
                return;
            }
            Vector3 displacement = new Vector3(0f, _rayHit.distance - _desiredHeight, 0f);
            Vector3 restorativeForce = DampedHookesLaw(_springStrength, displacement, _springDamper, _rb.velocity);
            Debug.DrawLine(transform.position, transform.position + restorativeForce, Color.red);
            if (_rb.useGravity == true)
            {
                // then negate the force of gravity whilst the spring is above a surface
                Vector3 gravitationalForce = _rb.mass * Physics.gravity;
                restorativeForce -= gravitationalForce;
            }
            _rb.AddForce(restorativeForce);
        }

        /// <summary>
        /// Calculate the restorative force of a spring.
        /// </summary>
        private Vector3 DampedHookesLaw(float springStrength, Vector3 displacement, float springDamper, Vector3 relativeVelocity)
        {
            Vector3 restorativeForce = - springStrength * displacement - springDamper * relativeVelocity;
            return restorativeForce;
        }

        /// <summary>
        /// Cast a ray downwards, to get the distance from the ground.
        /// </summary>
        private (bool, RaycastHit) RaycastToGround()
        {
            RaycastHit rayHit;
            Ray rayToGround = new Ray(transform.position, Vector3.down);
            bool didRayHit = Physics.Raycast(rayToGround, out rayHit, _maxRaycastDist);
            return (didRayHit, rayHit);
        }

        public void Jump()
        {
            while (_timeSinceJump >= _approxJumpDuration)
            {
                _rb.velocity = new Vector3(_rb.velocity.x, 0f, _rb.velocity.z);
                if (_rayHit.distance != 0) // i.e. if the ray has hit
                {
                    _rb.position = new Vector3(_rb.position.x, _rb.position.y - (_rayHit.distance - _desiredHeight),
                        _rb.position.z);
                }
                _rb.AddForce(Vector3.up * _jumpForceFactor, ForceMode.Impulse);
                _timeSinceJump = 0f;
            }
        }
    }
}