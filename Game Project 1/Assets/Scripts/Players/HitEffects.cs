using System;
using System.Collections;
using System.Collections.Generic;
using Managers.Audio;
using Managers.Camera;
using UnityEngine;
using Utilities;

public class HitEffects : MonoBehaviour
{
    [SerializeField] private Material _flashMaterial;
    [SerializeField] private List<Transform> _renderers;
    [SerializeField] private AudioClip _ouchSound;

    private List<Vector3> _renderersDefaultLocalScales = new List<Vector3>();
    private List<Transform> _allRenderers = new List<Transform>();
    private const float FLASH_DURATION = 0.25f;

    private void Awake()
    {
        for (int i=0; i < _renderers.Count; i++)
        {
            _renderersDefaultLocalScales.Add(_renderers[i].transform.localScale);
        }
    }

    public void Play()
    {
        AudioManager.PlaySound(_ouchSound);
        StartCoroutine(FlashMaterialCoroutine(_flashMaterial));
        StartCoroutine(FlashRendererSize(1.35f));
        CameraManager.Main.Shake(20f, FLASH_DURATION);
    }
    
    private IEnumerator FlashMaterialCoroutine(Material material)
    {
        ChangeMaterials(material);

        yield return new WaitForSeconds(FLASH_DURATION);

        RestoreDefaultMaterials();
    }
    
    private IEnumerator FlashRendererSize(float size) // TODO: Change this to an eased in-out lerp
    {
        for (int i = 0; i < _renderers.Count; i++)
        {
            _renderers[i].localScale = _renderersDefaultLocalScales[i] * size;
        }

        yield return new WaitForSeconds(FLASH_DURATION);

        for (int i = 0; i < _renderers.Count; i++)
        {
            _renderers[i].localScale = _renderersDefaultLocalScales[i];
        }
    }

    public void RestoreDefaultMaterials()
    {
        UpdateAllRenderers(); // Not sure why this is needed here, since it already gets called when new hats are instantiated.

        foreach (Transform transform in _allRenderers)
        {
            var materialStorage = transform.GetComponent<MaterialStorage>();
            if (materialStorage == null)
            {
                continue;
            }

            var renderer = transform.GetComponent<Renderer>();
            var materials = renderer.materials;
            for (int i = 0; i < renderer.materials.Length; i++)
            {
                materials[i] = materialStorage.DefaultMaterials[i];
            }
            renderer.materials = materials;
        }
    }

    private void ChangeMaterials(Material material)
    {
        UpdateAllRenderers();

        foreach (Transform transform in _allRenderers)
        {
            var renderer = transform.GetComponent<Renderer>();
            if (renderer == null)
            {
                continue;
            }
            
            var materials = renderer.materials;
            for (int i = 0; i < renderer.materials.Length; i++)
            {
                materials[i] = material;
            }
            renderer.materials = materials;
        }
    }
    
    private void UpdateAllRenderers()
    {
        _allRenderers.Clear();
        foreach (Transform renderers in _renderers)
        {
            _allRenderers.Add(renderers);
            int layerMask = Physics.AllLayers;
            UnityUtils.GetAllChildren(renderers.transform, ref _allRenderers, layerMask);
        }
    }
}
