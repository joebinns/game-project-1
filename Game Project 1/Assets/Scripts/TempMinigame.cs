using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Managers.Audio;
using Managers.Camera;

public class TempMinigame : MonoBehaviour
{
    float pointsDecreaseSpeed;
    float maxPoints;
    bool p1PlayerPressed, p2PlayerPressed;
    float p1PointsGained, p2PointsGained;
    float currentPoints;

    //
    void Enter() => StartCoroutine(Countdown());

    IEnumerator Countdown()
    {
        currentPoints = maxPoints;
        while (currentPoints > 0)
        {
            currentPoints -= pointsDecreaseSpeed * Time.deltaTime;
            yield return null;
        }
        AnnouceWinner();
    }

    void AnnouceWinner()
    {
        if(p1PointsGained > p2PointsGained)
            Debug.Log($"player 1 wins with {p1PointsGained} against player 2 with {p2PointsGained}");
            else
            Debug.Log($"player 2 wins with {p2PointsGained} against player 1 with {p1PointsGained}");
    }

    //
    void pressed(int playerid)
    {

        if(playerid == 1 && !p1PlayerPressed)
        {
            p1PlayerPressed = true;
            p1PointsGained = currentPoints;
        } else if (playerid == 2 && !p2PlayerPressed)
        {
            p2PlayerPressed = true;
            p2PointsGained = currentPoints;
        } else { Debug.LogError($"player with id {playerid} not found"); }
    }
}
