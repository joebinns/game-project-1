using System;
using System.Collections;
using Inputs;
using Managers.Audio;
using Managers.Points;
using Players;
using UnityEngine;
using UI;

namespace Tiles.Examples
{
    public class WhoTapLast : Tile
    {
        [SerializeField] private float timer = 3f;
        [SerializeField] private int pointsToWinner = 100;
        
        private bool _cooldownFinished;

        private bool _hasEnded;
        
        private float _player0Time, _player1Time;


        private void Start()
        {
            _player0Time = timer;
            _player1Time = timer;
        }

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
            base.BeginEffect(); // Redirect inputs to this tile, play BeginEffectAudion and activate BeginEffectSprite.
                
            StartCoroutine(Cooldown());
        }
            
        public override void EndEffect()
        {
            if (_player0Time < _player1Time)
            {
                Debug.Log("Player0 gained " + pointsToWinner + " points!");
               // PointsManager.GainPoints(0, pointsToWinner);
            }
            else if (_player1Time < _player0Time)
            {
                Debug.Log("Player1 gained " + pointsToWinner + " points!");
               // PointsManager.GainPoints(1, pointsToWinner);
            }

            // Call this method as the tile's last piece of logic!
            base.EndEffect(); // Redirect inputs back to player, play EndEffectAudio and deactivate effect sprite.
        }
    
        private IEnumerator Cooldown()
        {
            while (timer > 0)
            {
                timer -= Time.deltaTime;
                FindObjectOfType<UIHandler>().SetEffectText(timer.ToString("WHO TAP LAST! \n 0.0#"));  
                yield return null;
            }

            _cooldownFinished = true;
            yield return null;
        }
            
        public override void HandleInput(Player player, OneFitsAllInput input)
        {
            if (timer > 0)
            {
                switch (player.ID)
                {
                    case 0:
                        _player0Time = timer;
                        break;
                    
                    case 1:
                        _player1Time = timer;
                        break;
                }
            }
        }
        
    }
}

