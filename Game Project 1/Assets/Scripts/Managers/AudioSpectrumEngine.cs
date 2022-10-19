using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class AudioSpectrumEngine : MonoBehaviour
{
    public static float spectrumValue { get; private set; }
    private float[] _audioSpectrum;

    // Start is called before the first frame update
    void Start()
    {
        _audioSpectrum = new float[128];
    }

    // Update is called once per frame
    void Update()
    {
        //AudioListener.GetSpectrumData(_audioSpectrum, 0, FFTWindow.Hamming);
       

        if(_audioSpectrum != null && _audioSpectrum.Length > 0)
        {
            spectrumValue = _audioSpectrum[0] * 100;
        }
    }
}
