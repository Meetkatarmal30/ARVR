using System;

namespace VirtualLibrary.Core
{
    /// <summary>
    /// Educational categories for library catalog classification.
    /// Supports SDG 4 (Quality Education) multi-disciplinary learning.
    /// </summary>
    public enum BookCategory
    {
        All = 0,
        Programming = 1,
        Science = 2,
        Mathematics = 3,
        Fiction = 4,
        GeneralKnowledge = 5
    }

    /// <summary>
    /// Complexity level of reading material.
    /// </summary>
    public enum ReadingDifficulty
    {
        Beginner = 1,
        Intermediate = 2,
        Advanced = 3
    }

    /// <summary>
    /// Global application interaction state machine.
    /// </summary>
    public enum InteractionState
    {
        WelcomeScreen,
        Exploring,
        ReadingBook,
        SearchingCatalog,
        Paused
    }
}

