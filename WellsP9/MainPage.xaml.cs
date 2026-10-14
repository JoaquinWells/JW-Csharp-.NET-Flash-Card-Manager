// Name: Joaquin Wells
// Email: jwells32@student.cnm.edu
// File: MainPage.xaml.cs

using System.ComponentModel;

namespace WellsP9
{
    /// <summary>
    /// Code-behind for the main Flash Card window.
    /// Handles card presentation, right/wrong scoring, and full CRUD management.
    /// </summary>
    public partial class MainPage : ContentPage
    {
        #region Fields

        /// <summary>
        /// Binding list of all cards loaded from the database.
        /// </summary>
        private BindingList<Card> cards;

        /// <summary>
        /// The card currently displayed in the Flash Card tab.
        /// </summary>
        private Card? currentCard;

        /// <summary>
        /// The card selected in the Manage Cards CollectionView.
        /// </summary>
        private Card? selectedCard;

        /// <summary>
        /// Database manager providing all CRUD operations.
        /// </summary>
        private DBManager dbManager;

        /// <summary>
        /// Random number generator used by GetRandomCard().
        /// </summary>
        private Random random;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes the page, loads cards from the database, and displays the first random card.
        /// </summary>
        public MainPage()
        {
            InitializeComponent();
            dbManager = new DBManager();
            cards = new BindingList<Card>();
            random = new Random();

            GetCards();
            currentCard = GetRandomCard();
            DisplayCardQuestion();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Calls DBManager.GetCards() to reload the BindingList from the database,
        /// then refreshes the Manage tab CollectionView.
        /// </summary>
        private void GetCards()
        {
            dbManager.GetCards(cards);
            CardsCollectionView.ItemsSource = null;
            CardsCollectionView.ItemsSource = cards;
        }

        /// <summary>
        /// Randomly selects and returns one Card from the cards list using System.Random.
        /// Returns null if the list is empty.
        /// </summary>
        private Card? GetRandomCard()
        {
            if (cards.Count == 0) return null;
            int index = random.Next(cards.Count);
            return cards[index];
        }

        /// <summary>
        /// Displays the current card's ID, title, and question.
        /// Hides the answer panel and re-enables the Show Answer button.
        /// </summary>
        private void DisplayCardQuestion()
        {
            if (currentCard == null)
            {
                CardIDLabel.Text = "-";
                CardTitleLabel.Text = "(No cards available)";
                CardQuestionLabel.Text = "Add cards in the Manage Cards tab.";
                InstructionLabel.Text = string.Empty;
                ShowAnswerButton.IsEnabled = false;
                AnswerPanel.IsVisible = false;
                return;
            }

            CardIDLabel.Text = currentCard.CardID.ToString();
            CardTitleLabel.Text = currentCard.Title;
            CardQuestionLabel.Text = currentCard.Question;
            InstructionLabel.Text = "Think about the answer, then click Show Answer.";

            AnswerPanel.IsVisible = false;
            ShowAnswerButton.IsEnabled = true;
        }

        /// <summary>
        /// Switches the active tab visually by toggling panel visibility and button colors.
        /// </summary>
        /// <param name="showFlashCard">True to show the Flash Card tab; false for Manage Cards.</param>
        private void SwitchTab(bool showFlashCard)
        {
            // If switching to Flash Card tab and no card is loaded, pick one now
            if (showFlashCard && currentCard == null)
            {
                GetCards();
                currentCard = GetRandomCard();
                DisplayCardQuestion();
            }

            FlashCardPanel.IsVisible = showFlashCard;
            ManagePanel.IsVisible = !showFlashCard;
            FlashCardTabButton.BackgroundColor = showFlashCard
                ? Color.FromArgb("#512BD4") : Color.FromArgb("#CCCCCC");
            FlashCardTabButton.TextColor = showFlashCard
                ? Colors.White : Colors.Black;
            ManageTabButton.BackgroundColor = showFlashCard
                ? Color.FromArgb("#CCCCCC") : Color.FromArgb("#512BD4");
            ManageTabButton.TextColor = showFlashCard
                ? Colors.Black : Colors.White;
        }

        #endregion

        #region Event Handlers

        /// <summary>Switches to the Flash Card tab.</summary>
        private void FlashCardTabButton_Clicked(object? sender, EventArgs e) =>
            SwitchTab(true);

        /// <summary>Switches to the Manage Cards tab.</summary>
        private void ManageTabButton_Clicked(object? sender, EventArgs e) =>
            SwitchTab(false);

        /// <summary>
        /// Reveals the answer panel when the user is ready to check their answer.
        /// </summary>
        private void ShowAnswerButton_Clicked(object? sender, EventArgs e)
        {
            if (currentCard == null) return;
            CardAnswerLabel.Text = currentCard.Answer;
            AnswerPanel.IsVisible = true;
            ShowAnswerButton.IsEnabled = false;
        }

        /// <summary>
        /// Increments NumRight for the current card, saves to DB, then loads a new random card.
        /// </summary>
        private void RightButton_Clicked(object? sender, EventArgs e)
        {
            if (currentCard == null) return;
            currentCard.NumRight++;
            dbManager.Update(currentCard);
            GetCards();
            currentCard = GetRandomCard();
            DisplayCardQuestion();
        }

        /// <summary>
        /// Increments NumWrong for the current card, saves to DB, then loads a new random card.
        /// </summary>
        private void WrongButton_Clicked(object? sender, EventArgs e)
        {
            if (currentCard == null) return;
            currentCard.NumWrong++;
            dbManager.Update(currentCard);
            GetCards();
            currentCard = GetRandomCard();
            DisplayCardQuestion();
        }

        /// <summary>
        /// Populates the edit fields when the user selects a card in the Manage tab list.
        /// </summary>
        private void CardsCollectionView_SelectionChanged(object? sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.Count == 0)
            {
                selectedCard = null;
                return;
            }

            selectedCard = e.CurrentSelection[0] as Card;
            if (selectedCard == null) return;

            TitleEntry.Text     = selectedCard.Title;
            QuestionEditor.Text = selectedCard.Question;
            AnswerEditor.Text   = selectedCard.Answer;
        }

