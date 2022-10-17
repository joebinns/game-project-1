using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class SplineLerp : MonoBehaviour
{
    [SerializeField] private Transform[] anchorPoints;
    [SerializeField] private GameObject objectToLerp;
    [SerializeField][Range(0, 2.5f)] private float lerpSpeed;

    private float _t = 0;

    [SerializeField] private bool shouldLerp;
    [SerializeField] private bool shouldDrawAnchorLines;
    private bool _forward = true;

    private Vector3 A, B, C, D, E;

    public enum LerpMode
    {
        LerpOnce,
        LoopingLerp,
        YoYoLerp
    }

    public LerpMode activeLerpMode;

    private void Start()
    {
        objectToLerp.transform.position = anchorPoints[0].transform.position;
    }

    private void Update()
    {
        if (shouldDrawAnchorLines)
        {
            Debug.DrawRay(anchorPoints[0].position, anchorPoints[1].position - anchorPoints[0].position, Color.green, .1f);
            Debug.DrawRay(anchorPoints[1].position, anchorPoints[2].position - anchorPoints[1].position, Color.green, .1f);
            Debug.DrawRay(anchorPoints[2].position, anchorPoints[3].position - anchorPoints[2].position, Color.green, .1f);
        }

        if (shouldLerp)
        {
            switch (activeLerpMode)
            {
                case LerpMode.LerpOnce:

                    A = Vector3.Lerp(anchorPoints[0].position, anchorPoints[1].position, _t);
                    B = Vector3.Lerp(anchorPoints[1].position, anchorPoints[2].position, _t);
                    C = Vector3.Lerp(anchorPoints[2].position, anchorPoints[3].position, _t);
                    
                    D = Vector3.Lerp(A, B, _t);
                    E = Vector3.Lerp(B, C, _t);

                    objectToLerp.transform.position = Vector3.Lerp(D, E, _t);

                    _t += Time.deltaTime * lerpSpeed;
                    
                    break;
                
                case LerpMode.LoopingLerp:

                    A = Vector3.Lerp(anchorPoints[0].position, anchorPoints[1].position, _t);
                    B = Vector3.Lerp(anchorPoints[1].position, anchorPoints[2].position, _t);
                    C = Vector3.Lerp(anchorPoints[2].position, anchorPoints[3].position, _t);
                    
                    D = Vector3.Lerp(A, B, _t);
                    E = Vector3.Lerp(B, C, _t);

                    objectToLerp.transform.position = Vector3.Lerp(D, E, _t);

                    _t += Time.deltaTime * lerpSpeed;

                    if (objectToLerp.transform.position == anchorPoints[3].position)
                    {
                        _t = 0;
                        objectToLerp.transform.position = anchorPoints[0].position;
                    }
                    
                    break;

                case LerpMode.YoYoLerp:

                    switch (_forward)
                    {
                        case true:

                            A = Vector3.Lerp(anchorPoints[0].position, anchorPoints[1].position, _t);
                            B = Vector3.Lerp(anchorPoints[1].position, anchorPoints[2].position, _t);
                            C = Vector3.Lerp(anchorPoints[2].position, anchorPoints[3].position, _t);
                    
                            D = Vector3.Lerp(A, B, _t);
                            E = Vector3.Lerp(B, C, _t);

                            objectToLerp.transform.position = Vector3.Lerp(D, E, _t);

                            _t += Time.deltaTime * lerpSpeed;

                            if (objectToLerp.transform.position == anchorPoints[3].position)
                            {
                                _t = 0;
                                _forward = false;
                            }
                            
                            break;
                        
                        case false:

                            A = Vector3.Lerp(anchorPoints[3].position, anchorPoints[2].position, _t);
                            B = Vector3.Lerp(anchorPoints[2].position, anchorPoints[1].position, _t);
                            C = Vector3.Lerp(anchorPoints[1].position, anchorPoints[0].position, _t);
                    
                            D = Vector3.Lerp(A, B, _t);
                            E = Vector3.Lerp(B, C, _t);

                            objectToLerp.transform.position = Vector3.Lerp(D, E, _t);

                            _t += Time.deltaTime * lerpSpeed;
                            
                            if (objectToLerp.transform.position == anchorPoints[0].position)
                            {
                                _t = 0;
                                _forward = true;
                            }
                            
                            break;
                    }
                    
                    break;
            }
        }
    }

    public void StartLerp()
    {
        shouldLerp = true;
    }
}
