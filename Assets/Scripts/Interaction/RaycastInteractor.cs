using System;
using UnityEngine;
using VirtualLibrary.Core;
using VirtualLibrary.UI;

namespace VirtualLibrary.Interaction
{
    /// <summary>
    /// Performs center-screen viewport raycasting (desktop) or controller raycasting (VR)
    /// to detect, highlight, and activate interactable library objects.
    /// </summary>
    public class RaycastInteractor : MonoBehaviour
    {
        [Header("Raycast Setup")]
        [SerializeField] private Camera playerCamera;
        [SerializeField] private Transform vrControllerTransform; // Optional for VR
        [SerializeField] private float maxInteractionDistance = 3.5f;
        [SerializeField] private LayerMask interactionLayers = ~0; // Default all layers

        [Header("UI Reference")]
        [SerializeField] private InteractionHUD hud;

        private IInteractable currentHoveredInteractable;

        private void Start()
        {
            if (playerCamera == null)
            {
                playerCamera = Camera.main;
            }
            if (hud == null)
            {
                hud = FindObjectOfType<InteractionHUD>();
            }
        }

        private void Update()
        {
            // Only perform raycast interaction when actively exploring
            if (LibraryManager.Instance != null && LibraryManager.Instance.CurrentState != InteractionState.Exploring)
            {
                ClearCurrentHover();
                return;
            }

            PerformRaycast();
            CheckInteractionInput();
        }

        private void PerformRaycast()
        {
            Ray ray;
            if (vrControllerTransform != null && vrControllerTransform.gameObject.activeInHierarchy)
            {
                ray = new Ray(vrControllerTransform.position, vrControllerTransform.forward);
            }
            else if (playerCamera != null)
            {
                ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
            }
            else
            {
                return;
            }

            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, maxInteractionDistance, interactionLayers))
            {
                IInteractable interactable = hit.collider.GetComponentInParent<IInteractable>();
                if (interactable != null)
                {
                    if (currentHoveredInteractable != interactable)
                    {
                        ClearCurrentHover();
                        currentHoveredInteractable = interactable;
                        currentHoveredInteractable.OnHoverEnter();
                    }

                    if (hud != null)
                    {
                        hud.ShowPrompt(currentHoveredInteractable.GetInteractionPrompt());
                        hud.SetReticleHighlighted(true);
                    }
                    return;
                }
            }

            ClearCurrentHover();
        }

        private void ClearCurrentHover()
        {
            if (currentHoveredInteractable != null)
            {
                currentHoveredInteractable.OnHoverExit();
                currentHoveredInteractable = null;
            }

            if (hud != null)
            {
                hud.HidePrompt();
                hud.SetReticleHighlighted(false);
            }
        }

        private void CheckInteractionInput()
        {
            if (currentHoveredInteractable == null) return;

            // Trigger interaction via [E], Space, Left Mouse Button, or VR primary trigger
            bool interactTriggered = Input.GetKeyDown(KeyCode.E) ||
                                     Input.GetMouseButtonDown(0);

            if (interactTriggered)
            {
                currentHoveredInteractable.OnInteract();
            }
        }

        public void SetVRControllerTransform(Transform controller)
        {
            vrControllerTransform = controller;
        }
    }
}

