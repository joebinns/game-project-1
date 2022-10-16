using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using Cinemachine;
using Managers.Camera;

public class SpeedVFX : MonoBehaviour
{
    static VisualEffect speedLines;


    private void Awake()
    {
        speedLines = GetComponent<VisualEffect>();

        transform.parent = Camera.main.transform;
    }

    public static void Speed(float speed)
    {
        speedLines.SetFloat("SpawnRate", speed > 35 ? speed * 20:0);
        speedLines.SetFloat("Alpha", Mathf.Lerp(0,1, speed / 75));
        speedLines.SetFloat("SpeedMultiplier", speed * 0.15f);
        CameraManager.Main.currentDefaultShake = Mathf.Lerp(.2f, 4, (speed - 35) / 75);
        Debug.Log("setting Speed");
    }
}
