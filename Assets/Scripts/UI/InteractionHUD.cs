using System;
using UnityEngine;
using UnityEngine.UI;
using VirtualLibrary.Core;

namespace VirtualLibrary.UI
{
    /// <summary>
    /// Displays central reticle, dynamic interaction prompt tooltips,
    /// shelf waypoint guidance distance, and persistent helper controls.
    /// </summary>
    public class InteractionHUD : MonoBehaviour
    {
        [Header("Crosshair Reticle")]
        [SerializeField] private Image reticleImage;
        [SerializeField] private Color normalReticleColor = new Color(1f, 1f, 1f, 0.6f);
        [SerializeField] private Color highlightReticleColor = new Color(0.2f, 0.9f, 0.4f, 1f);
        [SerializeField] private Vector3 normalReticleScale = Vector3.one;
        [SerializeField] private Vector3 highlightReticleScale = new Vector3(1.35f, 1.35f, 1f);

        [Header("Prompt Box")]
        [SerializeField] private GameObject promptContainer;
        [SerializeField] private Text promptText;

        [Header("Waypoint Guidance")]
        [SerializeField] private GameObject waypointContainer;
        [SerializeField] private Text waypointText;
        [SerializeField] private Transform playerTransform;

        [Header("Controls Tip")]
        [SerializeField] private Text controlsTipText;

        private void Start()
        {
            HidePrompt();
            SetReticleHighlighted(false);

            if (controlsTipText != null)
            {
                controlsTipText.text = "[WASD] Move | [E] Read Book | [Tab] Search Catalog | [Esc] Menu";
            }

            if (playerTransform == null && Camera.main != null)
            {
                playerTransform = Camera.main.transform;
            }
        }

        private void Update()
        {
            UpdateWaypointIndicator();
        }

        private void UpdateWaypointIndicator()
        {
            if (LibraryManager.Instance == null || !LibraryManager.Instance.HasActiveWaypoint)
            {
                if (waypointContainer != null && waypointContainer.activeSelf)
                {
                    waypointContainer.SetActive(false);
                }
                return;
            }

            if (waypointContainer != null && !waypointContainer.activeSelf)
            {
                waypointContainer.SetActive(true);
            }

            if (waypointText != null && playerTransform != null)
            {
                Vector3 target = LibraryManager.Instance.ActiveWaypointPosition;
                float distance = Vector3.Distance(playerTransform.position, target);

                waypointText.text = $"★ Destination: {LibraryManager.Instance.ActiveWaypointLabel} [{distance:F1}m away]";

                // Automatically clear waypoint if player arrived within 1.8 meters
                if (distance < 1.8f)
                {
                    LibraryManager.Instance.ClearShelfWaypoint();
                }
            }
        }

        public void ShowPrompt(string message)
        {
            if (promptContainer != null) promptContainer.SetActive(true);
            if (promptText != null) promptText.text = message;
        }

        public void HidePrompt()
        {
            if (promptContainer != null) promptContainer.SetActive(false);
            if (promptText != null) promptText.text = string.Empty;
        }

        public void SetReticleHighlighted(bool isHighlighted)
        {
            if (reticleImage != null)
            {
                reticleImage.color = isHighlighted ? highlightReticleColor : normalReticleColor;
                reticleImage.transform.localScale = isHighlighted ? highlightReticleScale : normalReticleScale;
            }
        }
    }
}

