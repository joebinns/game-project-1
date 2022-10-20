using System.Collections;

using UnityEngine;

using FMODUnity;


namespace Managers.Audio
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioManager : MonoBehaviour
    {
        [FMODUnity.EventRef]
        public string music = "event:/MusicEvent";

        public FMOD.Studio.EventInstance instance;
        public FMOD.Studio.EventInstance fmodEvent;

        public static AudioManager Instance;

        //public FMODUnity.EventReference _music = "event:/MusicEvent";

        [SerializeProperty("Parameter")]
        public Parameters _parameter = Parameters.Default;
        public Parameters Parameter
        {
            get => _parameter;
            set
            {
                UnityEngine.Debug.Log(value);

                ChangeParameter(value);
            }
        }


        private AudioSource audioSource;

        //[Range(0, 1)]
        //public float mainVolume = 1;

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(this);
            }
            Instance = this;

            //audioSource = GetComponent<AudioSource>();
            // audioSource.volume = mainVolume;
        }

        private void Start()
        {
            instance = RuntimeManager.CreateInstance(music);
            instance.start();
        }

        private IEnumerator FadeParameterCoroutine(Parameters parameter, float a, float b, float duration)
        {
            var t = 0f;
            while (t < duration)
            {
                t += Time.deltaTime;
                var value = Mathf.Lerp(a, b, t);
                instance.setParameterByName(ParameterToString(parameter), value);
                yield return null;
            }
            instance.setParameterByName(ParameterToString(parameter), b);
        }

        
        private void FadeParameter(Parameters parameter, float endValue)
        {
            float parameterStart;
            float transitionDuration = 0.5f;
            instance.getParameterByName(ParameterToString(parameter), out parameterStart);

            StartCoroutine(FadeParameterCoroutine(parameter, parameterStart, endValue, transitionDuration));
        }

        public void ChangeParameter(Parameters parameter)
        {
            UnityEngine.Debug.Log("changing parameter");

            switch (parameter)
            {
                case Parameters.Default:
                    FadeParameter(Parameters.Warp, 0f);
                    FadeParameter(Parameters.RapidTap, 0f);
                    break;
                case Parameters.Warp:
                    FadeParameter(Parameters.Warp, 1f);
                    FadeParameter(Parameters.RapidTap, 0f);
                    break;
                case Parameters.RapidTap:
                    FadeParameter(Parameters.Warp, 0f);
                    FadeParameter(Parameters.RapidTap, 1f);
                    break;
            }
            _parameter = parameter;
        }

        private string ParameterToString(Parameters parameter)
        {
            string str = "";
            switch (parameter)
            {
                case Parameters.Default:
                    str = "";
                    break;
                case Parameters.Warp:
                    str = "Warp";
                    break;
                case Parameters.RapidTap:
                    str = "Rapid tap";
                    break;
            }
            return str;
        }


        public void PlaySound()
        {
            //RuntimeManager.PlayOneShot();
        }

        /// <summary>
        /// play an audio clip
        /// </summary>
        /// <param name="clip">AudioClip</param>
        //public void PlaySound(AudioClip clip)
        //{
        //    audioSource.pitch = 1;
        //    audioSource.PlayOneShot(clip);
        //}

        /// <summary>
        /// play an audio clip with a random pitch (0.9-1.1)
        /// </summary>
        /// <param name="clip">AudioClip</param>
        //public void PlaySoundRandomPitch(AudioClip clip)
        //{
        //    audioSource.pitch = UnityEngine.Random.Range(.9f, 1.1f);
        //    audioSource.PlayOneShot(clip);
        //}

        /// <summary>
        /// play an audio clip with volume
        /// </summary>
        /// <param name="clip">AudioClip</param>
        /// <param name="volumeScale">AudioClip</param>
        //public void PlaySound(AudioClip clip, float volumeScale)
        //{
        //    audioSource.pitch = 1;
        //    audioSource.PlayOneShot(clip, volumeScale);
        //}

        /// <summary>
        /// play an audio clip with a random pitch (0.9-1.1) with volume
        /// </summary>
        /// <param name="clip">AudioClip</param>
        /// <param name="volumeScale">AudioClip</param>
        //public void PlaySoundRandomPitch(AudioClip clip, float volumeScale)
        //{
        //    audioSource.pitch = UnityEngine.Random.Range(.9f, 1.1f);
        //    audioSource.PlayOneShot(clip, volumeScale);
        //}
    }
    public enum Parameters
    {
        Default,
        Warp,
        RapidTap
    }
}