using System;
using UnityEngine;
using VirtualLibrary.Core;
using VirtualLibrary.Data;

namespace VirtualLibrary.UI
{
    /// <summary>
    /// Master UI Orchestrator managing view stack transitions,
    /// modal visibility, and events dispatched from the LibraryManager.
    /// </summary>
    public class UIController : MonoBehaviour
    {
        [Header("Managed Views")]
        [SerializeField] private WelcomeScreenUI welcomeScreen;
        [SerializeField] private BookDetailPanelUI bookDetailPanel;
        [SerializeField] private LibraryCatalogUI catalogPanel;
        [SerializeField] private InteractionHUD hud;

        private void Start()
        {
            if (LibraryManager.Instance != null)
            {
                LibraryManager.Instance.OnStateChanged += HandleStateChanged;
                LibraryManager.Instance.OnBookOpened += HandleBookOpened;
                LibraryManager.Instance.OnBookClosed += HandleBookClosed;
                LibraryManager.Instance.OnCatalogOpened += HandleCatalogOpened;
                LibraryManager.Instance.OnCatalogClosed += HandleCatalogClosed;

                // Set initial UI according to starting state
                HandleStateChanged(LibraryManager.Instance.CurrentState);
            }
        }

        private void OnDestroy()
        {
            if (LibraryManager.Instance != null)
            {
                LibraryManager.Instance.OnStateChanged -= HandleStateChanged;
                LibraryManager.Instance.OnBookOpened -= HandleBookOpened;
                LibraryManager.Instance.OnBookClosed -= HandleBookClosed;
                LibraryManager.Instance.OnCatalogOpened -= HandleCatalogOpened;
                LibraryManager.Instance.OnCatalogClosed -= HandleCatalogClosed;
            }
        }

        private void HandleStateChanged(InteractionState state)
        {
            switch (state)
            {
                case InteractionState.WelcomeScreen:
                    if (welcomeScreen != null) welcomeScreen.Show();
                    if (bookDetailPanel != null) bookDetailPanel.Hide();
                    if (catalogPanel != null) catalogPanel.Hide();
                    if (hud != null) hud.gameObject.SetActive(false);
                    break;

                case InteractionState.Exploring:
                    if (welcomeScreen != null) welcomeScreen.Hide();
                    if (bookDetailPanel != null) bookDetailPanel.Hide();
                    if (catalogPanel != null) catalogPanel.Hide();
                    if (hud != null) hud.gameObject.SetActive(true);
                    break;

                case InteractionState.ReadingBook:
                    if (welcomeScreen != null) welcomeScreen.Hide();
                    if (catalogPanel != null) catalogPanel.Hide();
                    if (hud != null) hud.gameObject.SetActive(false);
                    break;

                case InteractionState.SearchingCatalog:
                    if (welcomeScreen != null) welcomeScreen.Hide();
                    if (bookDetailPanel != null) bookDetailPanel.Hide();
                    if (hud != null) hud.gameObject.SetActive(false);
                    break;
            }
        }

        private void HandleBookOpened(BookData book)
        {
            if (bookDetailPanel != null)
            {
                bookDetailPanel.DisplayBook(book);
            }
        }

        private void HandleBookClosed()
        {
            if (bookDetailPanel != null)
            {
                bookDetailPanel.Hide();
            }
        }

        private void HandleCatalogOpened()
        {
            if (catalogPanel != null)
            {
                catalogPanel.Show();
            }
        }

        private void HandleCatalogClosed()
        {
            if (catalogPanel != null)
            {
                catalogPanel.Hide();
            }
        }
    }
}

