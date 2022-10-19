using Oscillators;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using Unity.VisualScripting;
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

        //Sound
        private FMOD.Studio.EventInstance PlayJumpSound;
        private FMOD.Studio.EventInstance PlayCrouchSound;
        private FMOD.Studio.EventInstance PlayCrouchStopSound;
        public FMOD.Studio.EventInstance PlayHoverSoundLoop;

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

        private enum MovementStates { None, JumpRise, JumpFall, Crouch};
        private MovementStates _movementState = MovementStates.None;
        private MovementStates MovementState
        {
            get => _movementState;
            set { SetMovementState(value); }
        }

        private bool _grounded = true;
        private RaycastHit _rayHit;
        private bool _isJumping = false;
        private float _jumpTimer;
        private float _initialPositionY;
        private float _finalRisePositionY;

        [Header("Jump:")]
        [SerializeField] private MovementOptions _movementOption = MovementOptions.Default;

        [Header("Tap for Ride Height Jump:")]
        [SerializeField] private AnimationCurve _jumpRiseCurve;
        [SerializeField] private AnimationCurve _jumpFallCurve;
        [Header("Hold for Ride Height Crouch:")]
        [SerializeField] private float _crouchRideHeight = 2f;
        [SerializeField] private float _transitionDurationCrouch = 0.25f;
        [Header("IK")]
        [SerializeField] private DelayFollow delayFollow;

        private void Awake()
        {
            _rideHeight = _defaultRideHeight;
            _rayToGroundLength = _defaultRayToGroundLength;
            PlayJumpSound = FMODUnity.RuntimeManager.CreateInstance("event:/HoverJump");
            PlayCrouchSound = FMODUnity.RuntimeManager.CreateInstance("event:/HoverCrouch");
            PlayCrouchStopSound = FMODUnity.RuntimeManager.CreateInstance("event:/HoverCrouchEnd");
            PlayHoverSoundLoop = FMODUnity.RuntimeManager.CreateInstance("event:/HoverSoundLoop");
        }

        /// <summary>
        /// Prepare frequently used variables.
        /// </summary>
        private void Start()
        {
            PlayHoverSoundLoop.start();
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
                grounded = rayHit.distance <= _defaultRideHeight * 1.2f; // 1.2f allows for greater leniancy (as the value will oscillate about the rideHeight).
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
            _rayHit = rayHit;
            
            if (rayHitGround & _shouldMaintainHeight & !_isJumping)
            {
                MaintainHeight(rayHit);
            }
            
            Jump();
            
            var velocity = _rb.velocity;
            velocity.x = 0f;
            velocity.z = 0f;
            _rb.velocity = velocity;
            
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
            if (_movementOption != MovementOptions.Default) { return; }
            
            if (context.canceled & _grounded & !_isJumping)
            {
                PlayJumpSound.start();
                PlayHoverSoundLoop.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
                MovementState = MovementStates.JumpRise;
            }
        }
        
        public void CrouchInput(InputAction.CallbackContext context)
        {
            Debug.Log(context);
            
            if (context.canceled)
            {
                // Stop crouching
                PlayCrouchStopSound.start();
                PlayCrouchSound.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
                StartCoroutine(TransitionRideHeight(_crouchRideHeight, _defaultRideHeight, _transitionDurationCrouch));
            }
            
            if (_movementOption != MovementOptions.Default) { return; }
            
            if (context.started)
            {
                // Start crouching
                PlayCrouchSound.start();
                StartCoroutine(TransitionRideHeight(_defaultRideHeight, _crouchRideHeight, _transitionDurationCrouch));
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

        private IEnumerator TransitionRideHeight(float a, float b, float duration) // Apply to transform directly (without spring), read from curve.
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
            GetComponent<DynamicSpringStrength>().ShouldSpringBeStiff = false;
        }

        private void SetMovementState(MovementStates movementState)
        {
            switch (movementState)
            {
                case MovementStates.None:
                    break;
                case MovementStates.JumpRise:
                    _jumpTimer = 0f;
                    _initialPositionY = _rayHit.point.y + _defaultRideHeight;
                    break;
                case MovementStates.JumpFall:
                    _jumpTimer = 0f;
                    _finalRisePositionY = _rb.position.y;
                    break;
                case MovementStates.Crouch:
                    break;
            }

            _movementState = movementState;
        }

        private void Jump()
        {
            if (MovementState == MovementStates.JumpRise)
            {
                JumpRise(_initialPositionY);
            }
            else if (MovementState == MovementStates.JumpFall)
            {
                JumpFall(_finalRisePositionY);
            }
        }

        private void JumpRise(float displacementY)
        {
            _isJumping = true;
            var curve = _jumpRiseCurve;
            var duration = curve[curve.length - 1].time;
            if (_jumpTimer < duration)
            {
                _jumpTimer += Time.fixedDeltaTime;

                //var position = _rb.position;
                //position.y = displacementY + curve.Evaluate(_jumpTimer);
                
                var velocityY = curve.Differentiate(_jumpTimer);
                //var position = _rb.position;
                //position.y += velocityY * Time.fixedDeltaTime;
                
                //_rb.MovePosition(position);
                var velocity = _rb.velocity;
                velocity.y = velocityY;
                _rb.velocity = velocity;
            }
            else
            {
                MovementState = MovementStates.JumpFall;
            }
        }
        
        private void JumpFall(float displacementY)
        {
            if (_grounded)
            {
                _isJumping = false;
                MovementState = MovementStates.None;
                return;
            }
            
            var curve = _jumpFallCurve;
            var duration = curve[curve.length - 1].time;
            if (_jumpTimer < duration)
            {
                _jumpTimer += Time.fixedDeltaTime;

                //var position = _rb.position;
                //position.y = displacementY + curve.Evaluate(_jumpTimer);
                
                var velocityY = curve.Differentiate(_jumpTimer);
                //var position = _rb.position;
                //position.y += velocityY * Time.fixedDeltaTime;
                
                //_rb.MovePosition(position);
                var velocity = _rb.velocity;
                velocity.y = velocityY;
                _rb.velocity = velocity;
            }
            else
            {
                // Continue falling along the same trajectory, until _grounded
                var velocityY = curve.Differentiate(curve[curve.length - 1].time);
                //var position = _rb.position;
                //position.y += velocityY * Time.fixedDeltaTime;
                
                //_rb.MovePosition(position);
                var velocity = _rb.velocity;
                velocity.y = velocityY;
                _rb.velocity = velocity;
            }
        }
        
        /*
        private IEnumerator JumpCoroutine()
        {
            _isJumping = true;
            var initialPositionY = _rayHit.point.y + _defaultRideHeight;
            yield return StartCoroutine(JumpRiseCoroutine(initialPositionY));
            var finalRisePositionY = _rb.position.y; // start falling from wherever the rise finished
            yield return StartCoroutine(JumpFallCoroutine(finalRisePositionY)); 
            _isJumping = false;
            PlayHoverSoundLoop.start();
        }
        
        private IEnumerator JumpRiseCoroutine(float displacementY)
        {
            var curve = _jumpRiseCurve;
            var duration = curve[curve.length - 1].time;
            var t = 0f;
            while (t < duration)
            {
                t += Time.fixedDeltaTime;

                var position = _rb.position;
                position.y = displacementY + curve.Evaluate(t);
                _rb.position = position;
                
                if (_shouldCancelJumpRise)
                {
                    _shouldCancelJumpRise = false;
                    yield break;
                }

                yield return new WaitForFixedUpdate();
            }
        }
        
        private IEnumerator JumpFallCoroutine(float displacementY)
        {
            var curve = _jumpFallCurve;
            var duration = curve[curve.length - 1].time;
            var t = 0f;
            while (t < duration)
            {
                t += Time.fixedDeltaTime;

                var position = _rb.position;
                position.y = displacementY + curve.Evaluate(t);
                _rb.position = position;

                if (_grounded)
                {
                    yield break;
                }

                yield return new WaitForFixedUpdate();
            }
            
            // Continue falling along the same trajectory, until _grounded
            var velocityY = curve.Differentiate(curve[curve.length - 1].time);
            while (!_grounded)
            {
                var position = _rb.position;
                position.y += velocityY * Time.fixedDeltaTime;
                _rb.position = position;

                yield return new WaitForFixedUpdate();
            }
        }
        */


        private void OnCollisionEnter(Collision collision)
        {
            if (MovementState == MovementStates.JumpRise)
            {
                MovementState = MovementStates.JumpFall;
            }
        }
    }
}
