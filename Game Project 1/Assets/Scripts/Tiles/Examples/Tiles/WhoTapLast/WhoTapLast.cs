using System;
using System.Collections;
using Managers.Audio;
using Managers.Points;
using UnityEngine;
using UI;

namespace Tiles.Examples
{
    public class WhoTapLast : Tile
    {
        [SerializeField] private float t = 3f;
        private bool _cooldownFinished;

        private bool _hasEnded;
        
        private float _player0Time, _player1Time;


        private void Start()
        {
            _player0Time = t;
            _player1Time = t;
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
                Debug.Log("Player0 gained 100 points!");
                PointsManager.GainPoints(0, 100);
            }
            else if (_player1Time < _player0Time)
            {
                Debug.Log("Player1 gained 100 points!");
                PointsManager.GainPoints(1, 100);
            }

            // Call this method as the tile's last piece of logic!
            base.EndEffect(); // Redirect inputs back to player, play EndEffectAudio and deactivate effect sprite.
        }
    
        private IEnumerator Cooldown()
        {
            while (t > 0)
            {
                t -= Time.deltaTime;
                FindObjectOfType<UIHandler>().SetEffectText(t.ToString("0.0#"));  
                yield return null;
            }

            _cooldownFinished = true;
            yield return null;
        }
            
        public override void HandleInput(int playerId)
        {
            if (t > 0)
            {
                switch (playerId)
                {
                    case 0:
                        _player0Time = t;
                        break;
                    
                    case 1:
                        _player1Time = t;
                        break;
                }
            }
        }
        
    }
}

