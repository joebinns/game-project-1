using System.Collections.Generic;
using UnityEngine;

namespace Utilities
{
    /// <summary>
    /// Makes this collider ignore all collisions with other specified colliders.
    /// </summary>
    public class IgnoreColliders : MonoBehaviour
    {
        [SerializeField] private List<Collider> _collidersToIgnore;
        private Collider _collider;

        /// <summary>
        /// Turn off collisions between this collider and other collidersToIgnore.
        /// </summary>
        void Start()
        {
            _collider = this.GetComponent<Collider>();
            foreach (Collider otherCollider in _collidersToIgnore)
            {
                Physics.IgnoreCollision(_collider, otherCollider);
            }
        }
    }
}
