using Oscillators;
using UnityEngine;
using UnityEngine.InputSystem;
using Utilities;

namespace Players.Physics_Based_Character_Controller
{
    /// <summary>
    /// A floating-capsule oriented physics based character controller. Based on the approach devised by Toyful Games for Very Very Valet.
    /// </summary>
    public class PhysicsBasedCharacterController : MonoBehaviour
    {
        private Rigidbody _rb;
        private Vector3 _gravitationalForce;
        private readonly Vector3 _rayDir = Vector3.down;
        private Vector3 _previousVelocity = Vector3.zero;
        private Vector2 _moveContext;

        [Header("Other:")]
        [SerializeField] private bool _adjustInputsToCameraAngle = false;
        [SerializeField] private LayerMask _terrainLayer;

        private float _rideHeight; // rideHeight: desired distance to ground (Note, this is distance from the original raycast position (currently centre of transform)).
        private float _rayToGroundLength; // rayToGroundLength: max distance of raycast to ground (Note, this should be greater than the rideHeight).
        private bool _shouldMaintainHeight = true;

        [Header("Height Spring:")]
        [SerializeField] private float _defaultRideHeight = 1.75f;
        [SerializeField] private float _defaultRayToGroundLength = 3f; 
        [SerializeField] public float _rideSpringStrength = 50f; // rideSpringStrength: strength of spring. (?)
        [SerializeField] public float _rideSpringDamper = 5f; // rideSpringDampener: dampener of spring. (?)
        [SerializeField] private Oscillator _squashAndStretchOcillator;

        
        public enum LookDirectionOptions { Velocity, Acceleration, MoveInput, Aiming };
        private Quaternion _uprightTargetRot = Quaternion.identity; // Adjust y value to match the desired direction to face.
        private Vector3 _lastYLookAt;
        private Quaternion _lastTargetRot;
        private Vector3 _platformInitRot;
        private bool didLastRayHit;

        [Header("Upright Spring: (NOTE this is somewhat obsolete now. Check MovementNoise.cs)")]
        [SerializeField] public LookDirectionOptions _characterLookDirection = LookDirectionOptions.Velocity;
        [SerializeField] private float _uprightSpringStrength = 40f;
        [SerializeField] private float _uprightSpringDamper = 5f;

        private Vector3 _moveInput;
        private float _speedFactor = 1f;
        private float _maxAccelForceFactor = 1f;

        public enum MovementOptions { None, Default };
        private Vector3 _jumpInput;
        private float _timeSinceJumpPressed = 0f;
        private float _timeSinceJumpReleased = 0f;
        private float _timeSinceCrouchPressed = 0f;
        private float _timeSinceCrouchReleased = 0f;
        private float _timeSinceUngrounded = 0f;
        private float _timeSinceJump = 0f;
        private bool _jumpReady = true;
        private bool _isJumping = false;

        [Header("Jump:")]
        [SerializeField] private MovementOptions _movementOption = MovementOptions.Default;
        [Header("Hold for High Jump:")]
        [SerializeField] private float _jumpForceFactor = 10f;
        [SerializeField] private float _riseGravityFactor = 5f;
        [SerializeField] private float _fallGravityFactor = 10f; // typically > 1f (i.e. 5f).
        [SerializeField] private float _jumpBuffer = 0.15f; // Note, jumpBuffer shouldn't really exceed the time of the jump.
        [SerializeField] private float _coyoteTime = 0.25f;
        [Header("Hold for Ride Height Crouch:")]
        [SerializeField] private float _rideHeightCrouch = 3f;
        [SerializeField] private float _transitionDurationCrouch = 0.25f;
        [Header("IK")]
        [SerializeField] private DelayFollow delayFollow;

        private void Awake()
        {
            _rideHeight = _defaultRideHeight;
            _rayToGroundLength = _defaultRayToGroundLength;
        }

