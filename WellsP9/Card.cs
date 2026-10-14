// Name: Joaquin Wells
// Email: jwells32@student.cnm.edu
// File: Card.cs

using System.ComponentModel;

namespace WellsP9
{
    /// <summary>
    /// Represents a single flash card stored in the database.
    /// Implements INotifyPropertyChanged for BindingList compatibility.
    /// </summary>
    public class Card : INotifyPropertyChanged
    {
        #region Fields

        private int _cardID;
        private string _title = string.Empty;
        private string _question = string.Empty;
        private string _answer = string.Empty;
        private int _numRight;
        private int _numWrong;

        #endregion

        #region Properties

        /// <summary>The unique identifier assigned by the database.</summary>
        public int CardID
        {
            get => _cardID;
            set { _cardID = value; OnPropertyChanged(nameof(CardID)); }
        }

        /// <summary>A short title or topic label for the flash card.</summary>
        public string Title
        {
            get => _title;
            set { _title = value; OnPropertyChanged(nameof(Title)); }
        }

        /// <summary>The question or prompt shown to the user.</summary>
        public string Question
        {
            get => _question;
            set { _question = value; OnPropertyChanged(nameof(Question)); }
        }

        /// <summary>The answer revealed after the user thinks.</summary>
        public string Answer
        {
            get => _answer;
            set { _answer = value; OnPropertyChanged(nameof(Answer)); }
        }

        /// <summary>Running count of times the user answered correctly.</summary>
        public int NumRight
        {
            get => _numRight;
            set { _numRight = value; OnPropertyChanged(nameof(NumRight)); }
        }

        /// <summary>Running count of times the user answered incorrectly.</summary>
        public int NumWrong
        {
            get => _numWrong;
            set { _numWrong = value; OnPropertyChanged(nameof(NumWrong)); }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Parameterless constructor — chains to the full constructor with default values.
        /// </summary>
        public Card() : this(0, string.Empty, string.Empty, string.Empty, 0, 0) { }

        /// <summary>
        /// Full 6-parameter constructor.
        /// </summary>
        /// <param name="cardID">Database-assigned ID.</param>
        /// <param name="title">Card title.</param>
        /// <param name="question">Card question.</param>
        /// <param name="answer">Card answer.</param>
        /// <param name="numRight">Number of correct answers.</param>
        /// <param name="numWrong">Number of incorrect answers.</param>
        public Card(int cardID, string title, string question, string answer, int numRight, int numWrong)
        {
            _cardID = cardID;
            _title = title;
            _question = question;
            _answer = answer;
            _numRight = numRight;
            _numWrong = numWrong;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Returns a display string showing the card title and right/wrong stats.
        /// Used by the CollectionView to label each card in the list.
        /// </summary>
        public override string ToString() =>
            $"{Title}  (Right: {NumRight} / Wrong: {NumWrong})";

        /// <summary>Fires when a property value changes.</summary>
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>Raises the PropertyChanged event for the given property name.</summary>
        /// <param name="propertyName">Name of the property that changed.</param>
        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        #endregion
    }
}
