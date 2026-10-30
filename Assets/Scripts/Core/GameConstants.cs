using System;

namespace VirtualLibrary.Core
{
    /// <summary>
    /// Global application constants, tags, layers, and configuration constants.
    /// </summary>
    public static class GameConstants
    {
        public const string APP_NAME = "VR Virtual Library for Interactive Learning";
        public const string APP_VERSION = "1.0.0";
        public const string CIAP_MODULE = "Community / Industry Action Project";

        // Tags
        public const string TAG_INTERACTABLE = "Interactable";
        public const string TAG_BOOK = "Book";
        public const string TAG_BOOKSHELF = "Bookshelf";
        public const string TAG_TERMINAL = "Terminal";
        public const string TAG_PLAYER = "Player";

        // Layers
        public const int LAYER_INTERACTABLE = 10;
        public const int LAYER_BOOKS = 11;
        public const int LAYER_PLAYER = 12;

        // Interaction Limits
        public const float MAX_RAYCAST_DISTANCE = 3.5f;
        public const float WAYPOINT_ARRIVAL_DISTANCE = 1.8f;

        // Audio Constants
        public const int AUDIO_SAMPLE_RATE = 44100;
    }
}

