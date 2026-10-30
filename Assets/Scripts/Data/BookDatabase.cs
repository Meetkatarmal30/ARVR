using System;
using System.Collections.Generic;
using UnityEngine;
using VirtualLibrary.Core;

namespace VirtualLibrary.Data
{
    /// <summary>
    /// Master repository of curated educational books for the Virtual Library.
    /// Supports real-time text query search, category filtering, and direct ID lookup.
    /// </summary>
    public class BookDatabase : MonoBehaviour
    {
        public static BookDatabase Instance { get; private set; }

        [SerializeField]
        private List<BookData> books = new List<BookData>();

        public IReadOnlyList<BookData> AllBooks => books;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            InitializeDefaultBooksIfEmpty();
        }

        /// <summary>
        /// Populates the catalog with 15 rich educational books across all 5 core categories.
        /// </summary>
        public void InitializeDefaultBooksIfEmpty()
        {
            if (books != null && books.Count >= 15) return;

            books = new List<BookData>
            {
                // ==================== PROGRAMMING & COMPUTER SCIENCE ====================
                new BookData(
                    "PROG-01",
                    "Clean Code: A Handbook of Agile Software Craftsmanship",
                    "Robert C. Martin",
                    BookCategory.Programming,
                    "2008",
                    "978-0132350884",
                    ReadingDifficulty.Intermediate,
                    "A cornerstone guide on writing readable, maintainable, and elegant software code.",
                    "Even bad code can function. But if code isn't clean, it can bring a development organization to its knees. Every year, countless hours and significant resources are lost because of poorly written code. This book instills the values of craftsmanship and professional software development.",
                    "- Meaningful names for variables, functions, and classes\n- Functions should do one thing and do it well\n- Formatting, error handling, and test-driven development (TDD)\n- Smells and heuristics for code refactoring",
                    "SDG 4: Quality Education & Technical Upskilling",
                    new Color(0.18f, 0.45f, 0.78f), // Deep Blue
                    new Vector3(-4.2f, 1.8f, 2.5f),
                    "Bookshelf 1 (Computing) - Tier 2",
                    "https://en.wikipedia.org/wiki/Clean_Code"
                ),
                new BookData(
                    "PROG-02",
                    "Introduction to Algorithms (CLRS)",
                    "Thomas H. Cormen, Charles E. Leiserson, Ronald L. Rivest, Clifford Stein",
                    BookCategory.Programming,
                    "2022",
                    "978-0262046305",
                    ReadingDifficulty.Advanced,
                    "The comprehensive global textbook standard on data structures and computational algorithmic design.",
                    "Covers a broad spectrum of algorithms in depth, yet makes their design and analysis accessible to all levels of readers. Rigorous explanations accompanied by mathematical proofs and pseudo-code demonstrate time and space complexity.",
                    "- Asymptotic notation, divide-and-conquer strategies\n- Dynamic programming and greedy approaches\n- Graph algorithms (BFS, DFS, Dijkstra, Bellman-Ford)\n- NP-completeness and approximation algorithms",
                    "SDG 4: Foundational Computing & Math Literacy",
                    new Color(0.12f, 0.58f, 0.65f), // Cyan Teal
                    new Vector3(-4.2f, 1.4f, 2.5f),
                    "Bookshelf 1 (Computing) - Tier 1",
                    "https://mitpress.mit.edu/9780262046305/introduction-to-algorithms/"
                ),
                new BookData(
                    "PROG-03",
                    "Designing Data-Intensive Applications",
                    "Martin Kleppmann",
                    BookCategory.Programming,
                    "2017",
                    "978-1449373320",
                    ReadingDifficulty.Advanced,
                    "The definitive manual on scalability, consistency, reliability, and distributed system architectures.",
                    "Data is at the center of many challenges in system design today. Difficult issues need to be figured out, such as scalability, consistency, reliability, efficiency, and maintainability. In addition, an overwhelming variety of tools exists, including relational databases, NoSQL datastores, stream processors, and message brokers.",
                    "- Reliability, scalability, and maintainability metrics\n- Storage engines, transaction processing, and consensus (Raft, Paxos)\n- Batch processing vs. stream processing\n- CAP theorem and PACELC trade-offs",
                    "SDG 9 & SDG 4: Digital Infrastructure & Skills",
                    new Color(0.85f, 0.35f, 0.15f), // Rust Red
                    new Vector3(-4.2f, 2.2f, 2.5f),
                    "Bookshelf 1 (Computing) - Tier 3",
                    "https://dataintensive.net/"
                ),

                // ==================== NATURAL SCIENCES ====================
                new BookData(
                    "SCI-01",
                    "A Brief History of Time",
                    "Stephen Hawking",
                    BookCategory.Science,
                    "1988",
                    "978-0553380163",
                    ReadingDifficulty.Beginner,
                    "Explores the origins, evolution, and ultimate fate of the universe from the Big Bang to Black Holes.",
                    "A landmark volume in science writing by one of the great minds of modern history. Hawking addresses fundamental questions such as: Was there a beginning of time? Could time run backward? Is the universe infinite or does it have boundaries?",
                    "- General Relativity and Quantum Mechanics unification\n- Spacetime fabric, singularity, and event horizons\n- The arrow of time and thermodynamic entropy\n- Wormholes and modern cosmological models",
                    "SDG 4: Inclusive Scientific Curiosity",
                    new Color(0.15f, 0.15f, 0.35f), // Midnight Navy
                    new Vector3(-4.2f, 1.8f, -1.0f),
                    "Bookshelf 2 (Sciences) - Tier 2",
                    "https://en.wikipedia.org/wiki/A_Brief_History_of_Time"
                ),
                new BookData(
                    "SCI-02",
                    "Cosmos: A Personal Voyage",
                    "Carl Sagan",
                    BookCategory.Science,
                    "1980",
                    "978-0345539434",
                    ReadingDifficulty.Beginner,
                    "A poetic journey through 15 billion years of cosmic evolution and civilization's quest for knowledge.",
                    "Sagan traces the origins of knowledge and the scientific method, mixing science and philosophy with an impassioned plea for planetary stewardship and nuclear disarmament. Cosmos invites learners to appreciate human existence as stardust contemplating the stars.",
                    "- The Cosmic Calendar: mapping 13.8 billion years to a single year\n- Planetary science, atmospheric runaway greenhouse effects\n- Search for Extraterrestrial Intelligence (SETI)\n- The fragility and unity of Spaceship Earth",
                    "SDG 4 & SDG 13: Science Education & Climate Awareness",
                    new Color(0.38f, 0.22f, 0.58f), // Deep Cosmos Purple
                    new Vector3(-4.2f, 1.4f, -1.0f),
                    "Bookshelf 2 (Sciences) - Tier 1",
                    "https://en.wikipedia.org/wiki/Cosmos_(Sagan_book)"
                ),
                new BookData(
                    "SCI-03",
                    "The Gene: An Intimate History",
                    "Siddhartha Mukherjee",
                    BookCategory.Science,
                    "2016",
                    "978-1476733524",
                    ReadingDifficulty.Intermediate,
                    "The story of genetics—from Mendel's peas to CRISPR gene editing and the future of human biology.",
                    "Mukherjee weaves science, social history, and personal narrative to tell the story of one of the most powerful and dangerous ideas in human history: the gene, the basic unit of heredity, and the basic unit of all biological information.",
                    "- Mendelian genetics and Darwinian evolutionary synthesis\n- Discovery of DNA double helix structure by Watson, Crick, and Franklin\n- Epigenetics, human genome sequencing, and moral dilemmas of CRISPR\n- The ethics of gene editing in medicine",
                    "SDG 3 & SDG 4: Health Sciences & Ethical Literacy",
                    new Color(0.25f, 0.55f, 0.32f), // Forest Green
                    new Vector3(-4.2f, 2.2f, -1.0f),
                    "Bookshelf 2 (Sciences) - Tier 3",
                    "https://en.wikipedia.org/wiki/The_Gene:_An_Intimate_History"
                ),

                // ==================== MATHEMATICS ====================
                new BookData(
                    "MATH-01",
                    "Calculus: Early Transcendentals",
                    "James Stewart",
                    BookCategory.Mathematics,
                    "2015",
                    "978-1285741550",
                    ReadingDifficulty.Intermediate,
                    "The definitive pedagogical resource for differential, integral, and multivariable calculus.",
                    "Stewart's Calculus offers clear explanations, thoughtfully chosen examples, and exceptional exercise sets that have guided millions of STEM students worldwide to master rates of change and accumulation.",
                    "- Limits, continuity, and instantaneous rates of change\n- The Fundamental Theorem of Calculus\n- Integration techniques, parametric equations, and polar coordinates\n- Multivariable vector calculus, divergence, and curl theorems",
                    "SDG 4: STEM Education & Quantitative Foundations",
                    new Color(0.78f, 0.22f, 0.22f), // Crimson
                    new Vector3(4.2f, 1.8f, 2.5f),
                    "Bookshelf 3 (Mathematics) - Tier 2",
                    "https://en.wikipedia.org/wiki/James_Stewart_(mathematician)"
                ),
                new BookData(
                    "MATH-02",
                    "Linear Algebra and Its Applications",
                    "Gilbert Strang",
                    BookCategory.Mathematics,
                    "2006",
                    "978-0030105678",
                    ReadingDifficulty.Intermediate,
                    "A vibrant, conceptual presentation of vector spaces, matrices, and eigenvalues in modern computing.",
                    "Strang's approach emphasizes the four fundamental subspaces, providing intuitive geometric and physical insights that power modern graphics, machine learning algorithms, and engineering simulations.",
                    "- Vector spaces, null spaces, and column spaces\n- Matrix factorizations (LU, QR, SVD - Singular Value Decomposition)\n- Eigenvalues, eigenvectors, and Markov matrices\n- Applications to 3D computer graphics and machine learning",
                    "SDG 4: High-Level Analytical & AI Foundations",
                    new Color(0.85f, 0.55f, 0.15f), // Amber Gold
                    new Vector3(4.2f, 1.4f, 2.5f),
                    "Bookshelf 3 (Mathematics) - Tier 1",
                    "https://math.mit.edu/~gs/linearalgebra/"
                ),
                new BookData(
                    "MATH-03",
                    "The Art of Problem Solving: Vol 1",
                    "Richard Rusczyk, Sandor Lehoczky",
                    BookCategory.Mathematics,
                    "2006",
                    "978-0977304561",
                    ReadingDifficulty.Beginner,
                    "Cultivates creative mathematical thinking, Olympiad problem-solving, and logical deduction.",
                    "Specifically crafted to take students beyond routine formulas and encourage creative, deep mathematical thinking. Teaches how to tackle unfamiliar challenges through methodical reasoning.",
                    "- Number theory, modular arithmetic, and prime properties\n- Combinatorics, permutations, and probability principles\n- Non-standard geometry and algebraic transformations\n- Breaking down complex problems into solvable lemmas",
                    "SDG 4 & SDG 10: Equalizing Access to Competitive Math",
                    new Color(0.20f, 0.65f, 0.55f), // Emerald Mint
                    new Vector3(4.2f, 2.2f, 2.5f),
                    "Bookshelf 3 (Mathematics) - Tier 3",
                    "https://artofproblemsolving.com/"
                ),

                // ==================== WORLD LITERATURE & FICTION ====================
                new BookData(
                    "FICT-01",
                    "To Kill a Mockingbird",
                    "Harper Lee",
                    BookCategory.Fiction,
                    "1960",
                    "978-0060935467",
                    ReadingDifficulty.Beginner,
                    "A masterpiece exploring empathy, social justice, racial inequality, and the loss of innocence.",
                    "Set in the American South during the Great Depression, the story follows young Scout Finch, whose father Atticus is appointed to defend a Black man falsely accused of a crime. Explores the destructive nature of racial prejudice and the power of human conscience.",
                    "- You never really understand a person until you climb into his skin and walk around in it\n- Moral courage against social conformity\n- Systemic racial injustice and legal inequality\n- Compassion and childhood maturation",
                    "SDG 10: Reduced Inequalities & Social Justice",
                    new Color(0.68f, 0.42f, 0.25f), // Warm Terracotta
                    new Vector3(4.2f, 1.8f, -1.0f),
                    "Bookshelf 4 (Literature & Civics) - Tier 2",
                    "https://en.wikipedia.org/wiki/To_Kill_a_Mockingbird"
                ),
                new BookData(
                    "FICT-02",
                    "1984: Dystopian Horizons",
                    "George Orwell",
                    BookCategory.Fiction,
                    "1949",
                    "978-0451524935",
                    ReadingDifficulty.Intermediate,
                    "A chilling warning on totalitarian surveillance, psychological manipulation, and censorship.",
                    "Winston Smith struggles against the all-seeing Big Brother and the Party's Ministry of Truth, where historical records are constantly rewritten and independent thought is branded as 'thoughtcrime'. A vital cultural touchstone for understanding information integrity.",
                    "- Doublethink, Newspeak, and linguistic constraints on freedom of thought\n- Panoptic surveillance and state overreach\n- Truth distortion, propaganda, and memory erasure\n- The fragility of human autonomy in authoritarian systems",
                    "SDG 16 & SDG 4: Democratic Freedoms & Critical Literacy",
                    new Color(0.35f, 0.35f, 0.38f), // Charcoal Slate
                    new Vector3(4.2f, 1.4f, -1.0f),
                    "Bookshelf 4 (Literature & Civics) - Tier 1",
                    "https://en.wikipedia.org/wiki/Nineteen_Eighty-Four"
                ),
                new BookData(
                    "FICT-03",
                    "Fahrenheit 451: The Power of Books",
                    "Ray Bradbury",
                    BookCategory.Fiction,
                    "1953",
                    "978-1451673319",
                    ReadingDifficulty.Beginner,
                    "A poignant celebration of literature, critical thought, and resistance against intellectual complacency.",
                    "Guy Montag is a fireman whose job is to burn books, which are banned in a futuristic hedonistic society addicted to shallow screen culture. Montag begins to question his society after meeting an eccentric girl and witnessing the tragic sacrifice of an old woman who chooses to burn with her library.",
                    "- Books provide the quality of information and leisure to digest it\n- The right to carry out actions based on what we learn\n- The danger of intellectual complacency and screen-induced apathy\n- Cultural preservation through oral and written memory",
                    "SDG 4: Preservation of Knowledge & Freedom to Learn",
                    new Color(0.88f, 0.32f, 0.18f), // Flame Orange
                    new Vector3(4.2f, 2.2f, -1.0f),
                    "Bookshelf 4 (Literature & Civics) - Tier 3",
                    "https://en.wikipedia.org/wiki/Fahrenheit_451"
                ),

                // ==================== GENERAL KNOWLEDGE & HUMANITIES ====================
                new BookData(
                    "GK-01",
                    "Sapiens: A Brief History of Humankind",
                    "Yuval Noah Harari",
                    BookCategory.GeneralKnowledge,
                    "2014",
                    "978-0062316097",
                    ReadingDifficulty.Beginner,
                    "Surveys the cognitive, agricultural, and scientific revolutions that shaped the human species.",
                    "Harari spans all of human history, from the first humans to walk the earth to the radical, and sometimes devastating, breakthroughs of the Cognitive, Agricultural, and Scientific Revolutions. Draws on biology, anthropology, and economics to explain how we conquered the globe.",
                    "- The Cognitive Revolution: fiction, shared myths, and large-scale cooperation\n- The Agricultural Revolution as a transformative transition\n- The unifying forces of money, empires, and universal religions\n- The Scientific Revolution and technological acceleration",
                    "SDG 4: Interdisciplinary Global Understanding",
                    new Color(0.75f, 0.65f, 0.25f), // Sand Gold
                    new Vector3(-1.5f, 1.1f, -3.2f),
                    "Reception Reading Lounge - Display Rack",
                    "https://www.ynharari.com/book/sapiens/"
                ),
                new BookData(
                    "GK-02",
                    "Thinking, Fast and Slow",
                    "Daniel Kahneman",
                    BookCategory.GeneralKnowledge,
                    "2011",
                    "978-0374533557",
                    ReadingDifficulty.Intermediate,
                    "Nobel laureate exploration of behavioral economics, cognitive biases, and rational decision-making.",
                    "Kahneman explains the two systems that drive the way we think: System 1 is fast, intuitive, and emotional; System 2 is slower, more deliberative, and more logical. Highlights the faults and biases of fast thinking, and pervasive influence of cognitive heuristics.",
                    "- System 1 (Intuitive/Automatic) vs. System 2 (Reflective/Effortful)\n- Heuristics: Availability, representativeness, and anchoring\n- Loss aversion and Prospect Theory in human choices\n- Overconfidence bias and planning fallacies",
                    "SDG 4 & SDG 10: Critical Thinking & Bias Awareness",
                    new Color(0.28f, 0.52f, 0.62f), // Steel Azure
                    new Vector3(0.0f, 1.1f, -3.2f),
                    "Reception Reading Lounge - Display Rack",
                    "https://en.wikipedia.org/wiki/Thinking,_Fast_and_Slow"
                ),
                new BookData(
                    "GK-03",
                    "Educated: A Memoir",
                    "Tara Westover",
                    BookCategory.GeneralKnowledge,
                    "2018",
                    "978-0399590504",
                    ReadingDifficulty.Beginner,
                    "An inspiring testament to resilience, the transformative power of education, and self-invention.",
                    "Born to survivalists in the mountains of Idaho, Tara Westover was isolated from mainstream society and did not set foot in a classroom until she was seventeen. Her quest for knowledge transformed her, taking her over oceans and across continents to Harvard and Cambridge University.",
                    "- Education as a vehicle for self-determination and emancipation\n- Overcoming generational dogma, abuse, and geographic isolation\n- The disparity between indoctrination and genuine critical inquiry\n- Empowering underserved students through lifelong learning",
                    "SDG 4 & SDG 10: Transformative Power of Accessible Education",
                    new Color(0.82f, 0.45f, 0.55f), // Rose Heather
                    new Vector3(1.5f, 1.1f, -3.2f),
                    "Reception Reading Lounge - Display Rack",
                    "https://tarawestover.com/book"
                )
            };
        }

