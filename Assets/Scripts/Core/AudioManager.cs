using System;
using UnityEngine;

namespace VirtualLibrary.Core
{
    public enum AudioCueType
    {
        ButtonClick,
        BookOpen,
        BookClose,
        MenuOpen,
        MenuClose,
        WaypointPing,
        StartExplore,
        Hover
    }

    /// <summary>
    /// Robust audio manager providing procedural acoustic synthesis for UI clicks,
    /// page turns, and ambient atmospheric tones when external sound files are absent.
    /// </summary>
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance { get; private set; }

        [Header("Audio Sources")]
        [SerializeField] private AudioSource sfxSource;
        [SerializeField] private AudioSource ambienceSource;

        [Header("Volume Controls")]
        [Range(0f, 1f)] public float masterVolume = 1f;
        [Range(0f, 1f)] public float sfxVolume = 0.8f;
        [Range(0f, 1f)] public float ambienceVolume = 0.35f;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;

            EnsureAudioSources();
            InitializeProceduralAmbience();
        }

        private void EnsureAudioSources()
        {
            if (sfxSource == null)
            {
                GameObject sfxObj = new GameObject("SFX_AudioSource");
                sfxObj.transform.SetParent(transform);
                sfxSource = sfxObj.AddComponent<AudioSource>();
                sfxSource.playOnAwake = false;
            }

            if (ambienceSource == null)
            {
                GameObject ambObj = new GameObject("Ambience_AudioSource");
                ambObj.transform.SetParent(transform);
                ambienceSource = ambObj.AddComponent<AudioSource>();
                ambienceSource.loop = true;
                ambienceSource.playOnAwake = false;
            }
        }

        private void InitializeProceduralAmbience()
        {
            if (ambienceSource != null && ambienceSource.clip == null)
            {
                // Synthesize 4 seconds of warm, soothing library room tone (pink/brown noise + low harmonics)
                ambienceSource.clip = CreateProceduralAmbienceClip(4.0f);
                ambienceSource.volume = ambienceVolume * masterVolume;
                ambienceSource.Play();
            }
        }

        public void PlaySoundEffect(AudioCueType cueType)
        {
            if (sfxSource == null) return;

            AudioClip clip = GenerateProceduralCueClip(cueType);
            if (clip != null)
            {
                sfxSource.PlayOneShot(clip, sfxVolume * masterVolume);
            }
        }

        /// <summary>
        /// Generates procedural audio clips on-the-fly using mathematical wave synthesis.
        /// Guaranteed to work without needing any external audio assets!
        /// </summary>
        private AudioClip GenerateProceduralCueClip(AudioCueType cueType)
        {
            int sampleRate = 44100;
            float duration;
            float[] samples;

            switch (cueType)
            {
                case AudioCueType.ButtonClick:
                    duration = 0.06f;
                    samples = new float[(int)(sampleRate * duration)];
                    for (int i = 0; i < samples.Length; i++)
                    {
                        float t = (float)i / sampleRate;
                        float freq = 800f - (t / duration) * 300f;
                        float envelope = 1f - (t / duration);
                        samples[i] = Mathf.Sin(2 * Mathf.PI * freq * t) * envelope * 0.3f;
                    }
                    break;

                case AudioCueType.BookOpen:
                    duration = 0.22f; // Soft rustle / page turn
                    samples = new float[(int)(sampleRate * duration)];
                    System.Random rand = new System.Random(42);
                    for (int i = 0; i < samples.Length; i++)
                    {
                        float t = (float)i / duration;
                        float envelope = Mathf.Sin(t * Mathf.PI);
                        float noise = (float)(rand.NextDouble() * 2.0 - 1.0);
                        samples[i] = noise * envelope * 0.25f;
                    }
                    break;

                case AudioCueType.BookClose:
                    duration = 0.18f; // Gentle book thump
                    samples = new float[(int)(sampleRate * duration)];
                    for (int i = 0; i < samples.Length; i++)
                    {
                        float t = (float)i / sampleRate;
                        float freq = 120f - (t / duration) * 60f;
                        float envelope = Mathf.Exp(-t * 18f);
                        samples[i] = Mathf.Sin(2 * Mathf.PI * freq * t) * envelope * 0.4f;
                    }
                    break;

                case AudioCueType.WaypointPing:
                    duration = 0.35f; // Chime
                    samples = new float[(int)(sampleRate * duration)];
                    for (int i = 0; i < samples.Length; i++)
                    {
                        float t = (float)i / sampleRate;
                        float freq = 1046.5f; // C6 note
                        float envelope = Mathf.Exp(-t * 9f);
                        samples[i] = (Mathf.Sin(2 * Mathf.PI * freq * t) + 0.5f * Mathf.Sin(2 * Mathf.PI * freq * 2f * t)) * envelope * 0.28f;
                    }
                    break;

                case AudioCueType.StartExplore:
                    duration = 0.4f; // Ascending pleasant major chord chime
                    samples = new float[(int)(sampleRate * duration)];
                    for (int i = 0; i < samples.Length; i++)
                    {
                        float t = (float)i / sampleRate;
                        float f1 = 523.25f; // C5
                        float f2 = 659.25f; // E5
                        float f3 = 783.99f; // G5
                        float envelope = Mathf.Exp(-t * 5f);
                        samples[i] = (Mathf.Sin(2 * Mathf.PI * f1 * t) + Mathf.Sin(2 * Mathf.PI * f2 * t) + Mathf.Sin(2 * Mathf.PI * f3 * t)) / 3f * envelope * 0.3f;
                    }
                    break;

                default:
                    duration = 0.05f;
                    samples = new float[(int)(sampleRate * duration)];
                    for (int i = 0; i < samples.Length; i++)
                    {
                        float t = (float)i / sampleRate;
                        samples[i] = Mathf.Sin(2 * Mathf.PI * 600f * t) * (1f - (float)i / samples.Length) * 0.2f;
                    }
                    break;
            }

            AudioClip clip = AudioClip.Create("SFX_" + cueType, samples.Length, 1, sampleRate, false);
            clip.SetData(samples, 0);
            return clip;
        }

        private AudioClip CreateProceduralAmbienceClip(float lengthSeconds)
        {
            int sampleRate = 44100;
            int totalSamples = (int)(sampleRate * lengthSeconds);
            float[] samples = new float[totalSamples];

            // Pink noise filtering with gentle low rumble (library ventilation / room acoustic)
            float b0 = 0, b1 = 0, b2 = 0;
            System.Random rand = new System.Random(1337);

            for (int i = 0; i < totalSamples; i++)
            {
                float white = (float)(rand.NextDouble() * 2.0 - 1.0);
                b0 = 0.99765f * b0 + white * 0.0990460f;
                b1 = 0.96300f * b1 + white * 0.0664790f;
                b2 = 0.57000f * b2 + white * 0.5391300f;
                float pink = (b0 + b1 + b2 + white * 0.5362f) * 0.04f;

                // Add very subtle 60Hz hum and 120Hz warmth harmonic
                float t = (float)i / sampleRate;
                float hum = Mathf.Sin(2 * Mathf.PI * 60f * t) * 0.015f + Mathf.Sin(2 * Mathf.PI * 120f * t) * 0.008f;

                samples[i] = Mathf.Clamp(pink + hum, -1f, 1f);
            }

            AudioClip clip = AudioClip.Create("Procedural_Library_Ambience", totalSamples, 1, sampleRate, false);
            clip.SetData(samples, 0);
            return clip;
        }
    }
}

