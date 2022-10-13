using System;
using System.Collections;
using UnityEngine;
using Managers.Audio;
using Managers.Points;
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
                FindObjectOfType<UIHandler>().SetEffectText(timer.ToString("0.0#"));  
                yield return null;
            }

            _cooldownFinished = true;
            yield return null;
        }
        
        
        public override void HandleInput(int playerId)
        {
            // Play HandleInputAudio
            base.HandleInput(playerId);

            if (!_cooldownFinished)
            {
                Debug.Log("Player"+playerId+" gained " + pointsPerTap + " points!");
                PointsManager.GainPoints(playerId, pointsPerTap);
            }
            
            
        }
    }
}

