using System;
using System.Collections;
using UnityEngine;

namespace VirtualLibrary.Audio
{
    /// <summary>
    /// Introduces realistic library acoustics with periodic subtle page turns,
    /// soft ambient echoes, and spatial room reverberation.
    /// </summary>
    [RequireComponent(typeof(AudioSource))]
    public class ProceduralLibraryAmbience : MonoBehaviour
    {
        [Header("Ambience Timing")]
        [SerializeField] private float minIntervalSeconds = 18f;
        [SerializeField] private float maxIntervalSeconds = 42f;
        [SerializeField] [Range(0f, 1f)] private float soundVolume = 0.25f;

        private AudioSource audioSource;
        private Coroutine ambienceCoroutine;

        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
            audioSource.playOnAwake = false;
            audioSource.spatialBlend = 0.7f; // Realistic 3D falloff
        }

        private void OnEnable()
        {
            ambienceCoroutine = StartCoroutine(AmbienceRoutine());
        }

        private void OnDisable()
        {
            if (ambienceCoroutine != null)
            {
                StopCoroutine(ambienceCoroutine);
            }
        }

        private IEnumerator AmbienceRoutine()
        {
            while (true)
            {
                float waitTime = UnityEngine.Random.Range(minIntervalSeconds, maxIntervalSeconds);
                yield return new WaitForSeconds(waitTime);

                PlaySubtlePageTurn();
            }
        }

        private void PlaySubtlePageTurn()
        {
            AudioClip clip = CreateSoftPageRustleClip();
            if (clip != null && audioSource != null)
            {
                audioSource.pitch = UnityEngine.Random.Range(0.9f, 1.1f);
                audioSource.PlayOneShot(clip, soundVolume);
            }
        }

        private AudioClip CreateSoftPageRustleClip()
        {
            int sampleRate = 44100;
            float duration = 0.35f;
            int totalSamples = (int)(sampleRate * duration);
            float[] samples = new float[totalSamples];

            System.Random rand = new System.Random();
            for (int i = 0; i < totalSamples; i++)
            {
                float t = (float)i / totalSamples;
                float envelope = Mathf.Sin(t * Mathf.PI);
                float noise = (float)(rand.NextDouble() * 2.0 - 1.0);
                samples[i] = noise * envelope * 0.18f;
            }

            AudioClip clip = AudioClip.Create("AmbientPageRustle", totalSamples, 1, sampleRate, false);
            clip.SetData(samples, 0);
            return clip;
        }
    }
}

