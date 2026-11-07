using System.Collections.Generic;

namespace QuizTime.Models
{
    // Een quiz: een titel met alle vragen die erbij horen.
    public class Quiz
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public List<Question> Questions { get; set; } = new();
    }
}