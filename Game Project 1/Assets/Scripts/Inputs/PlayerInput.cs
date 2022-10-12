using System;
using Players;
using Tiles;
using UnityEngine;

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

        public void RedirectInput()
        {
            switch (_currentTile)
            {
                case null:
                    // Go ahead with regular player inputs
                    _player.HandleInput();
                    break;
                default:
                    // Redirect inputs to current tiles effect
                    _currentTile.HandleInput(_player.ID);
                    break;
            }
        }
    }
}
