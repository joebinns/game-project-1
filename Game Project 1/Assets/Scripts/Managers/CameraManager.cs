using UnityEngine;
using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Tree;

namespace Managers.Camera
{
    public class CameraManager : MonoBehaviour
    {
        static CinemachineVirtualCamera vcam;
        static CinemachineBasicMultiChannelPerlin perlin;
        public static CameraManager Main { get; private set; }
        private void Awake()
        {
            // If there is an instance, and it's not me, delete myself.

            if (Main != null && Main != this)
                Destroy(this);
            else
                Main = this;

            vcam = GetComponent<CinemachineVirtualCamera>();
            perlin = vcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        }

        /// <summary>
        /// add a camera shake following a curve
        /// </summary>
        /// <param name="curve"></param>
        public void Shake(AnimationCurve curve)
        {
            StartCoroutine(ShakeLerp(curve));   
        }
        /// <summary>
        /// add a camera shake linearly
        /// </summary>
        /// <param name="force"></param>
        /// <param name="duration"></param>
        public void Shake(float force = 20f, float duration = 0.35f)
        {
            AnimationCurve newCurve = new AnimationCurve();
            newCurve.AddKey(0, force);
            newCurve.AddKey(duration, 0);
            Shake(newCurve);
        }

        IEnumerator ShakeLerp(AnimationCurve curve)
        {
            float timer = 0;
            float duration = curve[curve.length - 1].time;
            while (timer < duration)
            {
                timer += Time.deltaTime;
                timer = Mathf.Clamp(timer, 0, duration);
                perlin.m_AmplitudeGain = curve.Evaluate(timer);
                yield return null;
            }
        }

        public void LookAt()
        {

        }

        public void Position()
        {

        }

        public void Body()
        {

        }

        public void Aim()
        {

        }
    }

}
