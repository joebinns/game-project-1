using Tiles;

namespace Tiles.Examples.Triggers
{
    public class TileEffectTrigger : TileTrigger
    {
        public override void TriggerEntered()
        {
            base.TriggerEntered();
        
            var tile = transform.parent.GetComponent<Tile>();
            if (!tile.IsActive)
            {
                transform.parent.GetComponent<Tile>().BeginEffect();
            }
        }
    
        public override void TriggerExited()
        {
            base.TriggerExited();
        
            var tile = transform.parent.GetComponent<Tile>();
            if (tile.IsActive)
            {
                transform.parent.GetComponent<Tile>().EndEffect();
            }
        }
    }
}
