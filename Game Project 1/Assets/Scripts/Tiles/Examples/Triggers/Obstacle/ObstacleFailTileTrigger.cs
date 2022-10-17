using System.Collections.Generic;
using Players;
using UnityEngine;
using UnityEngine.Events;

namespace Tiles.Examples.Triggers
{
    public class ObstacleFailTileTrigger : TileTrigger
    {
        [HideInInspector] public List<Player> PlayersThatFailed = new List<Player>();

        public UnityEvent onFail;

        public override void TriggerEntered(Player player)
        {
            base.TriggerEntered(player);

           // onFail.Invoke();
            PlayersThatFailed.Add(player);
            Tile.EffectFail(player);
        }
    
        public override void TriggerExited(Player player)
        {
            base.TriggerExited(player);
        }
    }
}
