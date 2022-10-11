using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Inputs
{
    // If there is a current effective tile, then temporarily unsubscribe inputs from the player and subscribe to the tile
    public class InputHandler : MonoBehaviour
    {
        [SerializeField] private List<PlayerInput> _playerInputs;
        
        public void InputPlayerOne(InputAction.CallbackContext context)
        {
            if (!context.performed) { return; }

            _playerInputs[0].RedirectInput();
        }
        
        public void InputPlayerTwo(InputAction.CallbackContext context)
        {
            if (!context.performed) { return; }
            
            _playerInputs[1].RedirectInput();
        }

    }
}
