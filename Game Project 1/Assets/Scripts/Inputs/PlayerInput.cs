using System;
using System.Collections;
using Players;
using Tiles;
using UnityEditor.Rendering.LookDev;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Inputs
{
    public class PlayerInput : MonoBehaviour
    {
        [SerializeField] private float _holdTime = 1f;
        
        private Player _player;
        private Tile _currentTile;
        private bool _doesHoldRegister = false;
        
        private void Awake()
        {
            _player = GetComponent<Player>();
            
            // TODO: Get default hold time automatically...
            //InputSettings.defaultHoldTime;
            //Debug.Log(GetComponent<UnityEngine.InputSystem.PlayerInput>()..defaultHoldTime);
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
        
        /*
        public void InputTap(InputAction.CallbackContext context)
        {
            if (context.started) { StartCoroutine(CheckIfHoldRegisters()); }
            if (context.canceled & !_doesHoldRegister) { RedirectInput(new OneFitsAllInput(context, InputType.Tap)); }
        }
        */
        
        public void InputHold(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                _doesHoldRegister = false;
                return;
            }

            if (context.performed)
            {
                _doesHoldRegister = true;
            }

            RedirectInput(new OneFitsAllInput(context, _doesHoldRegister ? InputType.Hold : InputType.Tap));
        }
        
        /*
        private IEnumerator CheckIfHoldRegisters()
        {
            _doesHoldRegister = false;
            yield return new WaitForSeconds(_holdTime);
            _doesHoldRegister = true;
        }
        */

        public void RedirectInput(OneFitsAllInput input)
        {
            //Debug.Log(input);
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
