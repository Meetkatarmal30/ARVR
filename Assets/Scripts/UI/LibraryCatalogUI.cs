using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VirtualLibrary.Core;
using VirtualLibrary.Data;

namespace VirtualLibrary.UI
{
    /// <summary>
    /// Interactive search, filtering, and catalog exploration station.
    /// Allows learners to filter by subject matter (SDG 4 multidisciplinary curricula)
    /// and discover books by keyword query.
    /// </summary>
    public class LibraryCatalogUI : MonoBehaviour
    {
        [Header("Root Container")]
        [SerializeField] private GameObject rootContainer;

        [Header("Search & Filters")]
        [SerializeField] private InputField searchInputField;
        [SerializeField] private Button filterAllButton;
        [SerializeField] private Button filterProgButton;
        [SerializeField] private Button filterSciButton;
        [SerializeField] private Button filterMathButton;
        [SerializeField] private Button filterFictButton;
        [SerializeField] private Button filterGkButton;

        [Header("Results List")]
        [SerializeField] private Transform resultsContainer;
        [SerializeField] private GameObject resultItemPrefab;
        [SerializeField] private Text resultsCountText;

        [Header("Modal Controls")]
        [SerializeField] private Button closeButton;

        private BookCategory activeCategory = BookCategory.All;
        private string activeQuery = string.Empty;
        private List<GameObject> spawnedItems = new List<GameObject>();

        private void Awake()
        {
            if (searchInputField != null)
                searchInputField.onValueChanged.AddListener(OnSearchQueryChanged);

            if (filterAllButton != null) filterAllButton.onClick.AddListener(() => SetCategoryFilter(BookCategory.All));
            if (filterProgButton != null) filterProgButton.onClick.AddListener(() => SetCategoryFilter(BookCategory.Programming));
            if (filterSciButton != null) filterSciButton.onClick.AddListener(() => SetCategoryFilter(BookCategory.Science));
            if (filterMathButton != null) filterMathButton.onClick.AddListener(() => SetCategoryFilter(BookCategory.Mathematics));
            if (filterFictButton != null) filterFictButton.onClick.AddListener(() => SetCategoryFilter(BookCategory.Fiction));
            if (filterGkButton != null) filterGkButton.onClick.AddListener(() => SetCategoryFilter(BookCategory.GeneralKnowledge));

            if (closeButton != null)
                closeButton.onClick.AddListener(OnCloseClicked);
        }

        public void Show()
        {
            if (rootContainer != null) rootContainer.SetActive(true);
            if (searchInputField != null)
            {
                searchInputField.text = string.Empty;
                searchInputField.ActivateInputField();
            }
            activeQuery = string.Empty;
            activeCategory = BookCategory.All;
            RefreshSearchResults();
        }

        public void Hide()
        {
            if (rootContainer != null) rootContainer.SetActive(false);
        }

        private void OnSearchQueryChanged(string query)
        {
            activeQuery = query;
            RefreshSearchResults();
        }

        public void SetCategoryFilter(BookCategory category)
        {
            activeCategory = category;
            AudioManager.Instance?.PlaySoundEffect(AudioCueType.ButtonClick);
            RefreshSearchResults();
        }

        public void RefreshSearchResults()
        {
            if (BookDatabase.Instance == null) return;

            List<BookData> results = BookDatabase.Instance.SearchBooks(activeQuery, activeCategory);
            int totalBooks = BookDatabase.Instance.AllBooks.Count;

            if (resultsCountText != null)
            {
                resultsCountText.text = $"Showing {results.Count} of {totalBooks} books in library";
            }

            // Clear old items
            for (int i = 0; i < spawnedItems.Count; i++)
            {
                if (spawnedItems[i] != null)
                {
                    Destroy(spawnedItems[i]);
                }
            }
            spawnedItems.Clear();

            // Spawn new cards
            if (resultsContainer != null && resultItemPrefab != null)
            {
                for (int i = 0; i < results.Count; i++)
                {
                    BookData book = results[i];
                    GameObject itemObj = Instantiate(resultItemPrefab, resultsContainer);
                    spawnedItems.Add(itemObj);

                    BookSearchResultItemUI itemUI = itemObj.GetComponent<BookSearchResultItemUI>();
                    if (itemUI != null)
                    {
                        itemUI.Bind(book, OnBookSelectedFromCatalog);
                    }
                }
            }
        }

        private void OnBookSelectedFromCatalog(BookData book)
        {
            Hide();
            LibraryManager.Instance?.OpenBookDetails(book);
        }

        private void OnCloseClicked()
        {
            Hide();
            LibraryManager.Instance?.CloseCatalog();
        }
    }
}

