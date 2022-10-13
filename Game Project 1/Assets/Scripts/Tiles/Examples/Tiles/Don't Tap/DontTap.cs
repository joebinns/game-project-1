using System;
using System.Collections;
using System.Collections.Generic;
using Managers.Audio;
using Managers.Camera;
using Managers.Points;
using UI;
using UnityEngine;
using Managers.Camera;
using Players.Physics_Based_Character_Controller;

namespace Tiles
{
    
    public class DontTap : Tile
    {

        [SerializeField] private float timer = 3f;
        [SerializeField] private int pointsToRemove = -500;
        
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
            if (IsActive) { return; }
            IsActive = true;
            
            // Redirect inputs to this tile
            base.BeginEffect();

            _playerPressed = new[] { false, false };

            // UI Manager change sprite to _sprite

            StartCoroutine(Cooldown(timer));
        }
        
        public override void EndEffect()
        {
            if (!IsActive) { return; }
            IsActive = false;
            
            // Redirect inputs back to player
            base.EndEffect();
            
            // UI Manager change sprite to null
        }

        public override void HandleInput(int playerId)
        {
            base.HandleInput(playerId);
            
            CameraManager.Main.Shake(5f, 0.35f);
            //FindObjectOfType<PlayerManager>().Players[playerId].GetComponent<PhysicsBasedCharacterController>().JumpPressed();
            
            if (!_cooldownFinished && !_playerPressed[playerId])
            {
                PointsManager.RemovePoints(playerId, pointsToRemove);
                CameraManager.Main.Shake(20f, 0.1f);
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
