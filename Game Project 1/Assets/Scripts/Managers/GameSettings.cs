
using Managers.Points;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSettings : MonoBehaviour
{
    public GameObject finalPlayer1, finalPlayer2;
    
    public static GameSettings Instance;

    public int player1Points, player2Points;

    public Material[] p1Materials, p2Materials;
    public Material p1HelmMat, p2HelmMat, p1DetailMat, p2DetailMat;
    public GameObject p1, p2, p1Helm, p2Helm, p1Hoverboard, p2Hoverboard, p1DetailSlot, p2DetailSlot;
    public GameObject p1SelectedBoard, p2SelectedBoard, p1SelectedHelm, p2SelectedHelm;

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
        PointsManager.Instance.ResetPoints();
        player1Points = 0;
        player2Points = 0;
    }

    public void SetFinalPlayers(GameObject p1, GameObject p2)
    {
        finalPlayer1 = p1;
        finalPlayer2 = p2;
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

        Renderer p1renderer = p1SelectedHelm.GetComponent<Renderer>();

        if (p1renderer != null)
        {
            p1renderer.material = p1DetailMat;
        }

        else
        {
            p1SelectedHelm.GetComponentInChildren<Renderer>().material = p1DetailMat;
        }
        
        Renderer p2renderer = p2SelectedHelm.GetComponent<Renderer>();

        if (p2renderer != null)
        {
            p2renderer.material = p2DetailMat;
        }

        else
        {
            p2SelectedHelm.GetComponentInChildren<Renderer>().material = p2DetailMat;
        }
    }

    public void ApplyPlayerObjects(GameObject p1hover, GameObject p2hover, GameObject p1helm, GameObject p2helm, GameObject p1Detail, GameObject p2Detail)
    {
        p1SelectedBoard = p1hover;
        p2SelectedBoard = p2hover;
        p1SelectedHelm = p1helm;
        p2SelectedHelm = p2helm;
        p1SelectedHelm = p1Detail;
        p2SelectedHelm = p2Detail;
    }

    public void ApplyPlayerThings()
    {
        //Hoverboard
        Transform p1HoverTransform = p1Hoverboard.transform;
        Transform p2HoverTransform = p2Hoverboard.transform;
        
        GameObject newP1Hover = Instantiate(p1SelectedBoard, p1Hoverboard.transform.position, Quaternion.identity);
        newP1Hover.transform.parent = p1Hoverboard.transform.parent;
        Destroy(p1Hoverboard);
        newP1Hover.transform.position = p1HoverTransform.position;
        newP1Hover.transform.rotation = p1HoverTransform.rotation;
        newP1Hover.transform.localScale = p1HoverTransform.localScale;
        
        GameObject newP2Hover = Instantiate(p2SelectedBoard, p2Hoverboard.transform.position, Quaternion.identity);
        newP2Hover.transform.parent = p2Hoverboard.transform.parent;
        Destroy(p2Hoverboard);
        newP2Hover.transform.position = p2HoverTransform.position;
        newP2Hover.transform.rotation = p2HoverTransform.rotation;
        newP2Hover.transform.localScale = p2HoverTransform.localScale;

        //Helmet
        Transform p1HelmetTransform = p1DetailSlot.transform;
        Transform p2HelmetTransform = p2DetailSlot.transform;

        GameObject newP1Detail = Instantiate(p1SelectedHelm, p1Helm.transform.position, Quaternion.identity);
        newP1Detail.transform.parent = p1DetailSlot.transform.parent;
        newP1Detail.transform.position = p1HelmetTransform.position;
        newP1Detail.transform.rotation = p1HelmetTransform.transform.rotation;
        newP1Detail.transform.localScale = p1HelmetTransform.transform.localScale;
        
        GameObject newP2Detail = Instantiate(p2SelectedHelm, p2Helm.transform.position, Quaternion.identity);
        newP2Detail.transform.parent = p2DetailSlot.transform.parent;
        newP2Detail.transform.position = p2HelmetTransform.position;
        newP2Detail.transform.rotation = p2HelmetTransform.transform.rotation;
        newP2Detail.transform.localScale = p2HelmetTransform.transform.localScale;
    }
}
