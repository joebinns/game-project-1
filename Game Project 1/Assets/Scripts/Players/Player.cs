using System;
using Tiles;
using UnityEngine;

namespace Players
{
    public class Player : MonoBehaviour
    {
        public int ID;
        private HeightBuffer _heightBuffer;

        private void Awake()
        {
            _heightBuffer = GetComponent<HeightBuffer>();
        }

        private void OnTriggerEnter(Collider other)
        {
            var trigger = other.transform.GetComponent<TileTrigger>();
            if (!trigger) { Debug.LogWarning("Trigger is not a TileTrigger."); return; }
            
            trigger.TriggerEntered();
        }
        
        private void OnTriggerStay(Collider other)
        {
            var trigger = other.transform.GetComponent<TileTrigger>();
            if (!trigger) { Debug.LogWarning("Trigger is not a TileTrigger."); return; }

            trigger.TriggerStayed();
        }
        
        private void OnTriggerExit(Collider other)
        {
            var trigger = other.transform.GetComponent<TileTrigger>();
            if (!trigger) { Debug.LogWarning("Trigger is not a TileTrigger."); return; }

            trigger.TriggerExited();
        }

        public void HandleInput()
        {
            _heightBuffer.JumpPressed();
        }
    }
}
