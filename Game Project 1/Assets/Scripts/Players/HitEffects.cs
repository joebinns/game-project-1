using System;
using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using Managers.Audio;
using Managers.Camera;
using UnityEngine;
using UnityEngine.VFX;
using Utilities;

public class HitEffects : MonoBehaviour
{
    [SerializeField] private List<Renderer> _renderers;
    [SerializeField] private AudioClip _ouchSound;

    [SerializeField] private const float FLASH_DURATION = 0.25f;

    FMOD.Studio.EventInstance PlayOuchSound;

    private void Awake()
    {
        PlayOuchSound = FMODUnity.RuntimeManager.CreateInstance("event:/PlayerOuch1");
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.S))
        {
            PlayOuchSound.start();
        }
    }
    public void Play()
    {
        if (_ouchSound != null)
        {
            //AudioManager.Instance.PlaySound(_ouchSound);
        }
        if (_renderers.Count <= 0)
        {
            StartCoroutine(FlashMaterialCoroutine());
        }
        //CameraManager.Main.Shake(20f, FLASH_DURATION);
    }

    private IEnumerator FlashMaterialCoroutine()
    {
        foreach (Renderer renderer in _renderers)
            renderer.materials[renderer.materials.Length-1].SetFloat("_Alpha", 1);

        yield return new WaitForSeconds(FLASH_DURATION / 3);

        foreach (Renderer renderer in _renderers)
            renderer.materials[renderer.materials.Length-1].SetFloat("_Alpha", 0);

        yield return new WaitForSeconds(FLASH_DURATION / 3);

        foreach (Renderer renderer in _renderers)
            renderer.materials[renderer.materials.Length - 1].SetFloat("_Alpha", 1);

        yield return new WaitForSeconds(FLASH_DURATION / 3);


        foreach (Renderer renderer in _renderers)
            renderer.materials[renderer.materials.Length - 1].SetFloat("_Alpha", 0);
    }
}
