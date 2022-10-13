using System;
using System.Collections;
using System.Net;
using Managers.Camera;
using Managers.Points;
using UnityEngine;
using UI;
using Unity.VisualScripting;
using Unity.VisualScripting.Dependencies.NCalc;

namespace Tiles.Examples
{
    public class WhoTapFirstTile : Tile
    {
        [SerializeField] private float _countdownTime = 3f;
        [SerializeField] private int pointsToWinner = 100;
        
        private bool _cooldownFinished = false;
        private float _cooldownTimer;

        public override void BeginEffect()
        {
            
            if (IsActive) { return; }
            IsActive = true;

            base.BeginEffect(); // Redirect inputs to this tile, play BeginEffectAudion and activate BeginEffectSprite.

            StartCoroutine(Cooldown(_countdownTime));
        }
        
        public override void EndEffect()
        {
            if (!IsActive) { return; }
            IsActive = false;
            
            
            // Call this method as the tile's last piece of logic!
            base.EndEffect(); // Redirect inputs back to player, play EndEffectAudio and deactivate effect sprite.
        }

        private IEnumerator Cooldown(float duration)
        {
            _cooldownTimer = duration;
            while (_cooldownTimer > 0)
            {
                FindObjectOfType<UIHandler>().SetEffectText((_countdownTime-_cooldownTimer).ToString("WHO TAP FIRST! \n"));
                _cooldownTimer -= Time.deltaTime;
                yield return null;
            }
            
            _cooldownFinished = true;
            yield return null;
            EndEffect();
        }

        public override void HandleInput(int playerId)
        {
            Debug.Log(playerId);
            if (!_cooldownFinished)
            {
                CameraManager.Main.Shake(20f, 0.1f);

                // Play HandleInputAudio
                base.HandleInput(playerId);

                PointsManager.GainPoints(playerId, pointsToWinner);

                Debug.Log("Player" + playerId + " gained" + pointsToWinner + " points!");
                
                EndEffect();
            }
        }
        }
}
