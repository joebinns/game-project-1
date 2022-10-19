using System;
using System.Collections.Generic;
using Players;
using UnityEngine;

namespace Managers.Points
{
    public static class PointsManager
    {
        private static List<int> _points;
        private static List<int> _consecutiveGains;

        /// <summary>
        /// add points to a specifdied player
        /// </summary>
        /// <param name="playerIndex">the player to send points to, 1-2</param>
        /// <param name="points">the amount</param>
        public static void ChangePoints(Player player, int points)
        {
            int multipliedPoints;
            
            multipliedPoints = (int)((float)points * (1f + 0.1f * _consecutiveGains[player.ID]));
            _points[player.ID] += multipliedPoints;
            
            if (points > 0)
            {
                _consecutiveGains[player.ID]++;
            }
            else
            {
                _consecutiveGains[player.ID] = 0;
            }
            
            FloatingTextManager.Instance.SpawnText(multipliedPoints.ToString(), player.transform.position + Vector3.right * 5 + Vector3.up * 3, 1, 1, Color.white);
        }

        /// <summary>
        /// return the current amount of points the specified player has
        /// </summary>
        /// <param name="playerIndex"></param>
        /// <returns></returns>
        public static int GetPoints(int playerIndex)
        {
            return _points[playerIndex];
        }

        /// <summary>
        /// set both players points to 0
        /// </summary>
        public static void ResetPoints()
        {
            for (int i = 0; i < _points.Count; i++)
            {
                _points[i] = 0;
            }
        }
    }
}