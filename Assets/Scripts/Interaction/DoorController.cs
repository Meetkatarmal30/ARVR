using System;
using System.Collections;
using UnityEngine;
using VirtualLibrary.Core;

namespace VirtualLibrary.Interaction
{
    /// <summary>
    /// Interactive library entrance / courtyard door with smooth physical hinge rotation.
    /// </summary>
    [RequireComponent(typeof(Collider))]
    public class DoorController : MonoBehaviour, IInteractable
    {
        [Header("Door Pivot & Angles")]
        [SerializeField] private Transform doorHinge;
        [SerializeField] private float openAngle = 90f;
        [SerializeField] private float rotationSpeed = 3f;

        private bool isOpen = false;
        private Quaternion closedRotation;
        private Quaternion targetRotation;
        private Coroutine swingCoroutine;

        private void Start()
        {
            if (doorHinge == null) doorHinge = transform;
            closedRotation = doorHinge.localRotation;
            targetRotation = closedRotation;
        }

        public string GetInteractionPrompt()
        {
            return isOpen ? "Press [E] to Close Library Door" : "Press [E] to Open Library Door";
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
            isOpen = !isOpen;
            targetRotation = isOpen ? closedRotation * Quaternion.Euler(0f, openAngle, 0f) : closedRotation;

            if (swingCoroutine != null) StopCoroutine(swingCoroutine);
            swingCoroutine = StartCoroutine(SwingRoutine());

            AudioManager.Instance?.PlaySoundEffect(AudioCueType.MenuOpen);
        }

        private IEnumerator SwingRoutine()
        {
            while (Quaternion.Angle(doorHinge.localRotation, targetRotation) > 0.5f)
            {
                doorHinge.localRotation = Quaternion.Slerp(doorHinge.localRotation, targetRotation, Time.deltaTime * rotationSpeed);
                yield return null;
            }
            doorHinge.localRotation = targetRotation;
        }

        public Transform GetTransform()
        {
            return transform;
        }
    }
}

