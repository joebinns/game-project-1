using UnityEngine;
using Oscillators;

namespace Players
{
    [RequireComponent(typeof(Oscillator))]
    public class HeightBuffer : MonoBehaviour
    {
        // TODO: Try moving oscillator to an empty child or something, in order to make local equilibrium position easier?
        // TODO: Fix jump... wth?!

        private Rigidbody _rb;
        private Oscillator _oscillator;

        [Header("Hover")]
        [SerializeField] private float _maxRaycastDist = 3f;

        private RaycastHit _rayHit;
        private Vector3 _gravitationalForce;
        private bool _shouldMaintainHeight = true;

        [Header("Jump")]
        [SerializeField] private float _jumpForceFactor = 15f;
        [SerializeField] private float _riseGravityFactor = 3f;
        [SerializeField] private float _fallGravityFactor = 8f;
        [SerializeField] private float _jumpBuffer = 0.15f; // Note, jumpBuffer shouldn't really exceed the time of the jump.
        [SerializeField] private float _coyoteTime = 0.25f;
        
        private float _timeSinceJumpPressed = 0f;
        private float _timeSinceUngrounded = 0f;
        private float _timeSinceJump = 0f;
        private bool _jumpReady = true;
        private bool _isJumping = false;
        private bool _grounded = true;
        
        
        
        

        /// <summary>
        /// Get the rigidbody.
        /// </summary>
        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
            _oscillator = GetComponent<Oscillator>();
            _gravitationalForce = Physics.gravity * _rb.mass;
        }

        /// <summary>
        /// Hold the rigid body above any underneath surface within range using a spring force.
        /// </summary>
        private void FixedUpdate()
        {
            (bool didRayHit, RaycastHit rayHit) = RaycastToGround();
            _rayHit = rayHit;
            _grounded = CheckIfGrounded(didRayHit, _rayHit);
            if (_grounded)
            {
                _timeSinceUngrounded = 0f;

                if (_timeSinceJump > 0.2f)
                {
                    _isJumping = false;
                }
            }
            else
            {
                _timeSinceUngrounded += Time.fixedDeltaTime;
            }
            
            if (didRayHit && _shouldMaintainHeight)
            {
                //_oscillator.ForceScale = _oscillator.ForceScaleDefault;
                MaintainHeight();
            }
            else
            {
                //_oscillator.ForceScale = Vector3.zero;
            }
            
            Jump();
        }

        private void MaintainHeight()
        {
            _oscillator.LocalEquilibriumPosition = _oscillator.LocalEquilibriumPositionDefault + _rayHit.point;
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

        /// <summary>
        /// Use the result of a Raycast to determine if the capsules distance from the ground is sufficiently close to the desired ride height such that the character can be considered 'grounded'.
        /// </summary>
        /// <param name="rayHitGround">Whether or not the Raycast hit anything.</param>
        /// <param name="rayHit">Information about the ray.</param>
        /// <returns>Whether or not the player is considered grounded.</returns>
        private bool CheckIfGrounded(bool rayHitGround, RaycastHit rayHit)
        {
            bool grounded;
            if (rayHitGround == true)
            {
                grounded = rayHit.distance <= _oscillator.LocalEquilibriumPositionDefault.y * 1.3f; // 1.3f allows for greater leniancy (as the value will oscillate about the rideHeight).
            }
            else
            {
                grounded = false;
            }
            return grounded;
        }

        public void JumpPressed()
        {
            _timeSinceJumpPressed = 0f;
        }

        private void Jump()
        {
            _timeSinceJumpPressed += Time.fixedDeltaTime;
            _timeSinceJump += Time.fixedDeltaTime;
            
            if (_rb.velocity.y < 0)
            {
                _shouldMaintainHeight = true;
                _jumpReady = true;
                if (!_grounded)
                {
                    // Increase downforce for a sudden plummet.
                    _rb.AddForce(_gravitationalForce * (_fallGravityFactor - 1f)); // Hmm... this feels a bit weird. I want a reactive jump, but I don't want it to dive all the time...
                }
            }
            else if (_rb.velocity.y > 0)
            {
                if (!_grounded)
                {
                    if (_isJumping)
                    {
                        _rb.AddForce(_gravitationalForce * (_riseGravityFactor - 1f));
                    }
                }
            }

            if (_timeSinceJumpPressed < _jumpBuffer)
            {
                if (_timeSinceUngrounded < _coyoteTime)
                {
                    if (_jumpReady)
                    {
                        _jumpReady = false;
                        _shouldMaintainHeight = false;
                        _isJumping = true;
                        _rb.velocity =
                            new Vector3(_rb.velocity.x, 0f,
                                _rb.velocity.z); // Cheat fix... (see comment below when adding force to rigidbody).
                        if (_rayHit.distance != 0) // i.e. if the ray has hit
                        {
                            _rb.position = new Vector3(_rb.position.x,
                                _rb.position.y - (_rayHit.distance - _oscillator.LocalEquilibriumPositionDefault.y),
                                _rb.position.z);
                        }

                        _rb.AddForce(Vector3.up * _jumpForceFactor,
                            ForceMode
                                .Impulse); // This does not work very consistently... Jump height is affected by initial y velocity and y position relative to RideHeight... Want to adopt a fancier approach (more like PlayerMovement). A cheat fix to ensure consistency has been issued above...
                        _timeSinceJumpPressed = _jumpBuffer;
                        _timeSinceJump = 0f;
                    }
                }
            }
        }
    }
}