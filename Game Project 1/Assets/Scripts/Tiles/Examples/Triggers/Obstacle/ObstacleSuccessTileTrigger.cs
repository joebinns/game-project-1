using System.Collections.Generic;
using Players;
using UnityEngine;

namespace Tiles.Examples.Triggers
{
    public class ObstacleSuccessTileTrigger : SuccessTileTrigger
    {
        [SerializeField] private FailTileTrigger _failTileTrigger;
    
        public override void TriggerEntered(Player player)
        {
            base.TriggerEntered(player);
        }
    
        public override void TriggerExited(Player player)
        {
            if (_failTileTrigger.PlayersThatFailed.Contains(player)) { return; }
            
            base.TriggerExited(player);
        }
    }
}
