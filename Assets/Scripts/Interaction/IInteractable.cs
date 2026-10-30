using System;
using UnityEngine;

namespace VirtualLibrary.Interaction
{
    /// <summary>
    /// Common contract for any physical 3D element in the Virtual Library
    /// that can be hovered and interacted with by the player (Desktop or VR).
    /// </summary>
    public interface IInteractable
    {
        string GetInteractionPrompt();
        void OnHoverEnter();
        void OnHoverExit();
        void OnInteract();
        Transform GetTransform();
    }
}

