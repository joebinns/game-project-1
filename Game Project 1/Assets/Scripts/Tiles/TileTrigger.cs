using System;
using UnityEngine;

namespace Tiles
{
    public abstract class TileTrigger : MonoBehaviour
    {
        public event Action OnTriggerEnter;
        public event Action OnTriggerStay;
        public event Action OnTriggerExit;

        public virtual void TriggerEntered()
        {
            OnTriggerEnter?.Invoke();
        }
    
        public virtual void TriggerStayed()
        {
            OnTriggerStay?.Invoke();
        }
    
        public virtual void TriggerExited()
        {
            OnTriggerExit?.Invoke();
        }
    }
}
