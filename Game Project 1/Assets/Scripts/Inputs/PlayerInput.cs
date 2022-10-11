using System;
using Players;
using Tiles;
using UnityEngine;

namespace Inputs
{
    public class PlayerInput : MonoBehaviour
    {
        private Player _player;
        private TileSettings _currentTileSettings;
        
        private void Awake()
        {
            _player = GetComponent<Player>();
        }
        
        private void OnEnable()
        {
            TileSettings.OnTileChanged += SetRecipient;
        }
        
        private void OnDisable()
        {
            TileSettings.OnTileChanged -= SetRecipient;
        }

        private void SetRecipient(TileSettings tileSettings)
        {
            _currentTileSettings = tileSettings;
        }

        public void RedirectInput()
        {
            switch (_currentTileSettings)
            {
                case null:
                    // Go ahead with regular player inputs
                    _player.HandleInput();
                    break;
                default:
                    // Redirect inputs to current tiles effect
                    _currentTileSettings.HandleInput(_player.ID);
                    break;
            }
        }
    }
}
