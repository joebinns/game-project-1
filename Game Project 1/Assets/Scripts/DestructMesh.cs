using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class DestructMesh : MonoBehaviour
{
    VisualEffect vfx;
    Material mat;
    Mesh mesh;
    private void Awake()
    {
        vfx = GetComponent<VisualEffect>();
        mat = transform.parent.GetComponent<Renderer>().material;
        mesh = transform.parent.GetComponent<MeshFilter>().mesh;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
            Destruct();
    }

    public void Destruct()
    {
        Debug.Log("test");
       // vfx.SetVector4("Color", new Vector4(mat.GetColor("_Color").r, mat.GetColor("_Color").g, mat.GetColor("_Color").b, 1));
        vfx.SetMesh("Mesh", mesh);
        vfx.SetVector3("collider plane pos", transform.position + (Vector3.down * 1));
        vfx.Play();

        transform.parent.GetComponent<MeshRenderer>().enabled = false;
    }
}
