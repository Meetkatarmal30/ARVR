using System;
using UnityEngine;
using VirtualLibrary.Core;

namespace VirtualLibrary.Data
{
    /// <summary>
    /// Represents comprehensive metadata for an educational book in the virtual library.
    /// Aligns with SDG 4 (Quality Education) and SDG 10 (Reduced Inequalities) by providing
    /// open, accessible summaries, difficulty indicators, and key learning objectives.
    /// </summary>
    [System.Serializable]
    public class BookData
    {
        [Header("Identification")]
        public string id;
        public string title;
        public string author;
        public BookCategory category;
        public string publicationYear;
        public string isbn;

        [Header("Pedagogical Details")]
        public ReadingDifficulty difficulty;
        [TextArea(2, 4)]
        public string shortDescription;
        [TextArea(5, 10)]
        public string fullOverview;
        [TextArea(3, 6)]
        public string keyTakeaways;
        public string targetSDG;
        public string externalLearnUrl;

        [Header("Visual & Spatial Placement")]
        public Color coverColor = Color.blue;
        public Vector3 shelfWorldPosition;
        public string shelfLocationName;

        public BookData() { }

        public BookData(
            string id,
            string title,
            string author,
            BookCategory category,
            string year,
            string isbn,
            ReadingDifficulty difficulty,
            string shortDesc,
            string overview,
            string takeaways,
            string sdg,
            Color color,
            Vector3 shelfPos,
            string shelfName,
            string url = "")
        {
            this.id = id;
            this.title = title;
            this.author = author;
            this.category = category;
            this.publicationYear = year;
            this.isbn = isbn;
            this.difficulty = difficulty;
            this.shortDescription = shortDesc;
            this.fullOverview = overview;
            this.keyTakeaways = takeaways;
            this.targetSDG = sdg;
            this.coverColor = color;
            this.shelfWorldPosition = shelfPos;
            this.shelfLocationName = shelfName;
            this.externalLearnUrl = url;
        }

        public string GetFormattedCategory()
        {
            switch (category)
            {
                case BookCategory.Programming: return "Programming & CS";
                case BookCategory.Science: return "Natural Sciences";
                case BookCategory.Mathematics: return "Mathematics";
                case BookCategory.Fiction: return "World Literature";
                case BookCategory.GeneralKnowledge: return "General Knowledge";
                default: return "General";
            }
        }

        public string GetDifficultyBadge()
        {
            switch (difficulty)
            {
                case ReadingDifficulty.Beginner: return "Beginner Friendly";
                case ReadingDifficulty.Intermediate: return "Intermediate";
                case ReadingDifficulty.Advanced: return "Advanced Academic";
                default: return "All Levels";
            }
        }
    }
}