        /// <summary>
        /// Prepare frequently used variables.
        /// </summary>
        private void Start()
        {
            _rb = GetComponent<Rigidbody>();
            _gravitationalForce = Physics.gravity * _rb.mass;
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
                grounded = rayHit.distance <= _rideHeight * 1.3f; // 1.3f allows for greater leniancy (as the value will oscillate about the rideHeight).
            }
            else
            {
                grounded = false;
            }
            return grounded;
        }
        
        
        /// <summary>
        /// Determines and plays the appropriate character sounds, particle effects, then calls the appropriate methods to move and float the character.
        /// </summary>
        private void FixedUpdate()
        {
            _moveInput = new Vector3(_moveContext.x, 0, _moveContext.y);

            if (_adjustInputsToCameraAngle)
            {
                _moveInput = AdjustInputToFaceCamera(_moveInput);
            }

            (bool rayHitGround, RaycastHit rayHit) = RaycastToGround();
            SetPlatform(rayHit);

            bool grounded = CheckIfGrounded(rayHitGround, rayHit);
            if (grounded)
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
            
            _timeSinceJump += Time.fixedDeltaTime;
            _timeSinceJumpPressed += Time.fixedDeltaTime;
            _timeSinceJumpReleased += Time.fixedDeltaTime;
            _timeSinceCrouchPressed += Time.fixedDeltaTime;
            _timeSinceCrouchReleased += Time.fixedDeltaTime;
            CharacterJump(grounded, rayHit); 
            if (_movementOption == MovementOptions.Default)
            {
                RideHeightCrouch(_jumpInput);
            }

            if (rayHitGround && _shouldMaintainHeight)
            {
                MaintainHeight(rayHit);
            }

            //Vector3 lookDirection = GetLookDirection(_characterLookDirection);
            var lookDirection = Vector3.forward;
            MaintainUpright(lookDirection, rayHit);
        }

        /// <summary>
        /// Perfom raycast towards the ground.
        /// </summary>
        /// <returns>Whether the ray hit the ground, and information about the ray.</returns>
        private (bool, RaycastHit) RaycastToGround()
        {
            RaycastHit rayHit;
            Ray rayToGround = new Ray(transform.position, _rayDir);
            bool rayHitGround = Physics.Raycast(rayToGround, out rayHit, _rayToGroundLength, _terrainLayer.value);
            //Debug.DrawRay(transform.position, _rayDir * _rayToGroundLength, Color.blue);
            return (rayHitGround, rayHit);
        }

        /// <summary>
        /// Determines the relative velocity of the character to the ground beneath,
        /// Calculates and applies the oscillator force to bring the character towards the desired ride height.
        /// Additionally applies the oscillator force to the squash and stretch oscillator, and any object beneath.
        /// </summary>
        /// <param name="rayHit">Information about the RaycastToGround.</param>
        private void MaintainHeight(RaycastHit rayHit)
        {
            Vector3 vel = _rb.velocity;
            Vector3 otherVel = Vector3.zero;
            Rigidbody hitBody = rayHit.rigidbody;
            if (hitBody != null)
            {
                otherVel = hitBody.velocity;
            }
            float rayDirVel = Vector3.Dot(_rayDir, vel);
            float otherDirVel = Vector3.Dot(_rayDir, otherVel);

            float relVel = rayDirVel - otherDirVel;
            float currHeight = rayHit.distance - _rideHeight;
            float springForce = (currHeight * _rideSpringStrength) - (relVel * _rideSpringDamper);
            Vector3 maintainHeightForce = - _gravitationalForce + springForce * Vector3.down;
            Vector3 oscillationForce = springForce * Vector3.down;
            _rb.AddForce(maintainHeightForce);
            _squashAndStretchOcillator.ApplyForce(oscillationForce);
            //Debug.DrawLine(transform.position, transform.position + (_rayDir * springForce), Color.yellow);

            // Apply force to objects beneath
            if (hitBody != null)
            {
                hitBody.AddForceAtPosition(-maintainHeightForce, rayHit.point);
            }
        }

