using UnityEngine;
using Managers.Audio;
using Managers.Points;

namespace Tiles.Examples
{
    public class RapidTapTile : Tile
    {
        public override void BeginEffect()
        {
            base.BeginEffect(); // Redirect inputs to this tile, play BeginEffectAudio and activate BeginEffectSprite.
        }
        
        public override void EndEffect()
        {
            // Call this method as the tile's last piece of logic!
            base.EndEffect(); // Redirect inputs back to player, play EndEffectAudio and deactivate effect sprite.
        }

        public override void HandleInput(int playerId)
        {
            // Play HandleInputAudio
            base.HandleInput(playerId);
            
            PointsManager.GainPoints(playerId+1, 1000);
        }
    }
}

