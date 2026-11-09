using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Encodings.Web;
using System.Text.Json;
using QuizTime.Models;

namespace QuizTime.Services
{
    // Leest de quizzen uit Data/quiz.json.
    // De vensters praten alleen met deze class, niet direct met het bestand.
    public class QuizService
    {

        // Het quiz.json-bestand in de projectmap (3 mappen boven bin\Debug\net10.0-windows).
        private readonly string _dataPath = Path.GetFullPath(
            Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "Data", "quiz.json"));

        public List<Quiz> LoadQuizzes()
        {
            if (!File.Exists(_dataPath))
            {
                return new List<Quiz>();
            }

            string json = File.ReadAllText(_dataPath);
            if (string.IsNullOrWhiteSpace(json))
            {
                return new List<Quiz>();
            }

            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            return JsonSerializer.Deserialize<List<Quiz>>(json, options) ?? new List<Quiz>();
        }


        // Schrijft alle quizzen terug naar quiz.json.
        public void SaveQuizzes(List<Quiz> quizzes)
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            };

            string json = JsonSerializer.Serialize(quizzes, options);
            File.WriteAllText(_dataPath, json);
        }
    }
}