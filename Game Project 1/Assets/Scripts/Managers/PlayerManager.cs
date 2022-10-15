using System;
using System.Collections;
using System.Collections.Generic;
using Players;
using Players.Physics_Based_Character_Controller;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public List<Player> Players;

    public void SwitchAllMovementOptions(PhysicsBasedCharacterController.MovementOptions movementOption)
    {
        foreach (Player player in Players)
        {
            player.PhysicsBasedCharacterController.SetMovementOption(movementOption);
        }
    }
    
    public void ResetAllMovementOptions()
    {
        foreach (Player player in Players)
        {
            player.PhysicsBasedCharacterController.ResetMovementOption();
        }
    }
}
