using System;
using UnityEngine;
using UnityEngine.UI;
using VirtualLibrary.Core;

namespace VirtualLibrary.UI
{
    /// <summary>
    /// Displays the opening welcome portal, CIAP project title,
    /// SDG 4 and SDG 10 educational missions, controls guide, and entry point.
    /// </summary>
    public class WelcomeScreenUI : MonoBehaviour
    {
        [Header("UI Panels")]
        [SerializeField] private GameObject rootContainer;
        [SerializeField] private GameObject controlsSubPanel;

        [Header("Buttons")]
        [SerializeField] private Button startExploringButton;
        [SerializeField] private Button openCatalogButton;
        [SerializeField] private Button toggleControlsButton;
        [SerializeField] private Button exitButton;

        [Header("SDG & Project Labels")]
        [SerializeField] private Text titleText;
        [SerializeField] private Text subtitleText;
        [SerializeField] private Text sdgBadgeText;
        [SerializeField] private Text controlsGuideText;

        private void Awake()
        {
            if (startExploringButton != null)
                startExploringButton.onClick.AddListener(OnStartExploringClicked);

            if (openCatalogButton != null)
                openCatalogButton.onClick.AddListener(OnOpenCatalogClicked);

            if (toggleControlsButton != null)
                toggleControlsButton.onClick.AddListener(OnToggleControlsClicked);

            if (exitButton != null)
                exitButton.onClick.AddListener(OnExitClicked);

            InitializeContent();
        }

        private void InitializeContent()
        {
            if (titleText != null)
                titleText.text = "VR Virtual Library for Interactive Learning";

            if (subtitleText != null)
                subtitleText.text = "CIAP College Project — Interactive Autonomous 3D Educational Space";

            if (sdgBadgeText != null)
                sdgBadgeText.text = "Primary SDG: SDG 4 (Quality Education)\nSecondary SDG: SDG 10 (Reduced Inequalities)";

            if (controlsGuideText != null)
            {
                controlsGuideText.text =
                    "<b>DESKTOP CONTROLS:</b>\n" +
                    "• WASD / Arrow Keys — Walk & Strafe\n" +
                    "• Mouse — Look Around (Pitch & Yaw)\n" +
                    "• Left Shift — Sprint / Move Faster\n" +
                    "• [E] or Left Click — Read Book / Use Terminal\n" +
                    "• [Tab] or [M] — Search & Filter Catalog\n" +
                    "• [Escape] — Close Dialogs / Menu\n\n" +
                    "<b>VR CONTROLS (OpenXR):</b>\n" +
                    "• Left Thumbstick — Smooth Locomotion\n" +
                    "• Right Thumbstick — Snap Turn (45°)\n" +
                    "• Right Hand Ray + Trigger — Select & Read Book";
            }
        }

        public void Show()
        {
            if (rootContainer != null) rootContainer.SetActive(true);
            if (controlsSubPanel != null) controlsSubPanel.SetActive(false);
        }

        public void Hide()
        {
            if (rootContainer != null) rootContainer.SetActive(false);
        }

        private void OnStartExploringClicked()
        {
            Hide();
            LibraryManager.Instance?.StartExploring();
        }

        private void OnOpenCatalogClicked()
        {
            Hide();
            LibraryManager.Instance?.OpenCatalog();
        }

        private void OnToggleControlsClicked()
        {
            if (controlsSubPanel != null)
            {
                controlsSubPanel.SetActive(!controlsSubPanel.activeSelf);
            }
        }

        private void OnExitClicked()
        {
            LibraryManager.Instance?.QuitApplication();
        }
    }
}

