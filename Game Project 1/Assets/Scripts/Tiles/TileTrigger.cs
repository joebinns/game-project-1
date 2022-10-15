using System;
using Players;
using UnityEngine;

namespace Tiles
{
    public abstract class TileTrigger : MonoBehaviour
    {
        public event Action OnTriggerEnter;
        public event Action OnTriggerStay;
        public event Action OnTriggerExit;

        protected Tile Tile;

        private void Awake()
        {
            Tile = transform.parent.GetComponent<Tile>();
        }

        public virtual void TriggerEntered(Player player)
        {
            OnTriggerEnter?.Invoke();
        }
    
        public virtual void TriggerStayed(Player player)
        {
            OnTriggerStay?.Invoke();
        }
    
        public virtual void TriggerExited(Player player)
        {
            OnTriggerExit?.Invoke();
        }
    }
}
