using Tiles;

namespace Examples.Triggers
{
    public class TileEffectTrigger : TileTrigger
    {
        public override void TriggerEntered()
        {
            base.TriggerEntered();
        
            transform.parent.GetComponent<Tile>().BeginEffect();
        }
    
        public override void TriggerExited()
        {
            base.TriggerExited();
        
            transform.parent.GetComponent<Tile>().EndEffect();
        }
    }
}
