using System;
using UnityEngine;
using UnityEngine.UI;
using VirtualLibrary.Core;
using VirtualLibrary.Data;

namespace VirtualLibrary.UI
{
    /// <summary>
    /// Interactive modal displaying comprehensive educational book details:
    /// cover art placeholder, metadata, key learning outcomes, SDG impact,
    /// and a "Locate on Shelf" wayfinding navigation button.
    /// </summary>
    public class BookDetailPanelUI : MonoBehaviour
    {
        [Header("Root Container")]
        [SerializeField] private GameObject rootContainer;

        [Header("Cover Artwork Simulation")]
        [SerializeField] private Image coverImage;
        [SerializeField] private Text coverTitleText;
        [SerializeField] private Text coverAuthorText;

        [Header("Book Metadata Labels")]
        [SerializeField] private Text titleText;
        [SerializeField] private Text authorAndYearText;
        [SerializeField] private Text categoryBadgeText;
        [SerializeField] private Text difficultyBadgeText;
        [SerializeField] private Text isbnAndLocationText;
        [SerializeField] private Text sdgBadgeText;

        [Header("Content Body")]
        [SerializeField] private Text overviewText;
        [SerializeField] private Text takeawaysText;

        [Header("Interactive Buttons")]
        [SerializeField] private Button closeButton;
        [SerializeField] private Button locateOnShelfButton;

        private BookData currentBook;

        private void Awake()
        {
            if (closeButton != null)
                closeButton.onClick.AddListener(OnCloseClicked);

            if (locateOnShelfButton != null)
                locateOnShelfButton.onClick.AddListener(OnLocateOnShelfClicked);
        }

        public void DisplayBook(BookData book)
        {
            if (book == null) return;
            currentBook = book;

            if (rootContainer != null) rootContainer.SetActive(true);

            // Cover Art Placeholder
            if (coverImage != null) coverImage.color = book.coverColor;
            if (coverTitleText != null) coverTitleText.text = book.title;
            if (coverAuthorText != null) coverAuthorText.text = book.author;

            // Details
            if (titleText != null) titleText.text = book.title;
            if (authorAndYearText != null) authorAndYearText.text = $"By {book.author} ({book.publicationYear})";
            if (categoryBadgeText != null) categoryBadgeText.text = $"Category: {book.GetFormattedCategory()}";
            if (difficultyBadgeText != null) difficultyBadgeText.text = $"Level: {book.GetDifficultyBadge()}";
            if (isbnAndLocationText != null) isbnAndLocationText.text = $"ISBN: {book.isbn}  |  Location: {book.shelfLocationName}";
            if (sdgBadgeText != null) sdgBadgeText.text = $"Educational Impact: {book.targetSDG}";

            if (overviewText != null) overviewText.text = book.fullOverview;
            if (takeawaysText != null) takeawaysText.text = book.keyTakeaways;
        }

        public void Hide()
        {
            if (rootContainer != null) rootContainer.SetActive(false);
            currentBook = null;
        }

        private void OnCloseClicked()
        {
            Hide();
            LibraryManager.Instance?.CloseBookDetails();
        }

        private void OnLocateOnShelfClicked()
        {
            if (currentBook != null && LibraryManager.Instance != null)
            {
                LibraryManager.Instance.SetShelfWaypoint(currentBook.shelfWorldPosition, $"{currentBook.title} ({currentBook.shelfLocationName})");
                Hide();
                LibraryManager.Instance.CloseBookDetails();
            }
        }
    }
}

