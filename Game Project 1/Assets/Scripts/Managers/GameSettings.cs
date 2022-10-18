
using Managers.Points;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSettings : MonoBehaviour
{
    public static GameSettings Instance;

    public int player1Points, player2Points;

    public Material[] p1Materials, p2Materials;
    public Material p1HelmMat, p2HelmMat;
    public GameObject p1, p2, p1Helm, p2Helm;

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

    public void ApplyPlayerMats()
    {
        p1.GetComponent<Renderer>().materials = p1Materials;
        p2.GetComponent<Renderer>().materials = p2Materials;

        p1Helm.GetComponent<Renderer>().material = p1HelmMat;
        p2Helm.GetComponent<Renderer>().material = p2HelmMat;
    }
}
