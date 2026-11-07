using System;
using System.Collections.Generic;
using System.Text;

namespace QuizTime.Models
{
   
    public class Answer
    {
        public int Id { get; set; }
        public int QuestionId { get; set; }
        public string Label { get; set; } = string.Empty;
        public string Text { get; set; } = string.Empty;
        public bool IsCorrect { get; set; }
    }
}
