using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    private static AudioSource audioSource;

    [Range(0,1)]
    public static float mainVolume = 1;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = mainVolume;
    }




    /// <summary>
    /// play an audio clip
    /// </summary>
    /// <param name="clip">AudioClip</param>
    public static void PlaySound(AudioClip clip)
    {
        audioSource.pitch = 1;
        audioSource.PlayOneShot(clip);
    }

    /// <summary>
    /// play an audio clip with a random pitch (0.9-1.1)
    /// </summary>
    /// <param name="clip">AudioClip</param>
    public static void PlaySoundRandomPitch(AudioClip clip)
    {
        audioSource.pitch = Random.Range(.9f, 1.1f);
        audioSource.PlayOneShot(clip);
    }

    /// <summary>
    /// play an audio clip with volume
    /// </summary>
    /// <param name="clip">AudioClip</param>
    /// <param name="volumeScale">AudioClip</param>
    public static void PlaySound(AudioClip clip, float volumeScale)
    {
        audioSource.pitch = 1;
        audioSource.PlayOneShot(clip, volumeScale);
    }

    /// <summary>
    /// play an audio clip with a random pitch (0.9-1.1) with volume
    /// </summary>
    /// <param name="clip">AudioClip</param>
    /// <param name="volumeScale">AudioClip</param>
    public static void PlaySoundRandomPitch(AudioClip clip, float volumeScale)
    {
        audioSource.pitch = Random.Range(.9f, 1.1f);
        audioSource.PlayOneShot(clip, volumeScale);
    }
}
