using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD;
using FMODUnity;

public class LaserSound : MonoBehaviour
{
    public FMOD.Studio.EventInstance laserSound;

    void Awake()
    {
        laserSound = FMODUnity.RuntimeManager.CreateInstance("event:/Lazer");
        laserSound = FMODUnity.RuntimeManager.CreateInstance("event:/Lazer2");
        laserSound = FMODUnity.RuntimeManager.CreateInstance("event:/Spaceship");
    }
    void Start()
    {
        laserSound.start();
    }
}
