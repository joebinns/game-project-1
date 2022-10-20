using UnityEngine;
using Inputs;
using Players;

namespace Tiles.Examples
{
    public class ObstacleTile : Tile
    {
        public override void BeginEffect()
        {
            base.BeginEffect();
        }

        public override void EndEffect()
        {
            base.EndEffect();
        }

        public override void EffectSuccess(Player player, bool shouldResetMultiplier)
        {
            base.EffectSuccess(player, shouldResetMultiplier);
        }
        
        public override void EffectFail(Player player)
        {
            base.EffectFail(player);
        }

        public override void HandleInput(Player player, OneFitsAllInput input)
        {
            base.HandleInput(player, input);
        }
    }
}
