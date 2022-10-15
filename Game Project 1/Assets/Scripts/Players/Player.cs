using System;
using Managers.Camera;
using Players.Physics_Based_Character_Controller;
using Tiles;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Players
{
    public class Player : MonoBehaviour
    {
        public int ID;
        //private HeightBuffer _heightBuffer;
        private PhysicsBasedCharacterController _physicsBasedCharacterController;
        public PhysicsBasedCharacterController PhysicsBasedCharacterController => _physicsBasedCharacterController;

            private void Awake()
        {
            //_heightBuffer = GetComponent<HeightBuffer>();
            _physicsBasedCharacterController = GetComponent<PhysicsBasedCharacterController>();
        }

        private void OnTriggerEnter(Collider other)
        {
            var trigger = other.transform.GetComponent<TileTrigger>();
            if (!trigger) { return; }
            
            trigger.TriggerEntered(this);
        }
        
        private void OnTriggerStay(Collider other)
        {
            var trigger = other.transform.GetComponent<TileTrigger>();
            if (!trigger) { return; }

            trigger.TriggerStayed(this);
        }
        
        private void OnTriggerExit(Collider other)
        {
            var trigger = other.transform.GetComponent<TileTrigger>();
            if (!trigger) { return; }

            trigger.TriggerExited(this);
        }

        public void HandleInput(InputAction.CallbackContext context)
        {
            _physicsBasedCharacterController.InputAction(context);
        }
    }
}
