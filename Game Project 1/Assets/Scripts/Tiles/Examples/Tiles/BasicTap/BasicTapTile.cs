using System.Collections;
using System.Collections.Generic;
using Inputs;
using Managers.Camera;
using Managers.Points;
using Players;
using Players.Physics_Based_Character_Controller;
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
        }
        
        public override void EndEffect()
        {
            // Redirect inputs back to player
            base.EndEffect();
        }

        public override void HandleInput(Player player, OneFitsAllInput input)
        {
            base.HandleInput(player, input);
            
            CameraManager.Main.Shake(5f, 0.35f);
            //FindObjectOfType<PlayerManager>().Players[playerId].GetComponent<PhysicsBasedCharacterController>().JumpPressed();

            if (!_cooldownFinished && !_playerPressed[player.ID])
            {
                PointsManager.GainPoints(player.ID, pointsToWinner);

                Debug.Log("Player"+ player.ID +" avoided the obstacle and gained " + pointsToWinner + " points!");
                //Remove points from playerId
                _playerPressed[player.ID] = true;
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



