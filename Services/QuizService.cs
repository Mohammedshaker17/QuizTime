using System;
using System.IO;
using System.Text.Json;
using QuizTime.Models;

namespace QuizTime.Services
{
    // Deze class is onze eigen "API"-laag binnen de applicatie: de vensters (Views)
    // vragen via deze class om quizdata, zonder te hoeven weten HOE die data wordt
    // opgehaald. Nu lezen we een lokaal JSON-bestand; later zou je deze class
    // kunnen vervangen door een echte webAPI-aanroep, zonder dat de vensters
    // hoeven te veranderen. Dat noemen we "loose coupling" (losse koppeling).
    public class QuizService
    {
        // Pad naar het JSON-bestand met de quizvragen.
        private readonly string _dataPath;

        // Standaard zoeken we naar Data/quiz.json, relatief vanaf de map
        // waar de applicatie draait (de "output"-map).
        public QuizService(string dataPath = "Data/quiz.json")
        {
            _dataPath = dataPath;
        }

        // Laadt de quiz uit het JSON-bestand en zet ("deserialiseert") de tekst
        // om naar een Quiz-object dat we in C# kunnen gebruiken.
        public Quiz LoadQuiz()
        {
            if (!File.Exists(_dataPath))
            {
                throw new FileNotFoundException($"Quizbestand niet gevonden: {_dataPath}");
            }

            // Lees de volledige inhoud van het JSON-bestand als platte tekst.
            string json = File.ReadAllText(_dataPath);

            // PropertyNameCaseInsensitive = true zorgt dat "title" in het JSON-bestand
            // ook matcht met de property "Title" in onze Quiz-class (hoofdletters
            // maken dan niet uit).
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            // JsonSerializer.Deserialize zet de JSON-tekst om in een echt C#-object.
            Quiz? quiz = JsonSerializer.Deserialize<Quiz>(json, options);

            // Als het bestand om wat voor reden dan ook leeg/ongeldig was,
            // geven we liever een lege (maar geldige) Quiz terug dan null.
            return quiz ?? new Quiz();
        }
    }
}