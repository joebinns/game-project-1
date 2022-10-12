using UnityEngine;
using Oscillators;

namespace Players
{
    [RequireComponent(typeof(Oscillator))]
    public class HeightBuffer : MonoBehaviour
    {
        private Rigidbody _rb;
        private Oscillator _oscillator;

        [Header("Hover")]
        //[SerializeField] private float _desiredHeight = 2f;
        [SerializeField] private float _maxRaycastDist = 3f;

        private RaycastHit _rayHit;

        [Header("Jump")]
        [SerializeField] private float _jumpForceFactor = 15f;
        [SerializeField] private float _approxJumpDuration = 0.5f;

        private float _timeSinceJump;

        /// <summary>
        /// Get the rigidbody.
        /// </summary>
        private void Awake()
        {
            _timeSinceJump = _approxJumpDuration;
            _rb = GetComponent<Rigidbody>();
            _oscillator = GetComponent<Oscillator>();
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
            //Vector3 displacement = new Vector3(0f, _rayHit.distance - _desiredHeight, 0f);

            /*
            var localEquilibriumPosition = _oscillator.LocalEquilibriumPosition;
            localEquilibriumPosition.y = transform.InverseTransformPoint(_rayHit.point).y + _desiredHeight;
            Debug.Log(localEquilibriumPosition);
            _oscillator.LocalEquilibriumPosition = localEquilibriumPosition;
            */

            _oscillator.LocalEquilibriumPosition = _oscillator.LocalEquilibriumPositionDefault +
                                                   transform.InverseTransformPoint(_rayHit.point);

        }

        /// <summary>
        /// Cast a ray downwards, to get the distance from the ground.
        /// </summary>
        private (bool, RaycastHit) RaycastToGround()
        {
            RaycastHit rayHit;
            Ray rayToGround = new Ray(transform.position, Vector3.down);
            bool didRayHit = Physics.Raycast(rayToGround, out rayHit, _maxRaycastDist);
            Debug.DrawLine(transform.position, _rayHit.point, Color.green);
            return (didRayHit, rayHit);
        }

        /*
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
        */
    }
}