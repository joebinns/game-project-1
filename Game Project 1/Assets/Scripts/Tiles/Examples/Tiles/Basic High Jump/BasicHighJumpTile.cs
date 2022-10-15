using DG.Tweening.Core.Easing;
using Players;

namespace Tiles.Examples
{
    public class BasicHighJumpTile : Tile
    {
        public override void BeginEffect()
        {
            base.BeginEffect();
        }

        public override void EndEffect()
        {
            base.EndEffect();
        }

        public override void EffectSuccess(Player player)
        {
            base.EffectSuccess(player);
        }
        
        public override void EffectFail(Player player)
        {
            base.EffectFail(player);
            
            player.GetComponent<FlashMaterial>().FlashMaterials();
        }

        public override void HandleInput(Player player)
        {
            base.HandleInput(player);
        }
    }
}
