using System;
using UnityEngine;
using VirtualLibrary.Core;
using VirtualLibrary.Data;

namespace VirtualLibrary.Interaction
{
    /// <summary>
    /// Attaches to 3D book gameObjects on shelves or desks.
    /// Provides dynamic hover highlighting, tooltip prompt, and opens BookDetailPanel upon interaction.
    /// </summary>
    [RequireComponent(typeof(Collider))]
    public class BookInteractable : MonoBehaviour, IInteractable
    {
        [Header("Book Configuration")]
        [SerializeField] private string bookId = "PROG-01";
        [SerializeField] private BookData customBookData;

        [Header("Visual Feedback")]
        [SerializeField] private Renderer bookRenderer;
        [SerializeField] private Color highlightEmissionColor = new Color(0.25f, 0.25f, 0.45f);
        [SerializeField] private float hoverScaleFactor = 1.05f;

        private BookData resolvedBookData;
        private Material originalMaterial;
        private Material instanceMaterial;
        private Vector3 initialScale;
        private bool isHovered;

        private void Start()
        {
            initialScale = transform.localScale;

            if (bookRenderer == null)
            {
                bookRenderer = GetComponentInChildren<Renderer>();
            }

            if (bookRenderer != null)
            {
                instanceMaterial = bookRenderer.material; // Creates local instance
            }

            ResolveBookData();
            ApplyCoverColor();
        }

        private void ResolveBookData()
        {
            if (customBookData != null && !string.IsNullOrEmpty(customBookData.id))
            {
                resolvedBookData = customBookData;
            }
            else if (BookDatabase.Instance != null)
            {
                resolvedBookData = BookDatabase.Instance.GetBookById(bookId);
            }
        }

        private void ApplyCoverColor()
        {
            if (resolvedBookData != null && instanceMaterial != null)
            {
                // Tint base color with book cover color
                if (instanceMaterial.HasProperty("_Color"))
                {
                    instanceMaterial.color = resolvedBookData.coverColor;
                }
                else if (instanceMaterial.HasProperty("_BaseColor"))
                {
                    instanceMaterial.SetColor("_BaseColor", resolvedBookData.coverColor);
                }
            }
        }

        public string GetInteractionPrompt()
        {
            ResolveBookData();
            string title = resolvedBookData != null ? resolvedBookData.title : "Educational Book";
            return $"Press [E] or Click to inspect \"{title}\"";
        }

        public void OnHoverEnter()
        {
            isHovered = true;
            transform.localScale = initialScale * hoverScaleFactor;

            if (instanceMaterial != null)
            {
                instanceMaterial.EnableKeyword("_EMISSION");
                if (instanceMaterial.HasProperty("_EmissionColor"))
                {
                    instanceMaterial.SetColor("_EmissionColor", highlightEmissionColor);
                }
            }

            AudioManager.Instance?.PlaySoundEffect(AudioCueType.Hover);
        }

        public void OnHoverExit()
        {
            isHovered = false;
            transform.localScale = initialScale;

            if (instanceMaterial != null)
            {
                if (instanceMaterial.HasProperty("_EmissionColor"))
                {
                    instanceMaterial.SetColor("_EmissionColor", Color.black);
                }
            }
        }

        public void OnInteract()
        {
            ResolveBookData();
            if (resolvedBookData != null && LibraryManager.Instance != null)
            {
                LibraryManager.Instance.OpenBookDetails(resolvedBookData);
            }
        }

        public Transform GetTransform()
        {
            return transform;
        }

        public void SetBookId(string newId)
        {
            bookId = newId;
            ResolveBookData();
            ApplyCoverColor();
        }

        private void OnDestroy()
        {
            if (instanceMaterial != null)
            {
                Destroy(instanceMaterial);
            }
        }
    }
}