        /// <summary>
        /// Determines the desired y rotation for the character, with account for platform rotation.
        /// </summary>
        /// <param name="yLookAt">The input look rotation.</param>
        /// <param name="rayHit">The rayHit towards the platform.</param>
        private void CalculateTargetRotation(Vector3 yLookAt, RaycastHit rayHit = new RaycastHit())
        {
            if (didLastRayHit)
            {
                _lastTargetRot = _uprightTargetRot;
                try
                {
                    _platformInitRot = transform.parent.rotation.eulerAngles;
                }
                catch
                {
                    _platformInitRot = Vector3.zero;
                }
            }
            if (rayHit.rigidbody == null)
            {
                didLastRayHit = true;
            }
            else
            {
                didLastRayHit = false;
            }

            if (yLookAt != Vector3.zero)
            {
                if (_characterLookDirection == LookDirectionOptions.Velocity) // Get the last velocity's lookAt, to use as an offset whilst aiming
                {
                    _lastYLookAt = yLookAt.normalized;
                }
            
                _uprightTargetRot = Quaternion.LookRotation(yLookAt, Vector3.up);
                _lastTargetRot = _uprightTargetRot;
                try
                {
                    _platformInitRot = transform.parent.rotation.eulerAngles;
                }
                catch
                {
                    _platformInitRot = Vector3.zero;
                }
            }
            else
            {
                try
                {
                    Vector3 platformRot = transform.parent.rotation.eulerAngles;
                    Vector3 deltaPlatformRot = platformRot - _platformInitRot;
                    float yAngle = _lastTargetRot.eulerAngles.y + deltaPlatformRot.y;
                    _uprightTargetRot = Quaternion.Euler(new Vector3(0f, yAngle, 0f));
                }
                catch
                {

                }
            }
        }
        
        /// <summary>
        /// Adds torque to the character to keep the character upright, acting as a torsional oscillator (i.e. vertically flipped pendulum).
        /// </summary>
        /// <param name="yLookAt">The input look rotation.</param>
        /// <param name="rayHit">The rayHit towards the platform.</param>
        private void MaintainUpright(Vector3 yLookAt, RaycastHit rayHit = new RaycastHit())
        {
            CalculateTargetRotation(yLookAt, rayHit);

            if (_characterLookDirection == LookDirectionOptions.Aiming)
            {
                // Whilst aiming, take direct control over the rotation.
                transform.rotation = _uprightTargetRot;
                return;
            }

            Quaternion currentRot = transform.rotation;
            Quaternion toGoal = MathsUtils.ShortestRotation(_uprightTargetRot, currentRot);

            Vector3 rotAxis;
            float rotDegrees;

            toGoal.ToAngleAxis(out rotDegrees, out rotAxis);
            rotAxis.Normalize();

            float rotRadians = rotDegrees * Mathf.Deg2Rad;

            _rb.AddTorque((rotAxis * (rotRadians * _uprightSpringStrength)) - (_rb.angularVelocity * _uprightSpringDamper));
        
        
        }

        /// <summary>
        /// Adjusts the input, so that the movement matches input regardless of camera rotation.
        /// </summary>
        /// <param name="moveInput">The player movement input.</param>
        /// <returns>The camera corrected movement input.</returns>
        private Vector3 AdjustInputToFaceCamera(Vector3 moveInput)
        {
            float facing = UnityEngine.Camera.main.transform.eulerAngles.y;
            return (Quaternion.Euler(0, facing, 0) * moveInput);
        }

