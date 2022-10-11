using UnityEngine;

namespace Managers.Points
{
    public static class PointsManager
    {
        private static int p1Points, p2Points;

        /// <summary>
        /// add points to a specified player
        /// </summary>
        /// <param name="playerIndex">the player to send points to, 1-2</param>
        /// <param name="points">the amount</param>
        public static void GainPoints(int playerIndex, int points)
        {
            switch (playerIndex)
            {
                case 1:
                    p1Points += points;
                    break;

                case 2:
                    p2Points += points;
                    break;

                default:
                    Debug.LogError($"No player with index {playerIndex}. cannot give points");
                    break;
            }
        }

        /// <summary>
        /// subtract points from a specified player
        /// </summary>
        /// <param name="playerIndex">the player to remove points from, 1-2</param>
        /// <param name="points">the amount</param>
        public static void RemovePoints(int playerIndex, int points)
        {
            switch (playerIndex)
            {
                case 1:
                    p1Points -= points;
                    break;

                case 2:
                    p2Points -= points;
                    break;

                default:
                    Debug.LogError($"No player with index {playerIndex}. cannot remove points");
                    break;
            }
        }

        /// <summary>
        /// return the current amount of points the specified player has
        /// </summary>
        /// <param name="playerIndex"></param>
        /// <returns></returns>
        public static int GetPoints(int playerIndex)
        {
            switch (playerIndex)
            {
                case 1:
                    return p1Points;

                case 2:
                    return p2Points;

                default:
                    Debug.LogError($"No player with index {playerIndex}. cannot get points");
                    return 0;
            }
        }

        /// <summary>
        /// set both players points to 0
        /// </summary>
        public static void ResetPoints()
        {
            p1Points = 0;
            p2Points = 0;
        }


    }
}