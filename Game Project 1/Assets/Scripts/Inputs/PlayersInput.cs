using System;
using Players;
using Tiles;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Inputs
{
    public class PlayersInput : MonoBehaviour
    {
        private Player _player;
        private Tile _currentTile;
        private bool _doesHoldRegister = false;
        
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

        public void InputHold(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                _doesHoldRegister = false;
            }

            if (context.performed)
            {
                _doesHoldRegister = true;
            }

            RedirectInput(new OneFitsAllInput(context, _doesHoldRegister | context.started ? InputType.Hold : InputType.Tap));
        }

        public void RedirectInput(OneFitsAllInput input)
        {
            _player.HandleInput(input);
            switch (_currentTile)
            {
                case null:
                    break;
                default:
                    // Direct inputs to current tiles effect
                    _currentTile.HandleInput(_player, input);
                    break;
            }
        }
    }

    public struct OneFitsAllInput
    {
        public OneFitsAllInput(InputAction.CallbackContext context, InputType inputType)
        {
            Context = context;
            InputType = inputType;
        }
        
        public InputAction.CallbackContext Context { get; }
        public InputType InputType { get; }
    }

    public enum InputType
    {
        Tap,
        Hold
    }
}
