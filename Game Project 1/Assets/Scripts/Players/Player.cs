using Tiles;
using UnityEngine;

namespace Players
{
    public class Player : MonoBehaviour
    {
        public int ID;
        
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
            Debug.Log("Player " + ID + " does default player movement thing!");
        }
    }
}
