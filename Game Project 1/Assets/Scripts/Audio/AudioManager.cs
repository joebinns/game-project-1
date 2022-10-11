using UnityEngine;

namespace Audio
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioManager : MonoBehaviour
    {
        private static AudioSource _audioSource;
        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
        }

        /// <summary>
        /// play an audio clip
        /// </summary>
        /// <param name="clip">AudioClip</param>
        public static void PlaySound(AudioClip clip)
        {
            _audioSource.pitch = 1;
            _audioSource.PlayOneShot(clip);
        }

        /// <summary>
        /// play an audio clip with a random pitch (0.9-1.1)
        /// </summary>
        /// <param name="clip">AudioClip</param>
        public static void PlaySoundRandomPitch(AudioClip clip)
        {
            _audioSource.pitch = Random.Range(.9f, 1.1f);
            _audioSource.PlayOneShot(clip);
        }
    }
}
