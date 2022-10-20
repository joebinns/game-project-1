using System;
using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using Managers.Audio;
using Managers.Camera;
using Players.Physics_Based_Character_Controller;
using UnityEngine;
using UnityEngine.VFX;
using Utilities;

public class HitEffects : MonoBehaviour
{
    [SerializeField] private SkinnedMeshRenderer _body;
    [SerializeField] private MeshRenderer _hemlet;
    [SerializeField] private AudioClip _ouchSound;

    [SerializeField] private const float FLASH_DURATION = 0.25f;

    private PhysicsBasedCharacterController characterController;

    private FMOD.Studio.EventInstance PlayOuchSound;
    private FMOD.Studio.EventInstance PlayHoverCrashSound;

    private void Awake()
    {
        characterController = GetComponent<PhysicsBasedCharacterController>();
        PlayOuchSound = FMODUnity.RuntimeManager.CreateInstance("event:/PlayerOuch1");
        PlayHoverCrashSound = FMODUnity.RuntimeManager.CreateInstance("event:/PlayerOuch2");
    }
    public void Play()
    {
        if (_ouchSound != null)
        {
            //AudioManager.Instance.PlaySound(_ouchSound);
            PlayHoverCrashSound.start();
            //characterController.PlayHoverSoundLoop.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        }

            StartCoroutine(FlashMaterialCoroutine());
        PlayHoverCrashSound.start();
        //CameraManager.Main.Shake(20f, FLASH_DURATION);
    }

    private IEnumerator FlashMaterialCoroutine()
    {

        _body.materials[_body.materials.Length-1].SetFloat("_Alpha", 1);
        _hemlet.materials[_hemlet.materials.Length-1].SetFloat("_Alpha", 1);

        yield return new WaitForSeconds(FLASH_DURATION / 3);

        _body.materials[_body.materials.Length - 1].SetFloat("_Alpha", 0);
        _hemlet.materials[_hemlet.materials.Length - 1].SetFloat("_Alpha", 0);


        yield return new WaitForSeconds(FLASH_DURATION / 3);

        _body.materials[_body.materials.Length - 1].SetFloat("_Alpha", 1);
        _hemlet.materials[_hemlet.materials.Length - 1].SetFloat("_Alpha", 1);


        yield return new WaitForSeconds(FLASH_DURATION / 3);

        _body.materials[_body.materials.Length - 1].SetFloat("_Alpha", 0);
        _hemlet.materials[_hemlet.materials.Length - 1].SetFloat("_Alpha", 0);



    }
}
