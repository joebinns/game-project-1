
using System;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class CustomizeManager : MonoBehaviour
{
    [SerializeField] private Material hitMat;
    
    [SerializeField] private GameObject player1Model, player2Model, p1Helm, p2Helm;
    
    [SerializeField] private GameObject p1CustomizeObj, p2CustomizeObj;
    [SerializeField] private Button startGameBtn, backToMainBtn;

    public Material[] helmetColorMats, jacketMats, pantMats, shoeMats, skinMats;
    public GameObject[] helmetModels, hoverboardModels;
    public GameObject[] p1HoverBoardsInScene, p2HoverBoardsInScene;

    public int p1HelmColorCounter, p1JacketCounter, p1PantCounter, p1ShoeCounter, p1SkinCounter, p1HelmModelCounter, p1HoverboardCounter;
    public int p2HelmColorCounter, p2JacketCounter, p2PantCounter, p2ShoeCounter, p2SkinCounter, p2HelmModelCounter, p2HoverboardCounter;

    public Material p1HelmColor, p1Jacket, p1Pant, p1Shoe, p1Skin, p2HelmColor, p2Jacket, p2Pant, p2Shoe, p2Skin;
    public GameObject p1HelmModel, p1Hoverboard, p2HelmModel, p2Hoverboard;
    
    private RectTransform _p1Transform, _p2Transform;
    private Vector3 _p1Origin, _p2Origin, _p1Lowered, _p2Lowered;

    private bool _p1CustomizeOpen, _p2CustomizeOpen;

    private void Awake()
    {
        ResetPlayerCustomization();

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

        foreach (GameObject hoverboard in p1HoverBoardsInScene)
        {
         hoverboard.SetActive(false);

             if (hoverboard == p1HoverBoardsInScene[0])
             {
                 hoverboard.SetActive(true);
             }
         
        }
        
        foreach (GameObject hoverboard in p2HoverBoardsInScene)
        {
            hoverboard.SetActive(false);

            if (hoverboard == p2HoverBoardsInScene[0])
            {
                hoverboard.SetActive(true);
            }
         
        }

        p1Hoverboard = hoverboardModels[0];
        p2Hoverboard = hoverboardModels[0];

        //Take back when helmets are in
        /*p1Helm = helmetModels[0];
        p2Helm = helmetModels[0];*/

        ApplyMaterials();
        ApplyModels();
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
            backToMainBtn.interactable = true;
        }

        else
        {
            startGameBtn.interactable = false;
            backToMainBtn.interactable = false;
        }
    }

    public void ChangeLook(string customSettings)
    {
        char[] settingsArray = customSettings.ToCharArray();

        switch (settingsArray[0])
        {
            //Helmet Color
            case '0':

                switch (settingsArray[1])
                {
                    //Player 1 customize change
                    case '1':

                        p1HelmColorCounter++;
                        if (p1HelmColorCounter == helmetColorMats.Length)
                        {
                            p1HelmColorCounter = 0;
                        }

                        p1HelmColor = helmetColorMats[p1HelmColorCounter];

                        break;
                    
                    //Player2 customize change
                    case '2':

                        p2HelmColorCounter++;
                        if (p2HelmColorCounter == helmetColorMats.Length)
                        {
                            p2HelmColorCounter = 0;
                        }

                        p2HelmColor = helmetColorMats[p2HelmColorCounter];
                        
                        break;
                }
                
                break;
            
            //Helmet Model
            case '1':

                switch (settingsArray[1])
                {
                    //Player 1 customize change
                    case '1':

                        if (customSettings[2] == '1')
                        {
                            p1HelmModelCounter++;
                            if (p1HelmModelCounter == helmetModels.Length)
                            {
                                p1HelmModelCounter = 0;
                            }

                            p1HelmModel = helmetModels[p1HelmModelCounter];
                        }

                        else
                        {
                            p1HelmModelCounter--;
                            if (p1HelmModelCounter < 0)
                            {
                                p1HelmModelCounter = helmetModels.Length - 1;
                            }

                            p1HelmModel = helmetModels[p1HelmModelCounter];
                        }
                        
                        break;
                    
                    //Player2 customize change
                    case '2':

                        if (customSettings[2] == '1')
                        {
                            p2HelmModelCounter++;
                            if (p2HelmModelCounter == helmetModels.Length)
                            {
                                p2HelmModelCounter = 0;
                            }

                            p2HelmModel = helmetModels[p2HelmModelCounter];
                        }

                        else
                        {
                            p2HelmModelCounter--;
                            if (p2HelmModelCounter < 0)
                            {
                                p2HelmModelCounter = helmetModels.Length - 1;
                            }

                            p2HelmModel = helmetModels[p2HelmModelCounter];
                        }
                        
                        break;
                }
                
                break;
            
            //Jacket
            case '2':

                switch (settingsArray[1])
                {
                    //Player 1 customize change
                    case '1':

                        if (customSettings[2] == '1')
                        {
                            p1JacketCounter++;
                            if (p1JacketCounter == jacketMats.Length)
                            {
                                p1JacketCounter = 0;
                            }

                            p1Jacket = jacketMats[p1JacketCounter];
                        }

                        else
                        {
                            p1JacketCounter--;
                            if (p1JacketCounter < 0)
                            {
                                p1JacketCounter = jacketMats.Length - 1;
                            }

                            p1Jacket = jacketMats[p1JacketCounter];
                        }
                        
                        break;
                    
                    //Player2 customize change
                    case '2':

                        if (customSettings[2] == '1')
                        {
                            p2JacketCounter++;
                            if (p2JacketCounter == jacketMats.Length)
                            {
                                p2JacketCounter = 0;
                            }

                            p2Jacket = jacketMats[p2JacketCounter];
                        }

                        else
                        {
                            p2JacketCounter--;
                            if (p2JacketCounter < 0)
                            {
                                p2JacketCounter = jacketMats.Length - 1;
                            }

                            p2Jacket = jacketMats[p2JacketCounter];
                        }
                        
                        break;
                }
                
                break;
            
            //Pants
            case '3':

                switch (settingsArray[1])
                {
                    //Player 1 customize change
                    case '1':

                        if (customSettings[2] == '1')
                        {
                            p1PantCounter++;
                            if (p1PantCounter == pantMats.Length)
                            {
                                p1PantCounter = 0;
                            }

                            p1Pant = pantMats[p1PantCounter];
                        }

                        else
                        {
                            p1PantCounter--;
                            if (p1PantCounter < 0)
                            {
                                p1PantCounter = pantMats.Length - 1;
                            }

                            p1Pant = pantMats[p1PantCounter];
                        }
                        
                        break;
                    
                    //Player2 customize change
                    case '2':

                        if (customSettings[2] == '1')
                        {
                            p2PantCounter++;
                            if (p2PantCounter == pantMats.Length)
                            {
                                p2PantCounter = 0;
                            }

                            p2Pant = pantMats[p2PantCounter];
                        }

                        else
                        {
                            p2PantCounter--;
                            if (p2PantCounter < 0)
                            {
                                p2PantCounter = pantMats.Length - 1;
                            }

                            p2Pant = pantMats[p2PantCounter];
                        }
                        
                        break;
                }
                
                break;
            
            //Shoes
            case '4':

                switch (settingsArray[1])
                {
                    //Player 1 customize change
                    case '1':

                        if (customSettings[2] == '1')
                        {
                            p1ShoeCounter++;
                            if (p1ShoeCounter == shoeMats.Length)
                            {
                                p1ShoeCounter = 0;
                            }

                            p1Shoe = shoeMats[p1ShoeCounter];
                        }

                        else
                        {
                            p1ShoeCounter--;
                            if (p1ShoeCounter < 0)
                            {
                                p1ShoeCounter = shoeMats.Length - 1;
                            }

                            p1Shoe = shoeMats[p1ShoeCounter];
                        }

                        break;

                    //Player2 customize change
                    case '2':

                        if (customSettings[2] == '1')
                        {
                            p2ShoeCounter++;
                            if (p2ShoeCounter == shoeMats.Length)
                            {
                                p2ShoeCounter = 0;
                            }

                            p2Shoe = shoeMats[p2ShoeCounter];
                        }

                        else
                        {
                            p2ShoeCounter--;
                            if (p2ShoeCounter < 0)
                            {
                                p2ShoeCounter = shoeMats.Length - 1;
                            }

                            p2Shoe = shoeMats[p2ShoeCounter];
                        }

                        break;
                }

                break;

            //Skin
            case '5':

                switch (settingsArray[1])
                {
                    //Player 1 customize change
                    case '1':

                        break;
                    
                    //Player2 customize change
                    case '2':

                        break;
                }
                
                break;
            
            //Hoverboard
            case '6':

                switch (settingsArray[1])
                {
                    //Player 1 customize change
                    case '1':

                        if (customSettings[2] == '1')
                        {
                            p1HoverboardCounter++;
                            if (p1HoverboardCounter == hoverboardModels.Length)
                            {
                                p1HoverboardCounter = 0;
                            }

                            p1Hoverboard = hoverboardModels[p1HoverboardCounter];

                            foreach (GameObject hoverboard in p1HoverBoardsInScene)
                            {
                                hoverboard.SetActive(false);
                                if (hoverboard == p1HoverBoardsInScene[p1HoverboardCounter])
                                {
                                    hoverboard.SetActive(true);
                                }
                            }
                        }

                        else
                        {
                            p1HoverboardCounter--;
                            if (p1HoverboardCounter < 0)
                            {
                                p1HoverboardCounter = hoverboardModels.Length - 1;
                            }

                            p1Hoverboard = hoverboardModels[p1HoverboardCounter];
                            
                            foreach (GameObject hoverboard in p1HoverBoardsInScene)
                            {
                                hoverboard.SetActive(false);
                                if (hoverboard == p1HoverBoardsInScene[p1HoverboardCounter])
                                {
                                    hoverboard.SetActive(true);
                                }
                            }
                        }

                        break;

                    //Player2 customize change
                    case '2':

                        if (customSettings[2] == '1')
                        {
                            p2HoverboardCounter++;
                            if (p2HoverboardCounter == hoverboardModels.Length)
                            {
                                p2HoverboardCounter = 0;
                            }

                            p2Hoverboard = hoverboardModels[p2HoverboardCounter];

                            foreach (GameObject hoverboard in p2HoverBoardsInScene)
                            {
                                hoverboard.SetActive(false);
                                if (hoverboard == p2HoverBoardsInScene[p2HoverboardCounter])
                                {
                                    hoverboard.SetActive(true);
                                }
                            }
                        }

                        else
                        {
                            p2HoverboardCounter--;
                            if (p2HoverboardCounter < 0)
                            {
                                p2HoverboardCounter = hoverboardModels.Length - 1;
                            }

                            p2Hoverboard = hoverboardModels[p2HoverboardCounter];
                            
                            foreach (GameObject hoverboard in p2HoverBoardsInScene)
                            {
                                hoverboard.SetActive(false);
                                if (hoverboard == p2HoverBoardsInScene[p2HoverboardCounter])
                                {
                                    hoverboard.SetActive(true);
                                }
                            }
                        }

                        break;
                }
                
                break;
        }
        
        ApplyMaterials();
        ApplyModels();
    }

    public void ResetPlayerCustomization()
    {
        p1HelmColorCounter = 0;
        p1HelmModelCounter = 0;
        p1JacketCounter = 0;
        p1PantCounter = 0;
        p1ShoeCounter = 0;
        p1SkinCounter = 0;
        p1HoverboardCounter = 0;
        
        p2HelmColorCounter = 0;
        p2HelmModelCounter = 0;
        p2JacketCounter = 0;
        p2PantCounter = 0;
        p2ShoeCounter = 0;
        p2SkinCounter = 0;
        p2HoverboardCounter = 0;
        
        
        //changing the actual materials back to default

        p1HelmColor = helmetColorMats[0];
        p1Jacket = jacketMats[0];
        p1Pant = pantMats[0];
        p1Shoe = shoeMats[0];
        
        p2HelmColor = helmetColorMats[0];
        p2Jacket = jacketMats[0];
        p2Pant = pantMats[0];
        p2Shoe = shoeMats[0];

    }

    public void ApplyMaterials()
    {
        player1Model.GetComponent<MeshRenderer>().materials = GetPlayerMats(1);
        player2Model.GetComponent<MeshRenderer>().materials = GetPlayerMats(2);

        p1Helm.GetComponent<MeshRenderer>().material = p1HelmColor;
        p2Helm.GetComponent<MeshRenderer>().material = p2HelmColor;

        GameSettings.Instance.p1Materials = GetPlayerMats(1);
        GameSettings.Instance.p2Materials = GetPlayerMats(2);

        GameSettings.Instance.p1HelmMat = p1HelmColor;
        GameSettings.Instance.p2HelmMat = p2HelmColor;
    }

    public void ApplyModels()
    {
        GameSettings.Instance.ApplyPlayerObjects(p1Hoverboard, p2Hoverboard, p1Helm, p2Helm);
    }

    private Material[] GetPlayerMats(int playerID)
    {
        Material[] toReturn = new Material[7];
        
        switch (playerID)
        {
            case 1:
                //Antenna
                toReturn[0] = p1HelmColor;
                //Jacket
                toReturn[1] = p1Jacket;
                toReturn[2] = p1Jacket;
                //Shoes
                toReturn[3] = p1Shoe;
                //Pants
                toReturn[4] = p1Pant;
                //Jacket
                toReturn[5] = p1Jacket;
                
                //HitEffectMat
                toReturn[6] = hitMat;
                break;
            
            case 2:
                //Antenna
                toReturn[0] = p2HelmColor;
                //Jacket
                toReturn[1] = p2Jacket;
                toReturn[2] = p2Jacket;
                //Shoes
                toReturn[3] = p2Shoe;
                //Pants
                toReturn[4] = p2Pant;
                //Jacket
                toReturn[5] = p2Jacket;
                
                //HitEffectMat
                toReturn[6] = hitMat;
                break;
        }

        return toReturn;
    }

}
