using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class vfxPlayer : MonoBehaviour
{
    [SerializeField] private VisualEffect[] effectsToPlay;

    private void OnTriggerEnter(Collider other)
    {
        foreach (VisualEffect effect in effectsToPlay)
        {
            effect.SendEvent("OnPlay");
        }
    }
}
