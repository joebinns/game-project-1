using UnityEngine;
using Managers.Audio;
using Managers.Points;

namespace Tiles
{
    public class RapidTapTile : Tile
    {
        public override void BeginEffect()
        {
            // Redirect inputs to this tile
            base.BeginEffect();

            AudioManager.PlaySound(TileSettings.audioClips[0]); // Play Demo Sound
            // UI Manager change sprite to _sprite

            Debug.Log("Rapid Tap Tile in effect");
        }
        
        public override void EndEffect()
        {
            // Redirect inputs back to player
            base.EndEffect();
            
            AudioManager.PlaySound(TileSettings.audioClips[0]); // Play Demo Sound
            // UI Manager change sprite to null

            Debug.Log("Rapid Tap Tile no longer in effect");
        }

        public override void HandleInput(int playerId)
        {
            AudioManager.PlaySound(TileSettings.audioClips[1]); // Play Demo Sound 2

            PointsManager.GainPoints(playerId, 1000);
            // Add points to player
            Debug.Log("Player " + playerId + " gains 10 points!");
        }
    }
}

