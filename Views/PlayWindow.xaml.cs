using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;
using QuizTime.Models;

namespace QuizTime.Views
{
    // Dit venster is het "speelscherm": het scherm dat aan het publiek/de
    // deelnemers wordt getoond, met de vraag, de antwoordopties en (in
    // spelen-modus) een aftel-timer.
    public partial class PlayWindow : Window
    {
        // Een DispatcherTimer is een timer die elke X tijd (hier: 1 seconde)
        // een "Tick"-event afvuurt. We gebruiken hem voor de countdown.
        private readonly DispatcherTimer _timer = new() { Interval = TimeSpan.FromSeconds(1) };

        // Hoeveel seconden er nog over zijn in de huidige countdown.
        private int _secondsLeft;

        // Hoeveel seconden een vraag standaard krijgt in "spelen"-modus.
        private const int SecondsPerQuestion = 30;

        public PlayWindow()
        {
            InitializeComponent();

            // We koppelen onze eigen methode (Timer_Tick) aan het Tick-event,
            // zodat die elke seconde automatisch wordt uitgevoerd zolang de timer loopt.
            _timer.Tick += Timer_Tick;
        }

        // Wordt vanuit OperatorWindow aangeroepen om een (nieuwe) vraag te tonen.
        // - isReviewMode: true = nakijkmodus (geen timer nodig).
        // - showAnswer: true = het juiste antwoord moet groen gemarkeerd worden.
        public void ShowQuestion(Question question, bool isReviewMode, bool showAnswer)
        {
            CategoryText.Text = question.Category;

            QuestionText.Text = question.Text;

            // We bouwen per antwoordoptie een "OptionDisplay"-object: de tekst
            // van de optie plus de kleur waarin hij getoond moet worden.
            var displayOptions = new ObservableCollection<OptionDisplay>();

            for (int i = 0; i < question.Options.Count; i++)
            {
                bool isCorrect = i == question.CorrectAnswerIndex;

                displayOptions.Add(new OptionDisplay
                {
                    Text = question.Options[i],
                    // Groen als dit het juiste antwoord ÉN we het antwoord mogen tonen;
                    // anders de standaard donkere achtergrondkleur.
                    Background = (showAnswer && isCorrect)
                        ? new SolidColorBrush(Color.FromRgb(76, 175, 80))   // groen
                        : new SolidColorBrush(Color.FromRgb(45, 45, 68))    // donker paars/grijs
                });
            }

            // ItemsSource koppelt onze lijst met opties aan de ItemsControl in de XAML,
            // die automatisch voor elke optie een "vak" (Border) tekent.
            OptionsList.ItemsSource = displayOptions;

            // De timer heeft alleen zin in spelen-modus; in nakijkmodus zetten we hem uit.
            if (isReviewMode)
            {
                _timer.Stop();
                TimerText.Text = string.Empty;
            }
            else
            {
                StartCountdown();
            }
        }

        // Zet de countdown terug op het startaantal seconden en start de timer.
        private void StartCountdown()
        {
            _secondsLeft = SecondsPerQuestion;
            TimerText.Text = _secondsLeft.ToString();
            _timer.Start();
        }

        // Wordt automatisch elke seconde aangeroepen zolang de timer loopt.
        private void Timer_Tick(object? sender, EventArgs e)
        {
            _secondsLeft--;
            TimerText.Text = _secondsLeft.ToString();

            // Zodra de tijd op is, stoppen we de timer. De operator schakelt
            // zelf naar "nakijken" zodra iedereen klaar is met antwoorden.
            if (_secondsLeft <= 0)
            {
                _timer.Stop();
                TimerText.Text = "Tijd voorbij!";
            }
        }
    }

    // Klein hulpklasje, alleen bedoeld om een antwoordoptie samen met zijn
    // achtergrondkleur aan de UI (de ItemsControl) door te geven.
    public class OptionDisplay
    {
        public string Text { get; set; } = string.Empty;
        public Brush Background { get; set; } = Brushes.Transparent;
    }
}