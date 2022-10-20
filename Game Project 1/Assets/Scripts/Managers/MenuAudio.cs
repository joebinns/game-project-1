using FMODUnity;
using Managers.Audio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuAudio : MonoBehaviour
{
    [FMODUnity.EventRef]
    public string music = "event:/MenuEvent";

    public FMOD.Studio.EventInstance instance;
    public FMOD.Studio.EventInstance PlayMenuSound;
    public static AudioManager Instance;

    private void Awake()
    {
        PlayMenuSound = FMODUnity.RuntimeManager.CreateInstance("event:/MenuEvent");
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
