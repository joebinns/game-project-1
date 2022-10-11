
using Managers.Audio;
using Inputs;
using Players;
using UnityEngine;
using Managers.Points;

namespace Tiles.Examples
{
    [CreateAssetMenu(menuName = "Tiles/RapidTapTile")]
    public class RapidTapTileSettings : TileSettings
    {
        public override void BeginEffect()
        {
            // Redirect inputs to this tile
            base.BeginEffect();

            AudioManager.PlaySound(_audioClips[0]); // Play Demo Sound
            // UI Manager change sprite to _sprite

            Debug.Log("Rapid Tap Tile in effect");
        }
        
        public override void EndEffect()
        {
            // Redirect inputs back to player
            base.EndEffect();
            
            AudioManager.PlaySound(_audioClips[0]); // Play Demo Sound
            // UI Manager change sprite to null

            Debug.Log("Rapid Tap Tile no longer in effect");
        }

        public override void HandleInput(int playerId)
        {
            AudioManager.PlaySound(_audioClips[1]); // Play Demo Sound 2


            PointsManager.GainPoints(playerId+1, 1000);
            // Add points to player
            Debug.Log("Player " + playerId + " gains 10 points!");
        }
    }
}
