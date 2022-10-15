using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class MaterialStorage : MonoBehaviour
{
    private Material[] _defaultMaterials;
    public Material[] DefaultMaterials => _defaultMaterials;

    private void Awake()
    {
        _defaultMaterials = this.GetComponent<Renderer>().materials;
    }
}