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
        
            transform.parent.GetComponent<Tile>().BeginEffect();
        }
    
        public override void TriggerExited()
        {
        }
    } 
}

