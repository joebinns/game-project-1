using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using Players;
using UnityEngine;
using Utilities;

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
                Destroy(this.gameObject);
            }
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }

        /// <summary>
        /// add points to a specifdied player
        /// </summary>
        /// <param name="playerIndex">the player to send points to, 1-2</param>
        /// <param name="points">the amount</param>
        public void ChangePoints(Player player, int points, MultiplierMode multiplierMode)
        {
            if (multiplierMode == MultiplierMode.Reset)
            {
                _consecutiveGains[player.ID] = 0;
            }

            int multipliedPoints = points;
            if (multiplierMode != MultiplierMode.None)
            {
                multipliedPoints = (int)(multipliedPoints * (1f + 0.1f * _consecutiveGains[player.ID]));
            }
            
            //multipliedPoints = MathsUtils.RoundOff(multipliedPoints); // Round points to nearest 10.
            _points[player.ID] += multipliedPoints;

            if (multiplierMode == MultiplierMode.Increment)
            {
                _consecutiveGains[player.ID]++;
            }
            
            float textSide = 0;
            switch (player.ID)
            {
                case 0:
                    textSide = 1f;
                    break;
                case 1:
                    textSide = -1f;
                    break;
            }

            float textSize = 50f;
            if (multiplierMode != MultiplierMode.None)
            {
                var maxTextSize = 100f;
                if (textSize * (1f + 0.1f * _consecutiveGains[player.ID]) < maxTextSize)
                {
                    textSize *= (1f + 0.1f * _consecutiveGains[player.ID]);
                }
                else
                {
                    textSize = maxTextSize;
                }
            }

            var text = "<size=" + (textSize).ToString() + ">" + multipliedPoints.ToString();
            var position = Vector3.zero;
            if (multiplierMode == MultiplierMode.None)
            {
                position = (UnityEngine.Random.insideUnitSphere + (Vector3.one * 0.5f * textSide)) * 1.5f;
            }
            position += player.transform.position + Vector3.right * textSide * 2f + Vector3.up * 1.5f;
            FloatingTextManager.Instance.SpawnText(text, position, (1f + 0.1f * _consecutiveGains[player.ID]), 1, Color.white);
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
        
        public float GetPointsMultiplier(int playerIndex)
        {
            var pointsMultiplier = (1f + 0.1f * _consecutiveGains[playerIndex]);
            return pointsMultiplier;
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

public enum MultiplierMode
{
    None,
    Reset,
    Increment,
}