        /// <summary>
        /// Set the transform parent to be the result of RaycastToGround.
        /// If the raycast didn't hit, then unset the transform parent.
        /// </summary>
        /// <param name="rayHit">The rayHit towards the platform.</param>
        private void SetPlatform(RaycastHit rayHit)
        {
            try
            {
                RigidPlatform rigidPlatform = rayHit.transform.GetComponent<RigidPlatform>();
                RigidParent rigidParent = rigidPlatform.rigidParent;
                transform.SetParent(rigidParent.transform);
            }
            catch
            {
                transform.SetParent(null);
            }
        }

        public void JumpInput(InputAction.CallbackContext context)
        {
            _timeSinceJumpPressed = 0f;
            _timeSinceJumpReleased = 0f;
        }
        
        public void CrouchInput(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                _timeSinceCrouchPressed = 0f;
                _jumpInput = new Vector3(0f, 1f, 0f);
            }
            if (context.canceled)
            {
                _timeSinceCrouchReleased = 0f;
                _jumpInput = new Vector3(0f, 0f, 0f);
            }
        }

        public void SetMovementOption(MovementOptions movementOption)
        {
            _movementOption = movementOption;
        }

        public void ResetMovementOption()
        { 
            SetMovementOption(MovementOptions.Default);
        }

        /// <summary>
        /// Apply force to cause the character to perform a single jump, including coyote time and a jump input buffer.
        /// </summary>
        /// <param name="jumpInput">The player jump input.</param>
        /// <param name="grounded">Whether or not the player is considered grounded.</param>
        /// <param name="rayHit">The rayHit towards the platform.</param>
        private void CharacterJump(bool grounded, RaycastHit rayHit)
        {
            if (_rb.velocity.y < 0)
            {
                _shouldMaintainHeight = true;
                _jumpReady = true;
                if (!grounded)
                {
                    // Increase downforce for a sudden plummet.
                    _rb.AddForce(_gravitationalForce * (_fallGravityFactor - 1f)); // Hmm... this feels a bit weird. I want a reactive jump, but I don't want it to dive all the time...
                }
            }
            else if (_rb.velocity.y > 0)
            {
                if (!grounded)
                {
                    if (_isJumping)
                    {
                        _rb.AddForce(_gravitationalForce * (_riseGravityFactor - 1f));
                    }
                }
            }

            if (_movementOption == MovementOptions.None)
            {
                return;
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
                        _rb.velocity = new Vector3(_rb.velocity.x, 0f, _rb.velocity.z); // Cheat fix... (see comment below when adding force to rigidbody).
                        if (rayHit.distance != 0) // i.e. if the ray has hit
                        {
                            _rb.position = new Vector3(_rb.position.x, _rb.position.y - (rayHit.distance - _rideHeight), _rb.position.z);
                        }
                        _rb.AddForce(Vector3.up * _jumpForceFactor, ForceMode.Impulse); // This does not work very consistently... Jump height is affected by initial y velocity and y position relative to RideHeight... Want to adopt a fancier approach (more like PlayerMovement). A cheat fix to ensure consistency has been issued above...
                        _timeSinceJumpPressed = _jumpBuffer; // So as to not activate further jumps, in the case that the player lands before the jump timer surpasses the buffer.
                        _timeSinceJump = 0f;
                    }
                }
            }
        }

        private void ChangeRideHeight(float input, float alternativeRideHeight, float duration)
        {
            var rideHeight = (input * _rideHeight) + ((1 - input) * _defaultRideHeight);
            float t;
            if (_timeSinceCrouchPressed < _timeSinceCrouchReleased)
            {
                t = _timeSinceCrouchPressed;
            }
            else
            {
                t = duration - _timeSinceCrouchReleased;
            }
            rideHeight = Mathf.Lerp(_defaultRideHeight, alternativeRideHeight, t / duration); // TODO: Change this to some easing function, if we stick with this type of jump.
            _rideHeight = rideHeight;
        }

        private void RideHeightCrouch(Vector3 jumpInput)
        {
            ChangeRideHeight(jumpInput.y, _rideHeightCrouch, _transitionDurationCrouch);
        }
    }
}
