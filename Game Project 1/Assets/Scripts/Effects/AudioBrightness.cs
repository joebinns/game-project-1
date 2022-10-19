using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioBrightness : AudioVisualizer
{
    public Color TargetColor;
    public float targetBrightness;

    private Material mat;
    private Color defaultColor;

    private void Awake()
    {
        mat = GetComponent<Renderer>().material;
        defaultColor = mat.color;
    }
    public override void OnBeat()
    {
        base.OnBeat();

        StopCoroutine("moveToBrightness");
        StartCoroutine(moveToBrightness(TargetColor * targetBrightness));
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        if (_isBeat) return;
        mat.SetColor("_Color", Color.Lerp(mat.color, defaultColor, restSmoothTime * Time.deltaTime));

    }

    IEnumerator moveToBrightness(Color target)
    {
        Color current = mat.color;
        Color init = current;
        float timer = 0;

        while(current != target)
        {
            current = Color.Lerp(init, target, timer / timeToBeat);
            timer += Time.deltaTime;

            mat.SetColor("_Color", current);

            yield return null;
        }

        _isBeat = false;
    }
}
