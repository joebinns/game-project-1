using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameOverManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI winnerNameTxt, loserNameTxt, winnerPointsTxt, loserPointsTxt;

    private void Start()
    {
        if (GameSettings.Instance.player1Points > GameSettings.Instance.player2Points)
        {
            winnerPointsTxt.text = GameSettings.Instance.player1Points.ToString();
            winnerNameTxt.text = "Player1";

            loserNameTxt.text = "Player2";
            loserPointsTxt.text = GameSettings.Instance.player2Points.ToString();
        }

        else
        {
            winnerPointsTxt.text = GameSettings.Instance.player2Points.ToString();
            winnerNameTxt.text = "Player2";

            loserNameTxt.text = "Player1";
            loserPointsTxt.text = GameSettings.Instance.player1Points.ToString();
        }
    }
}
