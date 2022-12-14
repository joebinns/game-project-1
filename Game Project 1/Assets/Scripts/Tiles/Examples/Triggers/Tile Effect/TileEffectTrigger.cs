using Players;
using Tiles;

namespace Tiles.Examples.Triggers
{
    public class TileEffectTrigger : TileTrigger
    {
        public override void TriggerEntered(Player player)
        {
            base.TriggerEntered(player);
            
            if (!Tile.IsActive)
            {
                Tile.BeginEffect();
            }
        }
    
        public override void TriggerExited(Player player)
        {
            base.TriggerExited(player);
            
            if (Tile.IsActive)
            {
                Tile.EndEffect();
            }
        }
    }
}
