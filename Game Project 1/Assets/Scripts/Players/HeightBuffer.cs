using UnityEngine;

namespace Players
{
    public class HeightBuffer : MonoBehaviour
    {
        private Rigidbody _rb;

        [SerializeField] private float _desiredHeight = 4f;
        [SerializeField] private float _maxRaycastDist = 8f;

        [SerializeField] private float _springStrength = 10f;
        [SerializeField] private float _springDamper = 5f;

        /// <summary>
        /// Get the rigidbody.
        /// </summary>
        void Start()
        {
            _rb = GetComponent<Rigidbody>();
        }

        /// <summary>
        /// Hold the rigid body above any underneath surface within range using a spring force.
        /// </summary>
        void FixedUpdate()
        {
            (bool didRayHit, RaycastHit rayHit) = RaycastToGround();
            if (didRayHit == false)
            {
                // then there are no forces to be apply
                return;
            }
            Vector3 displacement = new Vector3(0f, rayHit.distance - _desiredHeight, 0f);
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
        Vector3 DampedHookesLaw(float springStrength, Vector3 displacement, float springDamper, Vector3 relativeVelocity)
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
    }
}