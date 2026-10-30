using System;
using UnityEngine;
using VirtualLibrary.Data;

namespace VirtualLibrary.Core
{
    /// <summary>
    /// Central coordinator for application state, UI lifecycle, audio cues,
    /// and player navigation flow between exploration, inspection, and catalog search.
    /// </summary>
    public class LibraryManager : MonoBehaviour
    {
        public static LibraryManager Instance { get; private set; }

        [Header("State Tracking")]
        [SerializeField] private InteractionState currentState = InteractionState.WelcomeScreen;
        public InteractionState CurrentState => currentState;

        [Header("Active Selection")]
        [SerializeField] private BookData currentActiveBook;
        public BookData CurrentActiveBook => currentActiveBook;

        [Header("Waypoint & Wayfinding")]
        [SerializeField] private bool hasActiveWaypoint;
        [SerializeField] private Vector3 activeWaypointPosition;
        [SerializeField] private string activeWaypointLabel;

        public bool HasActiveWaypoint => hasActiveWaypoint;
        public Vector3 ActiveWaypointPosition => activeWaypointPosition;
        public string ActiveWaypointLabel => activeWaypointLabel;

        [Header("Exploration Analytics (SDG 4 Metrics)")]
        [SerializeField] private int booksReadCount;
        public int BooksReadCount => booksReadCount;

        // Events
        public event Action<InteractionState> OnStateChanged;
        public event Action<BookData> OnBookOpened;
        public event Action OnBookClosed;
        public event Action OnCatalogOpened;
        public event Action OnCatalogClosed;
        public event Action<Vector3, string> OnWaypointSet;
        public event Action OnWaypointCleared;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
        }

        private void Start()
        {
            SetState(InteractionState.WelcomeScreen);
        }

        private void Update()
        {
            HandleGlobalInput();
        }

        private void HandleGlobalInput()
        {
            // Escape key acts as a universal back / close / pause handler
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                switch (currentState)
                {
                    case InteractionState.ReadingBook:
                        CloseBookDetails();
                        break;
                    case InteractionState.SearchingCatalog:
                        CloseCatalog();
                        break;
                    case InteractionState.Exploring:
                        OpenWelcomeScreen();
                        break;
                    case InteractionState.WelcomeScreen:
                        // Already at top level
                        break;
                }
            }

            // Tab or M key toggles the Library Catalog Search
            if (Input.GetKeyDown(KeyCode.Tab) || Input.GetKeyDown(KeyCode.M))
            {
                if (currentState == InteractionState.Exploring)
                {
                    OpenCatalog();
                }
                else if (currentState == InteractionState.SearchingCatalog)
                {
                    CloseCatalog();
                }
            }
        }

        public void SetState(InteractionState newState)
        {
            currentState = newState;
            UpdateCursorAndMovementForState(newState);
            OnStateChanged?.Invoke(newState);
        }

        private void UpdateCursorAndMovementForState(InteractionState state)
        {
            bool isModalOpen = state == InteractionState.WelcomeScreen ||
                               state == InteractionState.ReadingBook ||
                               state == InteractionState.SearchingCatalog ||
                               state == InteractionState.Paused;

            if (isModalOpen)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }

        public void StartExploring()
        {
            SetState(InteractionState.Exploring);
            AudioManager.Instance?.PlaySoundEffect(AudioCueType.StartExplore);
        }

        public void OpenBookDetails(BookData book)
        {
            if (book == null) return;
            currentActiveBook = book;
            booksReadCount++;
            SetState(InteractionState.ReadingBook);
            OnBookOpened?.Invoke(book);
            AudioManager.Instance?.PlaySoundEffect(AudioCueType.BookOpen);
        }

        public void CloseBookDetails()
        {
            currentActiveBook = null;
            SetState(InteractionState.Exploring);
            OnBookClosed?.Invoke();
            AudioManager.Instance?.PlaySoundEffect(AudioCueType.BookClose);
        }

        public void OpenCatalog()
        {
            SetState(InteractionState.SearchingCatalog);
            OnCatalogOpened?.Invoke();
            AudioManager.Instance?.PlaySoundEffect(AudioCueType.MenuOpen);
        }

        public void CloseCatalog()
        {
            SetState(InteractionState.Exploring);
            OnCatalogClosed?.Invoke();
            AudioManager.Instance?.PlaySoundEffect(AudioCueType.MenuClose);
        }

        public void OpenWelcomeScreen()
        {
            SetState(InteractionState.WelcomeScreen);
            AudioManager.Instance?.PlaySoundEffect(AudioCueType.MenuOpen);
        }

        public void SetShelfWaypoint(Vector3 position, string label)
        {
            hasActiveWaypoint = true;
            activeWaypointPosition = position;
            activeWaypointLabel = label;
            OnWaypointSet?.Invoke(position, label);
            AudioManager.Instance?.PlaySoundEffect(AudioCueType.WaypointPing);
        }

        public void ClearShelfWaypoint()
        {
            hasActiveWaypoint = false;
            activeWaypointLabel = string.Empty;
            OnWaypointCleared?.Invoke();
        }

        public void QuitApplication()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}

