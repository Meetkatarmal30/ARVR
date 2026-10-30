using System;
using UnityEngine;

namespace VirtualLibrary.Player
{
    /// <summary>
    /// Implements natural sinusoidal head-bobbing motion synchronized with player walking speed.
    /// Provides subtle immersion while maintaining comfort.
    /// </summary>
    public class HeadBob : MonoBehaviour
    {
        [Header("Bob Parameters")]
        [SerializeField] private float bobbingSpeed = 10f;
        [SerializeField] private float bobbingAmount = 0.04f;
        [SerializeField] private FirstPersonController controller;

        private float defaultPosY;
        private float timer = 0f;

        private void Start()
        {
            defaultPosY = transform.localPosition.y;
            if (controller == null)
            {
                controller = GetComponentInParent<FirstPersonController>();
            }
        }

        private void Update()
        {
            if (controller != null && controller.IsMoving)
            {
                timer += Time.deltaTime * bobbingSpeed;
                float newY = defaultPosY + Mathf.Sin(timer) * bobbingAmount;
                transform.localPosition = new Vector3(transform.localPosition.x, newY, transform.localPosition.z);
            }
            else
            {
                timer = 0f;
                Vector3 targetPos = new Vector3(transform.localPosition.x, defaultPosY, transform.localPosition.z);
                transform.localPosition = Vector3.Lerp(transform.localPosition, targetPos, Time.deltaTime * 6f);
            }
        }
    }
}

