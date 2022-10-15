using System;
using System.Collections;
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

        [SerializeField] private float timer = 3f;
        [SerializeField] private int pointsPerTap = 100;
        
        private bool _cooldownFinished;
        private bool _hasEnded;


        private void Update()
        {
            if (_cooldownFinished && _hasEnded == false)
            {
                EndEffect();
                _hasEnded = true;
            }        
        }

        public override void BeginEffect()
        {
            base.BeginEffect(); // Redirect inputs to this tile, play BeginEffectAudio and activate BeginEffectSprite.
            
            StartCoroutine(Cooldown());
        }
        
        public override void EndEffect()
        {
            // Call this method as the tile's last piece of logic!
            base.EndEffect(); // Redirect inputs back to player, play EndEffectAudio and deactivate effect sprite.
        }

        
        private IEnumerator Cooldown()
        {
            while (timer > 0)
            {
                timer -= Time.deltaTime;
                FindObjectOfType<UIHandler>().SetEffectText(timer.ToString("RAPID TAP! \n 0.0#"));  
                yield return null;
            }

            _cooldownFinished = true;
            yield return null;
        }
        
        
        public override void HandleInput(Player player)
        {
            // Play HandleInputAudio
            base.HandleInput(player);

            if (!_cooldownFinished)
            {
                Debug.Log("Player"+player.ID+" gained " + pointsPerTap + " points!");
                PointsManager.GainPoints(player.ID, pointsPerTap);
                CameraManager.Main.Shake(5f, 0.35f);
            }
            
            
        }
    }
}

