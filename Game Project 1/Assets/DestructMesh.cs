using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class DestructMesh : MonoBehaviour
{
    public VisualEffect vfx;
    Material[] mats;
    private void Awake()
    {
        mats = GetComponent<Renderer>().materials;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
            Destruct(2);
    }

    public void Destruct(float PlanePositionZ)
    {
        Debug.Log("test");
        vfx.SetVector4("Color", new Vector4(mats[0].GetColor("_Color").r, mats[0].GetColor("_Color").g, mats[0].GetColor("_Color").b, 1));
        vfx.SetVector3("PlanePosition", Vector3.forward * PlanePositionZ);
        vfx.Play();

        foreach(Material mat in mats)
        {
            mat.SetVector("_Plane_Position", Vector3.forward * PlanePositionZ);
        }
    }
}
