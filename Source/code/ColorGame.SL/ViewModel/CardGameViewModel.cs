using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Threading;
using ColorGame.SL.View;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.ViewModel;

namespace ColorGame.SL.ViewModel
{
    public class CardGameViewModel : NotificationObject
    {
        #region Fields

        private static readonly string _instructions =
            string.Format(
                @"1. Click on the cards to flip them. At a time only 2 cards can be flipped, if both cards are of same colour you get +1 point and if they are different you get -1 point. {0}2. You can also use Keyboard to play this game. Use UP and DOWN arrow to navigate through cards and Enter key to flip the card.",
                Environment.NewLine);

        private DispatcherTimer _elapsedTimer;
        private DispatcherTimer _endTimer;

        private bool _isGameEnded, _isRectCollapsed;
        private CardViewModel _lastUpsideCard;
        private int _moveCounter;
        private int _pairCounter;
        private DispatcherTimer _resetTimer;
        private int _score;
        private int _timeCounter;
        private DispatcherTimer _timer;

        #endregion

        #region Properties

        public string Instructions
        {
            get { return _instructions; }
        }

        public bool IsGameEnded
        {
            get { return _isGameEnded; }

            set
            {
                _isGameEnded = value;
                RaisePropertyChanged(() => IsGameEnded);
            }
        }

        public int MoveCounter
        {
            get { return _moveCounter; }
            set
            {
                _moveCounter = value;
                RaisePropertyChanged(() => MoveCounter);
            }
        }

        public int PairCounter
        {
            get { return _pairCounter; }

            set
            {
                _pairCounter = value;

                // Update bindings, no broadcast
                RaisePropertyChanged(() => PairCounter);
            }
        }

        public int TimeCounter
        {
            get { return _timeCounter; }
            set
            {
                _timeCounter = value;
                RaisePropertyChanged(() => TimeCounter);
            }
        }

        public int Score
        {
            get { return _score; }
            set
            {
                _score = value;
                RaisePropertyChanged(() => Score);
            }
        }

        public bool IsRectCollapsed
        {
            get { return _isRectCollapsed; }
            set
            {
                _isRectCollapsed = value;
                RaisePropertyChanged(() => IsRectCollapsed);
            }
        }

        public ObservableCollection<CardViewModel> CardViewModels { get; set; }

        public DelegateCommand RestartGame { get; set; }

        #endregion

        public CardGameViewModel()
        {
            CardViewModels = new ObservableCollection<CardViewModel>();
            RestartGame = new DelegateCommand(OnRestart);
            IsRectCollapsed = true;
            Initialize();
        }

        public void Initialize()
        {
            _resetTimer = new DispatcherTimer();
            _resetTimer.Interval = new TimeSpan(0, 0, 1);
            _resetTimer.Tick += ResetHideTick;

            _timer = new DispatcherTimer {Interval = new TimeSpan(0, 0, 0, 0, 500)};
            _timer.Tick += CardTimerTick;

            _elapsedTimer = new DispatcherTimer {Interval = new TimeSpan(0, 0, 1)};

            _elapsedTimer.Tick += ElapsedTimerTick;

            StartGame();
            LoadCards();
        }

        private void StartGame()
        {
            Score = 0;
            MoveCounter = 0;
            TimeCounter = 0;
            PairCounter = 0;
            _elapsedTimer.Start();
        }

        public void LoadCards()
        {
            const int cardCount = 8;

            // clear
            CardViewModels.Clear();

            // add cards
            for (int i = 1; i <= cardCount; i++)
            {
                CardViewModels.Add(new CardViewModel(i)
                {
                    Upside = false
                });
                CardViewModels.Add(new CardViewModel(i)
                {
                    Upside = false
                });
            }

            // shuffle them
            var rand = new Random();
            for (int i = CardViewModels.Count - 1; i > 0; i--)
            {
                int n = rand.Next(i + 1);
                CardViewModel temp = CardViewModels[i];
                CardViewModels[i] = CardViewModels[n];
                CardViewModels[n] = temp;
            }

            IsGameEnded = false;
        }

        public void OnSelectionChanged(CardViewModel lastCard)
        {
            int upsideNo = CardViewModels.Count(x => x.Upside);

            if (upsideNo == 0)
            {
                _lastUpsideCard = lastCard;
                return;
            }

            if ((upsideNo == 1 && !lastCard.Upside) || (upsideNo == 2 && lastCard.Upside))
            {
                // one is upside + lastCard is 2, check IDs
                MoveCounter++;
                //var upsideCard = CardViewModels.First(m => m.Upside);

                if (_lastUpsideCard != null && _lastUpsideCard.Key == lastCard.Key)
                {
                    if (CardViewModels.Count(m => !m.Solved) == 2)
                    {
                        _elapsedTimer.Stop();
                        EndGame();
                    }

                    _lastUpsideCard.SetSolved();
                    lastCard.SetSolved();
                    PairCounter++;
                    Score++;
                    _lastUpsideCard = null;
                }
                else
                {
                    Score--;
                }
                if (!_timer.IsEnabled)
                {
                    _timer.Start();
                    IsRectCollapsed = false;
                    _resetTimer.Start();
                }
            }
        }

        public void ChangeCardSelection(CardViewModel lastCard)
        {
            //On setting it to true it will raise OnSelectionChange event due to Binding on its setter.
            //Handling on count 0, if card is selected by pressing enter key
            int upsideNo = CardViewModels.Count(x => x.Upside);
            if (upsideNo != 1)
            {
                _lastUpsideCard = lastCard;
            }
            lastCard.Upside = true;
        }

        private void EndGame()
        {
            if (_endTimer == null)
            {
                _endTimer = new DispatcherTimer {Interval = TimeSpan.FromMilliseconds(600)};
                _endTimer.Tick += EndTimerTick;
            }
            _endTimer.Start();
        }

        private void HideCards()
        {
            foreach (CardViewModel item in CardViewModels)
            {
                item.Upside = false;
            }
        }

        private void EndTimerTick(object sender, EventArgs e)
        {
            _endTimer.Stop();
            IsGameEnded = true;
            //MessageBox.Default.Send(Constants.EndGameMessage);
            //TODO: Score dialog
            var scoreInfo = new ScoreInfo(Score);
            scoreInfo.Show();
        }

        private void ResetHideTick(object sender, EventArgs e)
        {
            _resetTimer.Stop();
            IsRectCollapsed = true;
        }

        private void ElapsedTimerTick(object sender, EventArgs e)
        {
            TimeCounter++;
        }

        private void CardTimerTick(object sender, EventArgs e)
        {
            _timer.Stop();
            HideCards();
        }

        private void OnRestart()
        {
            _timer.Tick -= CardTimerTick;
            _timer = null;

            _elapsedTimer.Tick -= ElapsedTimerTick;
            _elapsedTimer = null;

            _resetTimer.Tick -= ResetHideTick;
            _resetTimer = null;

            if (_endTimer != null)
                _endTimer.Tick -= EndTimerTick;
            _endTimer = null;
            Initialize();
        }
    }
}