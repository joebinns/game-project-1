using Oscillators;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
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

        public enum MovementOptions { None, Default };
        private bool _grounded = true;

        [Header("Jump:")]
        [SerializeField] private MovementOptions _movementOption = MovementOptions.Default;

        [Header("Tap for Ride Height Jump:")]
        [SerializeField] private float _jumpRideHeight = 4f;
        [SerializeField] private float _transitionDurationJumpRise = 0.25f;
        [SerializeField] private float _transitionDurationJumpFall = 0.15f;
        [Header("Hold for Ride Height Crouch:")]
        [SerializeField] private float _crouchRideHeight = 2f;
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
                grounded = rayHit.distance <= _defaultRideHeight * 1.3f; // 1.3f allows for greater leniancy (as the value will oscillate about the rideHeight).
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
            (bool rayHitGround, RaycastHit rayHit) = RaycastToGround();
            SetPlatform(rayHit);

            _grounded = CheckIfGrounded(rayHitGround, rayHit);
            
            if (rayHitGround && _shouldMaintainHeight)
            {
                MaintainHeight(rayHit);
            }
            
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
            Debug.DrawLine(transform.position, transform.position + (_rayDir * springForce), Color.yellow);

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
            if (context.canceled & _grounded)
            {
                StartCoroutine(TransitionRideHeight(_defaultRideHeight, _jumpRideHeight, _transitionDurationJumpRise));
                StartCoroutine(TransitionRideHeightDelayed(_jumpRideHeight, _defaultRideHeight, _transitionDurationJumpFall,
                    _transitionDurationJumpRise));
            }
        }
        
        public void CrouchInput(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                StartCoroutine(TransitionRideHeight(_defaultRideHeight, _crouchRideHeight, _transitionDurationCrouch));
            }
            if (context.canceled)
            {
                StartCoroutine(TransitionRideHeight(_crouchRideHeight, _defaultRideHeight, _transitionDurationCrouch));
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

        private IEnumerator TransitionRideHeight(float a, float b, float duration)
        {
            var t = 0f;
            GetComponent<DynamicSpringStrength>().ShouldSpringBeStiff = true;
            while (t < duration)
            {
                t += Time.deltaTime;
                _rideHeight = Mathf.Lerp(a , b, t * (1f / duration)); // TODO: Change this to some easing function.
                yield return null;
            }
            _rideHeight = b;
            Debug.Log(_rideHeight);
            GetComponent<DynamicSpringStrength>().ShouldSpringBeStiff = false;
        }
        
        private IEnumerator TransitionRideHeightDelayed(float a, float b, float duration, float delay)
        {
            yield return new WaitForSeconds(delay);
            yield return TransitionRideHeight(a, b, duration);
        }
    }
}
