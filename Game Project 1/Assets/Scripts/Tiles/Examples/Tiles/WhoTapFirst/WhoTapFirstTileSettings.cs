
using System.Collections;
using Managers.Audio;
using Inputs;
using Players;
using UnityEngine;
using Managers.Points;
using Unity.VisualScripting;

namespace Tiles.Examples
{
    [CreateAssetMenu(menuName = "Tiles/WhoTapFirstTile")]
    
    public class WhoTapFirstTileSettings : TileSettings
    {

        public bool cooldownFinished = false;
        
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
            
            FindObjectOfType<RoadGenerator>().generationMode = RoadGenerator.GenerationModes.Normal;

            AudioManager.PlaySound(_audioClips[0]); // Play Demo Sound
            // UI Manager change sprite to null


            Debug.Log("Rapid Tap Tile no longer in effect");
        }

        public override void HandleInput(int playerId)
        {

            if (cooldownFinished)
            {
                AudioManager.PlaySound(_audioClips[1]); // Play Demo Sound 2
                PointsManager.GainPoints(playerId+1, 1000);
                // Add points to player
                Debug.Log("Player " + playerId + " gains 100 points!");
                EndEffect();
            }

            
        }


    }
}
