using System.Collections.Generic;
using Players;
using UnityEngine;

namespace Tiles.Examples.Triggers
{
    public class ObstacleSuccessTileTrigger : TileTrigger
    {
        [SerializeField] private ObstacleFailTileTrigger _obstacleFailTileTrigger;
    
        public override void TriggerEntered(Player player)
        {
            base.TriggerEntered(player);
        }
    
        public override void TriggerExited(Player player)
        {
            base.TriggerExited(player);

            if (_obstacleFailTileTrigger.PlayersThatFailed.Contains(player)) { return; }
            
            Tile.EffectSuccess(player);
        }
    }
}
