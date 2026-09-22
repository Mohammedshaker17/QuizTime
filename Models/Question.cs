using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace QuizTime.Models
{
    // Een "Question" is een class (blauwdruk) voor één vraag in de quiz.
    // Door dit als class te maken, kunnen we overal in de code met één
    // "Question"-object werken in plaats van losse tekst- en lijst-variabelen.
    public class Question
    {
        // De vraagtekst zelf, bijvoorbeeld: "Wat is de hoofdstad van Nederland?"
        public string Text { get; set; } = string.Empty;

        // Alle antwoordmogelijkheden voor deze vraag (meestal 4 opties).
        public List<string> Options { get; set; } = new();

        // De positie (index) van het juiste antwoord in de Options-lijst.
        // Let op: lijsten in C# beginnen te tellen vanaf 0, dus de eerste
        // optie heeft index 0, de tweede index 1, enzovoort.
        [JsonPropertyName("correctAnswerIndex")]
        public int CorrectAnswerIndex { get; set; }
    }
}