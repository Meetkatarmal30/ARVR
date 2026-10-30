using System;
using UnityEngine;
using VirtualLibrary.Core;

namespace VirtualLibrary.Interaction
{
    /// <summary>
    /// Attaches to interactive computer kiosks / reception terminals in the library.
    /// Opens the Search and Filter Catalog modal upon interaction.
    /// </summary>
    [RequireComponent(typeof(Collider))]
    public class CatalogTerminalInteractable : MonoBehaviour, IInteractable
    {
        [Header("Terminal Configuration")]
        [SerializeField] private string terminalName = "Central Catalog Kiosk";
        [SerializeField] private Renderer screenRenderer;
        [SerializeField] private Color glowColor = new Color(0.2f, 0.7f, 1.0f);

        private Material screenMaterial;

        private void Start()
        {
            if (screenRenderer == null)
            {
                screenRenderer = GetComponentInChildren<Renderer>();
            }
            if (screenRenderer != null)
            {
                screenMaterial = screenRenderer.material;
            }
        }

        public string GetInteractionPrompt()
        {
            return $"Press [E] or Click to access {terminalName} [Search & Catalog]";
        }

        public void OnHoverEnter()
        {
            if (screenMaterial != null)
            {
                screenMaterial.EnableKeyword("_EMISSION");
                if (screenMaterial.HasProperty("_EmissionColor"))
                {
                    screenMaterial.SetColor("_EmissionColor", glowColor * 0.8f);
                }
            }
            AudioManager.Instance?.PlaySoundEffect(AudioCueType.Hover);
        }

        public void OnHoverExit()
        {
            if (screenMaterial != null)
            {
                if (screenMaterial.HasProperty("_EmissionColor"))
                {
                    screenMaterial.SetColor("_EmissionColor", Color.black);
                }
            }
        }

        public void OnInteract()
        {
            LibraryManager.Instance?.OpenCatalog();
        }

        public Transform GetTransform()
        {
            return transform;
        }

        private void OnDestroy()
        {
            if (screenMaterial != null)
            {
                Destroy(screenMaterial);
            }
        }
    }
}

