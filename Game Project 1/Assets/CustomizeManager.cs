
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class CustomizeManager : MonoBehaviour
{
    [SerializeField] private GameObject p1CustomizeObj, p2CustomizeObj;
    [SerializeField] private Button startGameBtn;
    
    private RectTransform _p1Transform, _p2Transform;
    private Vector3 _p1Origin, _p2Origin, _p1Lowered, _p2Lowered;

    private bool _p1CustomizeOpen, _p2CustomizeOpen;

    private void Awake()
    {
        _p1CustomizeOpen = false;
        _p2CustomizeOpen = false;
    }

    private void Start()
    {
        DOTween.Init();
        
        _p1Transform = p1CustomizeObj.GetComponent<RectTransform>();
        _p2Transform = p2CustomizeObj.GetComponent<RectTransform>();

        _p1Origin = _p1Transform.position;
        _p2Origin = _p2Transform.position;

        _p1Lowered = new Vector3(_p1Transform.position.x, _p1Transform.position.y - 25, _p1Transform.position.z);
        _p2Lowered = new Vector3(_p2Transform.position.x, _p2Transform.position.y - 25, _p2Transform.position.z);

        _p1Transform.position = _p1Lowered;
        _p2Transform.position = _p2Lowered;
    }

    public void ChangeP1State()
    {
        switch (_p1CustomizeOpen)
        {
            case true:

                _p1Transform.DOMove(_p1Lowered, 1f);
                _p1CustomizeOpen = false;
                
                CheckIfCanStart();
                
                break;
            
            case false:

                _p1Transform.DOMove(_p1Origin, 1f);
                _p1CustomizeOpen = true;
                
                CheckIfCanStart();
                
                break;
        }
    }
    
    public void ChangeP2State()
    {
        switch (_p2CustomizeOpen)
        {
            case true:

                _p2Transform.DOMove(_p2Lowered, 1f);
                _p2CustomizeOpen = false;
                
                CheckIfCanStart();
                
                break;
            
            case false:

                _p2Transform.DOMove(_p2Origin, 1f);
                _p2CustomizeOpen = true;
                
                CheckIfCanStart();
                
                break;
        }
    }

    private void CheckIfCanStart()
    {
        if (_p1CustomizeOpen == false && _p2CustomizeOpen == false)
        {
            startGameBtn.interactable = true;
        }

        else
        {
            startGameBtn.interactable = false;
        }
    }
}
