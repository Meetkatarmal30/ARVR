using System;
using UnityEngine;
using UnityEngine.XR;
using VirtualLibrary.Interaction;

namespace VirtualLibrary.Player
{
    /// <summary>
    /// Intelligently detects whether an OpenXR / XR device is active at runtime.
    /// Seamlessly switches between the Desktop First-Person Controller and the VR XR Origin rig.
    /// </summary>
    public class VRRigAdapter : MonoBehaviour
    {
        [Header("Rig Hierarchies")]
        [SerializeField] private GameObject desktopRig;
        [SerializeField] private GameObject xrOriginRig;

        [Header("VR Controllers")]
        [SerializeField] private Transform rightHandController;
        [SerializeField] private Transform leftHandController;
        [SerializeField] private RaycastInteractor raycastInteractor;

        [Header("Runtime Detection Status")]
        [SerializeField] private bool isVRActive;
        [SerializeField] private string detectedXRDevice;

        public bool IsVRActive => isVRActive;

        private void Awake()
        {
            DetectAndConfigureXR();
        }

        private void DetectAndConfigureXR()
        {
            isVRActive = CheckXRDevicePresent();

            if (isVRActive)
            {
                Debug.Log($"[VRRigAdapter] Active XR Device Detected: {detectedXRDevice}. Enabling VR Rig.");
                if (desktopRig != null) desktopRig.SetActive(false);
                if (xrOriginRig != null) xrOriginRig.SetActive(true);

                if (raycastInteractor != null && rightHandController != null)
                {
                    raycastInteractor.SetVRControllerTransform(rightHandController);
                }
            }
            else
            {
                Debug.Log("[VRRigAdapter] No active XR Headset detected. Enabling Desktop Controller mode.");
                if (desktopRig != null) desktopRig.SetActive(true);
                if (xrOriginRig != null) xrOriginRig.SetActive(false);
            }
        }

        private bool CheckXRDevicePresent()
        {
            // Check legacy XRSettings
            if (XRSettings.isDeviceActive && !string.IsNullOrEmpty(XRSettings.loadedDeviceName))
            {
                detectedXRDevice = XRSettings.loadedDeviceName;
                return true;
            }

            // Check InputDevices for HeadMountedDisplay
            var inputDevices = new System.Collections.Generic.List<InputDevice>();
            InputDevices.GetDevicesWithCharacteristics(InputDeviceCharacteristics.HeadMounted, inputDevices);
            if (inputDevices.Count > 0)
            {
                detectedXRDevice = inputDevices[0].name;
                return true;
            }

            detectedXRDevice = "None (Desktop Mode)";
            return false;
        }

        public void ForceDesktopMode()
        {
            isVRActive = false;
            if (desktopRig != null) desktopRig.SetActive(true);
            if (xrOriginRig != null) xrOriginRig.SetActive(false);
        }

        public void ForceVRMode()
        {
            isVRActive = true;
            if (desktopRig != null) desktopRig.SetActive(false);
            if (xrOriginRig != null) xrOriginRig.SetActive(true);
        }
    }
}

