using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using Players;
using UnityEngine;

namespace Managers.Points
{
    public class PointsManager : MonoBehaviour
    {
        public static PointsManager Instance;
        
        private int[] _points = new int[2];
        private int[] _consecutiveGains = new int[2];

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(this);
            }
            Instance = this;
        }

        /// <summary>
        /// add points to a specifdied player
        /// </summary>
        /// <param name="playerIndex">the player to send points to, 1-2</param>
        /// <param name="points">the amount</param>
        public void ChangePoints(Player player, int points)
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

            float textSide = 0;
            switch (player.ID)
            {
                case 0:
                    textSide = -1f;
                    break;
                case 1:
                    textSide = +1f;
                    break;
            }
            FloatingTextManager.Instance.SpawnText(multipliedPoints.ToString(), player.transform.position + Vector3.right * textSide * 5 + Vector3.up * 3, 1, 1, Color.white);
        }

        /// <summary>
        /// return the current amount of points the specified player has
        /// </summary>
        /// <param name="playerIndex"></param>
        /// <returns></returns>
        public int GetPoints(int playerIndex)
        {
            return _points[playerIndex];
        }

        /// <summary>
        /// set both players points to 0
        /// </summary>
        public void ResetPoints()
        {
            for (int i = 0; i < _points.Length; i++)
            {
                _points[i] = 0;
            }
        }
    }
}