        /// <summary>
        /// Adds a new card to the database using the edit field values, then refreshes the list.
        /// </summary>
        private void AddButton_Clicked(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TitleEntry.Text))
            {
                DisplayAlert("Validation", "Please enter a title.", "OK");
                return;
            }

            Card newCard = new Card
            {
                Title    = TitleEntry.Text.Trim(),
                Question = QuestionEditor.Text?.Trim() ?? string.Empty,
                Answer   = AnswerEditor.Text?.Trim() ?? string.Empty,
                NumRight = 0,
                NumWrong = 0
            };

            dbManager.AddCard(newCard);
            GetCards();

            // If the flash card tab had no card (first card ever added), pick it now
            if (currentCard == null)
            {
                currentCard = GetRandomCard();
                DisplayCardQuestion();
            }

            TitleEntry.Text     = string.Empty;
            QuestionEditor.Text = string.Empty;
            AnswerEditor.Text   = string.Empty;
        }

        /// <summary>
        /// Updates the selected card in the database with the current edit field values.
        /// </summary>
        private void UpdateButton_Clicked(object? sender, EventArgs e)
        {
            if (selectedCard == null)
            {
                DisplayAlert("Selection", "Please select a card to update.", "OK");
                return;
            }

            selectedCard.Title    = TitleEntry.Text?.Trim() ?? selectedCard.Title;
            selectedCard.Question = QuestionEditor.Text?.Trim() ?? selectedCard.Question;
            selectedCard.Answer   = AnswerEditor.Text?.Trim() ?? selectedCard.Answer;

            dbManager.Update(selectedCard);
            GetCards();
        }

        /// <summary>
        /// Removes the selected card from the database, then refreshes the list.
        /// </summary>
        private void RemoveButton_Clicked(object? sender, EventArgs e)
        {
            if (selectedCard == null)
            {
                DisplayAlert("Selection", "Please select a card to remove.", "OK");
                return;
            }

            dbManager.RemoveCard(selectedCard);
            selectedCard = null;
            GetCards();

            TitleEntry.Text     = string.Empty;
            QuestionEditor.Text = string.Empty;
            AnswerEditor.Text   = string.Empty;
        }

        #endregion
    }
}