        /// <summary>
        /// Searches books matching query string across title, author, and description,
        /// filtered by the specified category.
        /// </summary>
        public List<BookData> SearchBooks(string query, BookCategory categoryFilter)
        {
            InitializeDefaultBooksIfEmpty();

            string cleanQuery = string.IsNullOrWhiteSpace(query) ? string.Empty : query.Trim().ToLowerInvariant();
            List<BookData> results = new List<BookData>();

            for (int i = 0; i < books.Count; i++)
            {
                BookData book = books[i];

                // Category filter check
                if (categoryFilter != BookCategory.All && book.category != categoryFilter)
                {
                    continue;
                }

                // Query filter check
                if (string.IsNullOrEmpty(cleanQuery))
                {
                    results.Add(book);
                    continue;
                }

                bool matchTitle = book.title != null && book.title.ToLowerInvariant().Contains(cleanQuery);
                bool matchAuthor = book.author != null && book.author.ToLowerInvariant().Contains(cleanQuery);
                bool matchDesc = book.shortDescription != null && book.shortDescription.ToLowerInvariant().Contains(cleanQuery);
                bool matchIsbn = book.isbn != null && book.isbn.ToLowerInvariant().Contains(cleanQuery);

                if (matchTitle || matchAuthor || matchDesc || matchIsbn)
                {
                    results.Add(book);
                }
            }

            return results;
        }

        /// <summary>
        /// Retrieves a book by its unique ID.
        /// </summary>
        public BookData GetBookById(string id)
        {
            InitializeDefaultBooksIfEmpty();
            if (string.IsNullOrEmpty(id)) return null;

            for (int i = 0; i < books.Count; i++)
            {
                if (string.Equals(books[i].id, id, StringComparison.OrdinalIgnoreCase))
                {
                    return books[i];
                }
            }
            return null;
        }

        /// <summary>
        /// Counts books belonging to a specific category.
        /// </summary>
        public int GetCategoryCount(BookCategory category)
        {
            InitializeDefaultBooksIfEmpty();
            if (category == BookCategory.All) return books.Count;

            int count = 0;
            for (int i = 0; i < books.Count; i++)
            {
                if (books[i].category == category) count++;
            }
            return count;
        }
    }
}

