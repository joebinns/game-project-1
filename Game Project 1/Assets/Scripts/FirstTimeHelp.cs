using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstTimeHelp : MonoBehaviour
{
    public enum ObstacleType
    {
        Jump,
        Slide,
        RapidTap
    }

    public ObstacleType upcomingObstacle;

    private void OnTriggerEnter(Collider other)
    {
        switch (upcomingObstacle)
        {
            case ObstacleType.Jump:

                break;
            
            case ObstacleType.Slide:

                break;
            
            case ObstacleType.RapidTap:

                break;
        }
    }
}
