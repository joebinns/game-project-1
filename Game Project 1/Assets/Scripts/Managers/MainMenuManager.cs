using DG.Tweening;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private float menuSwitchSpeed = 1.5f;
    
    [SerializeField] private GameObject mainMenu, settingsMenu, playMenu;
    [SerializeField] private Transform leftPos, middlePos, rightPos;

    private void Start()
    {
        DOTween.Init();
        
        mainMenu.transform.position = middlePos.position;
        settingsMenu.transform.position = leftPos.position;
        playMenu.transform.position = rightPos.position;
    }

    public void MainToSettings()
    {
        mainMenu.transform.DOMove(rightPos.position, menuSwitchSpeed);
        settingsMenu.transform.DOMove(middlePos.position, menuSwitchSpeed);
    }
    
    public void SettingsToMain()
    {
        mainMenu.transform.DOMove(middlePos.position, menuSwitchSpeed);
        settingsMenu.transform.DOMove(leftPos.position, menuSwitchSpeed);
    }

    public void MainToPlay()
    {
        mainMenu.transform.DOMove(leftPos.position, menuSwitchSpeed);
        playMenu.transform.DOMove(middlePos.position, menuSwitchSpeed);
    }
    
    public void PlayToMain()
    {
        mainMenu.transform.DOMove(middlePos.position, menuSwitchSpeed);
        playMenu.transform.DOMove(rightPos.position, menuSwitchSpeed);
    }
}
