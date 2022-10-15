using System.Collections;
using System.Collections.Generic;
using Tiles;
using UnityEngine;

namespace Tiles.Examples.Triggers
{
    public class IndefiniteTrigger : TileTrigger
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
        }
    } 
}

