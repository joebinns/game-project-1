using Managers.Audio;
using Players;

namespace Tiles.Examples.Triggers
{
    public class ObstacleTileTrigger : TileTrigger
    {
        // TODO: Devise some way that individual obstacles in a tile can't assign success AND fail to a single player
        
        public override void TriggerEntered(Player player)
        {
            base.TriggerEntered(player);
            
            Tile.EffectFail(player);
        }
    
        public override void TriggerExited(Player player)
        {
            base.TriggerExited(player);
        }
    }
}
