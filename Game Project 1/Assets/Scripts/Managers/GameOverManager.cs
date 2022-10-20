using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Managers.Points;

public class GameOverManager : MonoBehaviour
{
    [SerializeField] private Transform winnerTransform, loserTransform;
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
        
        GetPlayerModels();
    }

    public void SwitchToMain()
    {
        GameSettings.Instance.ResetPoints();
        GameSettings.Instance.SwitchScene(0);
    }

    public void GetPlayerModels()
    {

        /*
        //p1Wins
        if (GameSettings.Instance.player1Points > GameSettings.Instance.player2Points)
        {
            GameObject winPlayer = Instantiate(GameSettings.Instance.finalPlayer1, winnerTransform.position, Quaternion.identity);
            GameObject losePlayer = Instantiate(GameSettings.Instance.finalPlayer2, loserTransform.position, Quaternion.identity);

            winPlayer.transform.rotation = winnerTransform.rotation;
            winPlayer.transform.localScale = winnerTransform.localScale;

            losePlayer.transform.rotation = winnerTransform.rotation;
            losePlayer.transform.localScale = loserTransform.localScale;
        }

        //p2Wins
        else
        {
            GameObject winPlayer = Instantiate(GameSettings.Instance.finalPlayer2, winnerTransform.position, Quaternion.identity);
            GameObject losePlayer = Instantiate(GameSettings.Instance.finalPlayer1, loserTransform.position, Quaternion.identity);
            
            winPlayer.transform.rotation = winnerTransform.rotation;
            winPlayer.transform.localScale = winnerTransform.localScale;

            losePlayer.transform.rotation = winnerTransform.rotation;
            losePlayer.transform.localScale = loserTransform.localScale;
            
        }
        */

        GameObject p1Obj = GameObject.Find("Player1Obj");
        GameObject p2Obj = GameObject.Find("Player2Obj");

        //p1Wins
        if (GameSettings.Instance.player1Points > GameSettings.Instance.player2Points)
        {
            p1Obj.transform.position = winnerTransform.position;
            p1Obj.transform.rotation = winnerTransform.rotation;
            p1Obj.transform.localScale = winnerTransform.localScale;

            p2Obj.transform.position = loserTransform.position;
            p2Obj.transform.rotation = loserTransform.rotation;
            p2Obj.transform.localScale = loserTransform.localScale;
        }

        //p2Wins
        else
        {
            p2Obj.transform.position = winnerTransform.position;
            p2Obj.transform.rotation = winnerTransform.rotation;
            p2Obj.transform.localScale = winnerTransform.localScale;

            p1Obj.transform.position = loserTransform.position;
            p1Obj.transform.rotation = loserTransform.rotation;
            p1Obj.transform.localScale = loserTransform.localScale;
        }
    }

    public void EndGameLoop()
    {
        GameObject p1Obj = GameObject.Find("Player1Obj");
        GameObject p2Obj = GameObject.Find("Player2Obj");
        
        Destroy(p1Obj);
        Destroy(p2Obj);
    }
}
