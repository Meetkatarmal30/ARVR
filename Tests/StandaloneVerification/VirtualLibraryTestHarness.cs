using System;
using System.Collections.Generic;
using UnityEngine;
using VirtualLibrary.Core;
using VirtualLibrary.Data;

namespace VirtualLibrary.Tests
{
    public class VirtualLibraryTestHarness
    {
        private static int passedTests = 0;
        private static int failedTests = 0;

        public static int Main(string[] args)
        {
            Console.WriteLine("===============================================================================");
            Console.WriteLine("  VR VIRTUAL LIBRARY FOR INTERACTIVE LEARNING — STANDALONE AUTOMATED TEST SUITE");
            Console.WriteLine("  CIAP Quality Assurance & Pedagogical Data Verification Engine");
            Console.WriteLine("===============================================================================\n");

            BookDatabase db = new BookDatabase();
            db.InitializeDefaultBooksIfEmpty();

            RunTest("Test 01: Master Catalog Size >= 12 Books", () =>
            {
                Assert(db.AllBooks.Count >= 12, $"Expected at least 12 books, found {db.AllBooks.Count}");
                Assert(db.AllBooks.Count == 15, $"Expected 15 curated books, found {db.AllBooks.Count}");
            });

            RunTest("Test 02: Educational Book Metadata Completeness", () =>
            {
                foreach (var book in db.AllBooks)
                {
                    Assert(!string.IsNullOrWhiteSpace(book.id), $"Book ID missing for '{book.title}'");
                    Assert(!string.IsNullOrWhiteSpace(book.title), $"Book Title missing for ID {book.id}");
                    Assert(!string.IsNullOrWhiteSpace(book.author), $"Book Author missing for '{book.title}'");
                    Assert(!string.IsNullOrWhiteSpace(book.isbn), $"Book ISBN missing for '{book.title}'");
                    Assert(!string.IsNullOrWhiteSpace(book.shortDescription), $"Short description missing for '{book.title}'");
                    Assert(!string.IsNullOrWhiteSpace(book.fullOverview), $"Full overview missing for '{book.title}'");
                    Assert(!string.IsNullOrWhiteSpace(book.keyTakeaways), $"Key takeaways missing for '{book.title}'");
                    Assert(!string.IsNullOrWhiteSpace(book.shelfLocationName), $"Shelf location name missing for '{book.title}'");
                }
            });

            RunTest("Test 03: 3D Spatial Shelf Coordinates Within Room Bounds", () =>
            {
                // Room is 16m wide (-8 to +8), 4.5m high (0 to 4.5), 24m long (-12 to +12)
                foreach (var book in db.AllBooks)
                {
                    Vector3 pos = book.shelfWorldPosition;
                    Assert(pos.x >= -7.5f && pos.x <= 7.5f, $"Book '{book.title}' X coordinate {pos.x} outside room walls.");
                    Assert(pos.y >= 0.5f && pos.y <= 4.0f, $"Book '{book.title}' Y coordinate {pos.y} outside shelf height.");
                    Assert(pos.z >= -11.5f && pos.z <= 11.5f, $"Book '{book.title}' Z coordinate {pos.z} outside room depth.");
                }
            });

            RunTest("Test 04: Category Distribution Across All 5 SDG Disciplines", () =>
            {
                Assert(db.GetCategoryCount(BookCategory.Programming) == 3, "Expected 3 Programming books.");
                Assert(db.GetCategoryCount(BookCategory.Science) == 3, "Expected 3 Science books.");
                Assert(db.GetCategoryCount(BookCategory.Mathematics) == 3, "Expected 3 Mathematics books.");
                Assert(db.GetCategoryCount(BookCategory.Fiction) == 3, "Expected 3 Fiction books.");
                Assert(db.GetCategoryCount(BookCategory.GeneralKnowledge) == 3, "Expected 3 General Knowledge books.");
                Assert(db.GetCategoryCount(BookCategory.All) == 15, "Expected 15 total books for All category.");
            });

            RunTest("Test 05: Exact Title Search", () =>
            {
                var results = db.SearchBooks("Clean Code", BookCategory.All);
                Assert(results.Count == 1, $"Expected 1 result for 'Clean Code', got {results.Count}");
                Assert(results[0].id == "PROG-01", $"Expected PROG-01, got {results[0].id}");
            });

            RunTest("Test 06: Case-Insensitive Substring Search", () =>
            {
                var results = db.SearchBooks("algorithms", BookCategory.All);
                Assert(results.Count >= 1, $"Expected at least 1 result for 'algorithms', got {results.Count}");
                Assert(results[0].title.Contains("Algorithms"), "Result title did not contain 'Algorithms'");
            });

            RunTest("Test 07: Author Name Search", () =>
            {
                var results = db.SearchBooks("Hawking", BookCategory.All);
                Assert(results.Count == 1, $"Expected 1 result for 'Hawking', got {results.Count}");
                Assert(results[0].id == "SCI-01", $"Expected SCI-01, got {results[0].id}");
            });

            RunTest("Test 08: ISBN Search", () =>
            {
                var results = db.SearchBooks("978-0060935467", BookCategory.All);
                Assert(results.Count == 1, $"Expected 1 result for Harper Lee ISBN, got {results.Count}");
                Assert(results[0].id == "FICT-01", $"Expected FICT-01, got {results[0].id}");
            });

            RunTest("Test 09: Cross-Filter Category Search", () =>
            {
                // "history" exists in both Science (Brief History of Time) and General Knowledge (Sapiens: A Brief History)
                var scienceHistory = db.SearchBooks("history", BookCategory.Science);
                Assert(scienceHistory.Count == 2, $"Expected 2 Science history books (Hawking & Gene), got {scienceHistory.Count}");

                var gkHistory = db.SearchBooks("history", BookCategory.GeneralKnowledge);
                Assert(gkHistory.Count == 1, $"Expected 1 General Knowledge history book (Sapiens), got {gkHistory.Count}");

                var mathHistory = db.SearchBooks("history", BookCategory.Mathematics);
                Assert(mathHistory.Count == 0, $"Expected 0 Mathematics history books, got {mathHistory.Count}");
            });

            RunTest("Test 10: Empty Query Returns Entire Selected Category", () =>
            {
                var results = db.SearchBooks("", BookCategory.Programming);
                Assert(results.Count == 3, $"Expected 3 books for empty query under Programming, got {results.Count}");
            });

            RunTest("Test 11: Non-Existent Search Query Returns Zero Results", () =>
            {
                var results = db.SearchBooks("NonExistentBookXYZ123", BookCategory.All);
                Assert(results.Count == 0, $"Expected 0 results for non-existent query, got {results.Count}");
            });

            RunTest("Test 12: Direct ID Lookup", () =>
            {
                var book = db.GetBookById("MATH-02");
                Assert(book != null, "Lookup for MATH-02 returned null");
                Assert(book.title.Contains("Linear Algebra"), $"Expected Linear Algebra, got '{book.title}'");

                var nonExistent = db.GetBookById("INVALID-ID");
                Assert(nonExistent == null, "Lookup for non-existent ID did not return null");
            });

            RunTest("Test 13: Waypoint Distance & Proximity Logic", () =>
            {
                Vector3 playerPos = new Vector3(0f, 1f, -10f); // Entrance
                Vector3 shelfPos = new Vector3(-4.2f, 1.8f, 2.5f); // Bookshelf 1
                float distance = Vector3.Distance(playerPos, shelfPos);

                Assert(distance > 13f && distance < 14f, $"Expected distance ~13.2m, got {distance:F2}m");

                // Near arrival test
                Vector3 nearPos = new Vector3(-4.0f, 1.5f, 2.0f);
                float nearDistance = Vector3.Distance(nearPos, shelfPos);
                Assert(nearDistance < 1.8f, $"Expected arrival within 1.8m threshold, got {nearDistance:F2}m");
            });

            RunTest("Test 14: Raycast Interaction Direction & Distance Limits", () =>
            {
                Vector3 playerCameraPos = new Vector3(0f, 1.65f, 0f);
                Vector3 forwardRay = new Vector3(0f, 0f, 1f); // Looking North

                Vector3 objectInFront = new Vector3(0f, 1.65f, 2.5f); // 2.5m away, directly in front
                Vector3 dirToObj = (objectInFront - playerCameraPos).normalized;
                float dot = Vector3.Dot(forwardRay, dirToObj);
                float dist = Vector3.Distance(playerCameraPos, objectInFront);

                Assert(dot > 0.99f, $"Expected target directly in front (dot ~1.0), got {dot}");
                Assert(dist <= 3.5f, $"Expected object within 3.5m interaction threshold, got {dist}");

                Vector3 objectBehind = new Vector3(0f, 1.65f, -2.5f);
                float dotBehind = Vector3.Dot(forwardRay, (objectBehind - playerCameraPos).normalized);
                Assert(dotBehind < 0f, $"Expected object behind player to have negative dot product, got {dotBehind}");
            });

            RunTest("Test 15: Procedural Sound Synthesis Mathematical Stability", () =>
            {
                // Verify procedural audio math doesn't produce NaN or Infinity
                int sampleRate = 44100;
                float duration = 0.1f;
                int totalSamples = (int)(sampleRate * duration);

                for (int i = 0; i < totalSamples; i++)
                {
                    float t = (float)i / sampleRate;
                    float freq = 800f - (t / duration) * 300f;
                    float envelope = 1f - (t / duration);
                    float sample = Mathf.Sin(2 * Mathf.PI * freq * t) * envelope * 0.3f;

                    Assert(!float.IsNaN(sample) && !float.IsInfinity(sample), $"Audio sample {i} resulted in invalid float {sample}");
                    Assert(Math.Abs(sample) <= 1.0f, $"Audio sample {i} clipped beyond range: {sample}");
                }
            });

            Console.WriteLine("\n-------------------------------------------------------------------------------");
            Console.WriteLine($"  TEST RUN COMPLETE: {passedTests} PASSED, {failedTests} FAILED (Total: {passedTests + failedTests})");
            Console.WriteLine("-------------------------------------------------------------------------------\n");

            return failedTests > 0 ? 1 : 0;
        }

        private static void RunTest(string testName, Action testAction)
        {
            try
            {
                testAction();
                Console.WriteLine($"[PASS] {testName}");
                passedTests++;
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"[FAIL] {testName}: {ex.Message}");
                Console.ResetColor();
                failedTests++;
            }
        }

        private static void Assert(bool condition, string failureMessage)
        {
            if (!condition)
            {
                throw new Exception(failureMessage);
            }
        }
    }
}

