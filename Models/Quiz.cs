using System.Collections.Generic;

namespace QuizTime.Models
{
    // De "Quiz" class bundelt een titel met alle vragen die erbij horen.
    // Dit is het object dat we uiteindelijk uit het JSON-bestand inladen.
    public class Quiz
    {
        // Titel van de quiz, bijvoorbeeld "Docentenquiz Blok 3".
        public string Title { get; set; } = string.Empty;

        // De volledige lijst met vragen (elk een Question-object) van deze quiz.
        public List<Question> Questions { get; set; } = new();
    }
}