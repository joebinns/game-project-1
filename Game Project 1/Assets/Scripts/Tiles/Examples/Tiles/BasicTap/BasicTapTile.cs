using System.Collections;
using System.Collections.Generic;
using Managers.Points;
using UnityEngine;

namespace Tiles
{
    public class BasicTapTile : Tile
    {
        
        [SerializeField] private float timer = 1f;
        [SerializeField] private int pointsToWinner = 50;
        
        private bool _cooldownFinished;
        private bool[] _playerPressed;
        
        
        private void Update()
        {
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

            Debug.Log("Basic Tap Tile in effect");
        }
        
        public override void EndEffect()
        {
            // Redirect inputs back to player
            base.EndEffect();
            
            // UI Manager change sprite to null
            
            Debug.Log("Basic Tap Tile no longer in effect");
        }

        public override void HandleInput(int playerId)
        {
            if (!_cooldownFinished && !_playerPressed[playerId])
            {
                PointsManager.GainPoints(playerId, pointsToWinner);

                Debug.Log("Player"+playerId+" avoided the obstacle and gained " + pointsToWinner + " points!");
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



