using System;
using UnityEngine;
using UnityEngine.UI;
using VirtualLibrary.Core;
using VirtualLibrary.Data;

namespace VirtualLibrary.UI
{
    /// <summary>
    /// Represents an individual book entry card within the search and catalog result list.
    /// </summary>
    public class BookSearchResultItemUI : MonoBehaviour
    {
        [Header("UI Fields")]
        [SerializeField] private Image categoryColorStrip;
        [SerializeField] private Text titleText;
        [SerializeField] private Text authorText;
        [SerializeField] private Text categoryPillText;
        [SerializeField] private Text locationText;
        [SerializeField] private Button inspectButton;

        private BookData boundBook;
        private Action<BookData> onSelectCallback;

        public void Bind(BookData book, Action<BookData> onSelect)
        {
            boundBook = book;
            onSelectCallback = onSelect;

            if (categoryColorStrip != null) categoryColorStrip.color = book.coverColor;
            if (titleText != null) titleText.text = book.title;
            if (authorText != null) authorText.text = $"{book.author} ({book.publicationYear})";
            if (categoryPillText != null) categoryPillText.text = book.GetFormattedCategory();
            if (locationText != null) locationText.text = book.shelfLocationName;

            if (inspectButton != null)
            {
                inspectButton.onClick.RemoveAllListeners();
                inspectButton.onClick.AddListener(OnInspectClicked);
            }
        }

        private void OnInspectClicked()
        {
            if (boundBook != null)
            {
                onSelectCallback?.Invoke(boundBook);
            }
        }
    }
}

