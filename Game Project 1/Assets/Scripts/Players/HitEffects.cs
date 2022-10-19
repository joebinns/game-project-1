using System;
using System.Collections;
using System.Collections.Generic;
using Managers.Audio;
using Managers.Camera;
using UnityEngine;
using Utilities;

public class HitEffects : MonoBehaviour
{
    [SerializeField] private List<Renderer> _renderers;
    [SerializeField] private AudioClip _ouchSound;

    [SerializeField] private const float FLASH_DURATION = 0.25f;

    public void Play()
    {
        AudioManager.PlaySound(_ouchSound);
        StartCoroutine(FlashMaterialCoroutine());
        //CameraManager.Main.Shake(20f, FLASH_DURATION); // Remove screenshake on hit, since it is confusing for two players
    }

    private IEnumerator FlashMaterialCoroutine()
    {
        foreach (Renderer renderer in _renderers)
            renderer.materials[1].SetFloat("_Alpha", 1);

        yield return new WaitForSeconds(FLASH_DURATION / 3);

        foreach (Renderer renderer in _renderers)
            renderer.materials[1].SetFloat("_Alpha", 0);

        yield return new WaitForSeconds(FLASH_DURATION / 3);

        foreach (Renderer renderer in _renderers)
            renderer.materials[1].SetFloat("_Alpha", 1);

        yield return new WaitForSeconds(FLASH_DURATION / 3);


        foreach (Renderer renderer in _renderers)
            renderer.materials[1].SetFloat("_Alpha", 0);
    }
}
