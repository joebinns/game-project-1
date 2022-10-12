using System.Collections;
using Managers.Audio;
using Managers.Points;
using UnityEngine;
using UI;

namespace Tiles.Examples
{
    public class WhoTapLast : Tile
    {
        private float t = 3f;
        
        private float _player0Time, _player1Time;
    
        private void Update()
        {
            if (t <= 0)
            {
                EndEffect();
            }
        }
    
        public override void BeginEffect()
        {
            base.BeginEffect(); // Redirect inputs to this tile, play BeginEffectAudion and activate BeginEffectSprite.
                
            StartCoroutine(Cooldown());
        }
            
        public override void EndEffect()
        {
            if (_player0Time < _player1Time)
            {
                PointsManager.GainPoints(1, 100);
            }
            else
            {
                PointsManager.GainPoints(2, 100);
            }
                
            RoadGenerator roadGen = FindObjectOfType<RoadGenerator>();
            
            roadGen.generationMode = RoadGenerator.GenerationModes.Normal;
            
            // Call this method as the tile's last piece of logic!
            base.EndEffect(); // Redirect inputs back to player, play EndEffectAudio and deactivate effect sprite.
        }
    
        private IEnumerator Cooldown()
        {
            while (t >= 0)
            {
                t -= Time.deltaTime;
                FindObjectOfType<UIHandler>().SetEffectText(t.ToString("0.0#"));  
                yield return null;
            }
    
            yield return null;
        }
            
        public override void HandleInput(int playerId)
        {
            if (t > 0)
            {
                switch (playerId)
                {
                    case 0:
                        _player0Time = t;
                        break;
                    
                    case 1:
                        _player1Time = t;
                        break;
                }
            }
        }
        
    }
}

