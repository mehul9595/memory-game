using System;
using System.Windows.Threading;
using Microsoft.Practices.Prism.ViewModel;

namespace ColorGame.SL.ViewModel
{
    public class CardViewModel : NotificationObject
    {
        #region Fields

        private int _height;
        private string _key;
        private bool _solved;
        private DispatcherTimer _solvedTimer;
        private bool _upside;
        private int _width;

        #endregion Fields

        #region Properties

        public bool Upside
        {
            get { return _upside; }
            set
            {
                _upside = value;
                RaisePropertyChanged(() => Upside);
            }
        }

        public string Key
        {
            get { return _key; }
            set
            {
                _key = value;

                RaisePropertyChanged(() => Key);
            }
        }

        public int Width
        {
            get { return _width; }
            set
            {
                _width = value;
                RaisePropertyChanged(() => Height);
            }
        }

        public int Height
        {
            get { return _height; }
            set
            {
                _height = value;
                RaisePropertyChanged(() => Height);
            }
        }

        public bool Solved
        {
            get { return _solved; }
            set
            {
                if (_solved == value)
                {
                    return;
                }

                _solved = value;

                RaisePropertyChanged(() => Solved);
            }
        }

        #endregion Properties

        #region Constructor

        public CardViewModel(int key)
        {
            Key = String.Format(@"colour{0}", key);
            Height = 100;
            Width = 80;
            InitTimer();
        }

        #endregion Constructor

        #region Public Methods

        public void SetSolved()
        {
            _solvedTimer.Start();
        }

        #endregion Public Methods

        #region Private Methods

        private void InitTimer()
        {
            _solvedTimer = new DispatcherTimer {Interval = TimeSpan.FromMilliseconds(600)};
            _solvedTimer.Tick += (s, e) =>
            {
                _solvedTimer.Stop();
                Solved = true;
            };
        }

        #endregion Private Methods
    }
}