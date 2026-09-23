using System.Text.Json.Serialization;

public class Question
{
    public string Text { get; set; } = string.Empty;
    public List<string> Options { get; set; } = new();

    [JsonPropertyName("correctAnswerIndex")]
    public int CorrectAnswerIndex { get; set; }

    // الفئة اللي ينتمي لها هالسؤال، مثلاً "Dieren" أو "Geschiedenis".
    // نستخدمها بعدين لنعرض تصنيف السؤال، أو نصفّي الأسئلة حسب الفئة.
    public string Category { get; set; } = string.Empty;
}