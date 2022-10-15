using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

public class FlashMaterial : MonoBehaviour
{
    [SerializeField] private Material _flashMaterial;
    [SerializeField] private List<Transform> _renderers;
    
    private List<Transform> _allRenderers = new List<Transform>();
    private const float FLASH_DURATION = 0.25f;

    public void FlashMaterials()
    {
        StartCoroutine(FlashMaterialCoroutine(_flashMaterial));
    }
    
    private IEnumerator FlashMaterialCoroutine(Material material)
    {
        ChangeMaterials(material);

        yield return new WaitForSeconds(FLASH_DURATION);

        RestoreDefaultMaterials();
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
