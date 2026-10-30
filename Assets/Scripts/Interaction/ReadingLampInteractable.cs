using System;
using UnityEngine;
using VirtualLibrary.Core;

namespace VirtualLibrary.Interaction
{
    /// <summary>
    /// Interactive desk lamp allowing learners to toggle localized reading lighting.
    /// Provides auditory and visual feedback.
    /// </summary>
    [RequireComponent(typeof(Collider))]
    public class ReadingLampInteractable : MonoBehaviour, IInteractable
    {
        [Header("Lamp Components")]
        [SerializeField] private Light deskLight;
        [SerializeField] private Renderer bulbRenderer;
        [SerializeField] private Color emissionColor = new Color(1f, 0.9f, 0.65f);

        private bool isOn = true;
        private Material bulbMaterial;

        private void Start()
        {
            if (bulbRenderer != null)
            {
                bulbMaterial = bulbRenderer.material;
            }
            UpdateVisuals();
        }

        public string GetInteractionPrompt()
        {
            return isOn ? "Press [E] to Turn Off Study Lamp" : "Press [E] to Turn On Study Lamp";
        }

        public void OnHoverEnter()
        {
            AudioManager.Instance?.PlaySoundEffect(AudioCueType.Hover);
        }

        public void OnHoverExit()
        {
        }

        public void OnInteract()
        {
            isOn = !isOn;
            UpdateVisuals();
            AudioManager.Instance?.PlaySoundEffect(AudioCueType.ButtonClick);
        }

        public Transform GetTransform()
        {
            return transform;
        }

        private void UpdateVisuals()
        {
            if (deskLight != null)
            {
                deskLight.enabled = isOn;
            }

            if (bulbMaterial != null)
            {
                if (isOn)
                {
                    bulbMaterial.EnableKeyword("_EMISSION");
                    if (bulbMaterial.HasProperty("_EmissionColor"))
                    {
                        bulbMaterial.SetColor("_EmissionColor", emissionColor * 1.5f);
                    }
                }
                else
                {
                    if (bulbMaterial.HasProperty("_EmissionColor"))
                    {
                        bulbMaterial.SetColor("_EmissionColor", Color.black);
                    }
                }
            }
        }

        private void OnDestroy()
        {
            if (bulbMaterial != null)
            {
                Destroy(bulbMaterial);
            }
        }
    }
}

