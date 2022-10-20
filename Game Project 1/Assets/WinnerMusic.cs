using FMODUnity;
using Managers.Audio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinnerMusic : MonoBehaviour
{
    [FMODUnity.EventRef]
    private string music = "event:/WinnerEvent";

    private FMOD.Studio.EventInstance instance;
    private FMOD.Studio.EventInstance PlayWinnerSound;
    public static AudioManager Instance;

    private void Awake()
    {
        PlayWinnerSound = FMODUnity.RuntimeManager.CreateInstance("event:/WinnerEvent");
    }
    private void Start()
    {
        instance = RuntimeManager.CreateInstance(music);
        instance.start();
    }

    private void OnDestroy()
    {
        Debug.Log("Destroyed");
        instance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }
}
