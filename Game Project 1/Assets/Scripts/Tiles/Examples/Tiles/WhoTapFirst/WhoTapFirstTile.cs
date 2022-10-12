using System.Collections;
using Managers.Audio;
using Managers.Points;
using UnityEngine;

namespace Tiles
{
    public class WhoTapFirstTile : Tile
    {
        private bool _cooldownFinished = false;

        public override void BeginEffect()
        {
            // Redirect inputs to this tile
            base.BeginEffect();

            AudioManager.PlaySound(TileSettings.audioClips[0]); // Play Demo Sound
            // UI Manager change sprite to _sprite

            Debug.Log("Who Tap First Tile in effect");
            
            StartCoroutine(Cooldown(3f));
        }
        
        public override void EndEffect()
        {
            AudioManager.PlaySound(TileSettings.audioClips[0]); // Play Demo Sound
            // UI Manager change sprite to null

            Debug.Log("Who Tap First Tile no longer in effect");
            
            // Redirect inputs back to player
            //ALWAYS CALL THIS AT THE END OF EndEffect() !!!!!!!
            base.EndEffect();
        }

        private IEnumerator Cooldown(float duration)
        {
            float t = duration;
            while (t > 0)
            {
                t -= Time.deltaTime;
                //Update UI                
            }
            
            _cooldownFinished = true;
            yield return null;
        }
        
        public override void HandleInput(int playerId)
        {
            if (_cooldownFinished)
            {
                AudioManager.PlaySound(TileSettings.audioClips[1]); // Play Demo Sound 2
                PointsManager.GainPoints(playerId+1, 1000);
                // Add points to player
                Debug.Log("Player " + playerId + " gains 100 points!");
                EndEffect();
            }
        }
    }
}
