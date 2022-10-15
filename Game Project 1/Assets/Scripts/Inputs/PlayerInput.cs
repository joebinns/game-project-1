using System;
using Players;
using Tiles;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Inputs
{
    public class PlayerInput : MonoBehaviour
    {
        private Player _player;
        private Tile _currentTile;
        
        private void Awake()
        {
            _player = GetComponent<Player>();
        }
        
        private void OnEnable()
        {
            Tile.OnBeginEffect += SetRecipient;
            Tile.OnEndEffect += SetRecipient;
        }
        
        private void OnDisable()
        {
            Tile.OnBeginEffect += SetRecipient;
            Tile.OnEndEffect += SetRecipient;
        }

        private void SetRecipient(Tile tile)
        {
            _currentTile = tile;
        }

        public void RedirectInput(InputAction.CallbackContext context)
        {
            _player.HandleInput(context); // Always do some player movement
            switch (_currentTile)
            {
                /*
                case null:
                    // Go ahead with regular player inputs
                    _player.HandleInput(context);
                    break;
                */
                default:
                    // Redirect inputs to current tiles effect
                    _currentTile.HandleInput(_player.ID);
                    break;
            }
        }
    }
}
