using System.Collections;
using System.Collections.Generic;
using Players;
using Tiles;
using UnityEngine;

namespace Tiles.Examples.Triggers
{
    public class IndefiniteTrigger : TileTrigger
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
        }
    } 
}

