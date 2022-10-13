
using Managers.Points;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSettings : MonoBehaviour
{
    public static GameSettings Instance;

    public int player1Points, player2Points;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(Instance);
            Instance = this;
        }

        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        ResetPoints();
    }

    public void SetPlayerPoints(int p1Points, int p2Points)
    {
        player1Points = p1Points;
        player2Points = p2Points;
    }

    public void ResetPoints()
    {
        PointsManager.ResetPoints();
        player1Points = 0;
        player2Points = 0;
    }

    public void SwitchScene(int sceneBuildID)
    {
        SceneManager.LoadScene(sceneBuildID);
    }
}
