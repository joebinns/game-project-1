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
    /// <summary>
    /// 
    /// 
    /// 
    /// 
    /// 
    ///     HI! hello....
    ///     if youre reading this. don't
    ///     alt + f out of this script
    ///     dont look at it
    ///     its not here
    ///     
    /// 
    /// 
    /// 
    /// 
    /// 
    /// 
    /// 
    /// 
    /// 
    /// 
    /// 
    /// 
    /// 
    /// 
    /// 
    /// 
    /// 
    /// 
    /// 
    /// 
    /// 
    /// 
    /// 
    /// 
    /// 
    /// 
    /// 
    /// 
    /// 
    /// 
    /// 
    /// 
    /// 
    /// 
    /// 
    /// 
    /// 
    /// 
    /// 
    /// 
    /// 
    /// 
    /// 
    /// 
    /// 
    /// 
    /// 
    ///             dont scroll down 
    /// 
    /// 
    /// 
    /// 
    /// 
    /// 
    /// 
    /// 
    /// 
    /// 
    /// 
    /// 
    /// 
    /// 
    /// 
    /// 
    /// 
    /// 
    /// 
    /// 
    /// 
    /// 
    /// 
    /// 
    /// 
    /// 
    /// 
    /// 
    /// 
    /// 
    /// 
    /// 
    /// 
    /// 
    /// 
    /// 
    /// 
    /// 
    ///         ...... go away
    /// 
    /// 
    /// 
    /// 
    /// 
    /// 
    /// 
    /// 
    /// 
    /// </summary>
    [SerializeField] private SkinnedMeshRenderer _body;
    [SerializeField] private MeshRenderer _hemlet;
    [SerializeField] private AudioClip _ouchSound;

    private Material[] body;
    private Material[] bodyhardcodedFUCK;
    private Material[] bodyHurt;
    private Material[] defaulthelmet;
    private Material[] defaulthelmetHurt;

    public Material hurtmaterial;

    [SerializeField] private const float FLASH_DURATION = 0.25f;

    private PhysicsBasedCharacterController characterController;

    private FMOD.Studio.EventInstance PlayOuchSound;
    private FMOD.Studio.EventInstance PlayHoverCrashSound;

    private void Awake()
    {
        characterController = GetComponent<PhysicsBasedCharacterController>();
        PlayOuchSound = FMODUnity.RuntimeManager.CreateInstance("event:/PlayerOuch1");
        PlayHoverCrashSound = FMODUnity.RuntimeManager.CreateInstance("event:/HoverCrash2");

        body = _body.materials;
        bodyhardcodedFUCK = _body.materials;
        defaulthelmet = _hemlet.materials;


        bodyHurt = body;
        defaulthelmetHurt = _hemlet.materials;


        for (int i = 0; i < bodyHurt.Length; i++)
        {
            bodyHurt[i] = hurtmaterial;
        }

        for (int i = 0; i < defaulthelmetHurt.Length; i++)
        {
            defaulthelmetHurt[i] = hurtmaterial;
        }
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

        //CameraManager.Main.Shake(20f, FLASH_DURATION);
    }

    private IEnumerator FlashMaterialCoroutine()
    {
        _body.materials = bodyHurt;
        _hemlet.materials = defaulthelmetHurt;

        yield return new WaitForSeconds(FLASH_DURATION / 3);

        _body.materials = bodyhardcodedFUCK;
        _hemlet.materials = defaulthelmet;

        yield return new WaitForSeconds(FLASH_DURATION / 3);

        _body.materials = bodyHurt;
        _hemlet.materials = defaulthelmetHurt;

        yield return new WaitForSeconds(FLASH_DURATION / 3);

        _body.materials = bodyhardcodedFUCK;
        _hemlet.materials = defaulthelmet;
        /* foreach(Material mat in _body.materials)
         _body.materials[_body.materials.Length-1].SetFloat("_Alpha", 1);
         _hemlet.materials[_hemlet.materials.Length-1].SetFloat("_Alpha", 1);

         yield return new WaitForSeconds(FLASH_DURATION / 3);

         foreach (Material mat in _body.materials)
             _body.materials[_body.materials.Length - 1].SetFloat("_Alpha", 0);
         _hemlet.materials[_hemlet.materials.Length - 1].SetFloat("_Alpha", 0);


         yield return new WaitForSeconds(FLASH_DURATION / 3);

         foreach (Material mat in _body.materials)
             _body.materials[_body.materials.Length - 1].SetFloat("_Alpha", 1);
         _hemlet.materials[_hemlet.materials.Length - 1].SetFloat("_Alpha", 1);


         yield return new WaitForSeconds(FLASH_DURATION / 3);

         foreach (Material mat in _body.materials)
             _body.materials[_body.materials.Length - 1].SetFloat("_Alpha", 0);
         _hemlet.materials[_hemlet.materials.Length - 1].SetFloat("_Alpha", 0);

         */

    }
}
