using System;
using System.Collections;
using Inputs;
using UnityEngine;
using Managers.Audio;
using Managers.Camera;
using Managers.Points;
using Players;
using UI;

namespace Tiles.Examples
{
    public class RapidTapTile : Tile
    {

        [SerializeField] private int pointsPerTap = 100;


        public override void BeginEffect()
        {
            base.BeginEffect(); // Redirect inputs to this tile, play BeginEffectAudio and activate BeginEffectSprite.

            StraightenRoad.Instance.Straighten(1);

            FindObjectOfType<SpeedSelector>().SpeedMode = SpeedMode.Medium;
            
        }
        
        public override void EndEffect()
        {
            FindObjectOfType<SpeedSelector>().SpeedMode = SpeedMode.Low;

            StraightenRoad.Instance.Curve(1);
            // Call this method as the tile's last piece of logic!
            base.EndEffect(); // Redirect inputs back to player, play EndEffectAudio and deactivate effect sprite.
        }


        public override void HandleInput(Player player, OneFitsAllInput input)
        {
            // Play HandleInputAudio
            base.HandleInput(player, input);

            base.EffectSuccess(player);
            CameraManager.Main.Shake(5f, 0.35f);

        }
    }
}

