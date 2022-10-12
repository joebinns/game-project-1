using System.Collections;
using System.Collections.Generic;
using Tiles.Examples;
using UnityEngine;

namespace Tiles
{
    public class WhoTapFirstTile : Tile
    {
        private WhoTapFirstTileSettings _whoTapFirstTileSettings
        {
            get { return TileSettings as WhoTapFirstTileSettings; }
        }

        protected override void BeginEffect()
        {
            StartCoroutine(Cooldown(3f));
        }
        
        protected override void EndEffect()
        {
        }

        private IEnumerator Cooldown(float duration)
        {
            float t = duration;
            while (t > 0)
            {
                t -= Time.deltaTime;
                //Update UI                
            }
            
            _whoTapFirstTileSettings.cooldownFinished = true;
            yield return null;
        }
    }
}
