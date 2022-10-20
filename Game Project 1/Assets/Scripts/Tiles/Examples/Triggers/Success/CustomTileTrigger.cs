using System;
using Managers.Points;
using Players;
using UnityEngine;

namespace Tiles.Examples.Triggers
{
    public class CustomTileTrigger : TileTrigger
    {
        [SerializeField] private int _points;
        
        public override void TriggerEntered(Player player)
        {
            base.TriggerEntered(player);
        }
    
        public override void TriggerExited(Player player)
        {
            base.TriggerExited(player);
            
            //AudioManager.PlaySound(TileSettings.EffectSuccessAudio);
            PointsManager.Instance.ChangePoints(player, _points, MultiplierMode.Increment);
        }
    }
}
