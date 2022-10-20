using Players;
using UnityEngine;

namespace Tiles.Examples.Triggers
{
    public class SuccessTileTrigger : TileTrigger
    {
        public override void TriggerEntered(Player player)
        {
            base.TriggerEntered(player);
        }
    
        public override void TriggerExited(Player player)
        {
            base.TriggerExited(player);
            
            Tile.EffectSuccess(player, MultiplierChange.Increment);
        }
    }
}
