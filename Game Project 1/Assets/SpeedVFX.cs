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
        Debug.Log(speed);
        speedLines.SetFloat("SpawnRate", speed > 34 ? Mathf.Lerp(0,200, (speed - 34) / 75) * 8: 0);
        speedLines.SetFloat("Alpha", Mathf.Lerp(0,1, speed / 75));
        speedLines.SetFloat("SpeedMultiplier", speed * 0.10f);
        speedLines.SetFloat("Radius", speed > 75 ? 0.5f:1);
        CameraManager.Main.CurrentDefaultShake = Mathf.Lerp(0, 4, (speed - 25) / 75);
    }
}
