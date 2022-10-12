using System.Collections;
using Managers.Points;
using UnityEngine;
using UI;
using Unity.VisualScripting;

namespace Tiles.Examples
{
    public class WhoTapFirstTile : Tile
    {
        private bool _cooldownFinished = false;
        private bool _eventFinished = false;
        float t;

        public override void BeginEffect()
        {
            base.BeginEffect(); // Redirect inputs to this tile, play BeginEffectAudion and activate BeginEffectSprite.
            
            
            StartCoroutine(Cooldown(3f));
        }
        
        public override void EndEffect()
        {
            // Call this method as the tile's last piece of logic!
            base.EndEffect(); // Redirect inputs back to player, play EndEffectAudio and deactivate effect sprite.
        }

        private IEnumerator Cooldown(float duration)
        {
            t = duration;
            while (t > 0)
            {
                t -= Time.deltaTime;
            }
            
            _cooldownFinished = true;
            yield return null;
        }

        public override void HandleInput(int playerId)
        {
            if (_cooldownFinished)
            {
                // Play HandleInputAudio
                base.HandleInput(playerId);

                PointsManager.GainPoints(playerId, 1000);
                FindObjectOfType<UIHandler>().SetEffectText((3f-t).ToString("0.0#"));
                EndEffect();
            }
        }
    }
}
