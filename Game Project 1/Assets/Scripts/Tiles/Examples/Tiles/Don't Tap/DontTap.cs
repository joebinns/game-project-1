using System;
using System.Collections;
using System.Collections.Generic;
using Managers.Audio;
using Managers.Points;
using UI;
using UnityEngine;

namespace Tiles
{
    
    public class DontTap : Tile
    {

        [SerializeField] private float timer = 3f;
        [SerializeField] private int pointsToRemove = -100;
        
        private bool _cooldownFinished;
        private bool[] _playerPressed;

        private void Update()
        {
            
            FindObjectOfType<UIHandler>().SetEffectText("DON'T TAP!!");

            if (_cooldownFinished)
            {
                EndEffect();
            }
        }

        public override void BeginEffect()
        {
            // Redirect inputs to this tile
            base.BeginEffect();

            _playerPressed = new[] { false, false };

            // UI Manager change sprite to _sprite

            StartCoroutine(Cooldown(timer));
        }
        
        public override void EndEffect()
        {
            // Redirect inputs back to player
            base.EndEffect();
            
            // UI Manager change sprite to null
        }

        public override void HandleInput(int playerId)
        {
            if (!_cooldownFinished && !_playerPressed[playerId])
            {
                PointsManager.GainPoints(playerId, pointsToRemove);

                Debug.Log("Player"+playerId+" tapped! You lost " + pointsToRemove + " points!");
                //Remove points from playerId
                _playerPressed[playerId] = true;
            }
        }
        
        private IEnumerator Cooldown(float duration)
        {
            float t = duration;
            while (t > 0)
            {
                t -= Time.deltaTime;
                //Update UI
                yield return null;
            }
            
            _cooldownFinished = true;
            yield return null;
        }
    }
}
