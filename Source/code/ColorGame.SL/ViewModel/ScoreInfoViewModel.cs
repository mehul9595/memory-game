using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using ColorGame.SL.GameScoreService;
using ColorGame.SL.Helpers;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.ViewModel;

namespace ColorGame.SL.ViewModel
{
    public class ScoreInfoViewModel : NotificationObject
    {
        private readonly UserScore _scoreModel;
        private string _email;
        private bool _isGridCollased, _isSubmitScoreCollapsed;
        private string _name;
        private bool _useDatabase;

        public ScoreInfoViewModel(int score)
        {
            _scoreModel = new UserScore();
            UserScores = new ObservableCollection<UserScore>();

            //remove if any existing subscriptions & subscribe
            ServiceHelperProxy.GameScoreServiceClient.GetTopRankersWithCurrentUserCompleted -=
                (ColorGameServiceGetTopRankersWithCurrentUserCompleted);
            ServiceHelperProxy.GameScoreServiceClient.GetTopRankersWithCurrentUserCompleted +=
                (ColorGameServiceGetTopRankersWithCurrentUserCompleted);

            ServiceHelperProxy.GameScoreServiceClient.SaveCompleted -= (ColorGameServiceSaveCompleted);
            ServiceHelperProxy.GameScoreServiceClient.SaveCompleted += (ColorGameServiceSaveCompleted);

            _scoreModel.Score = score;
            IsSubmitScoreCollapsed = false;
            IsGridCollapsed = true;

            SubmitDelegate = new DelegateCommand(OnSubmitDelegate, CanExecute);
            SubmitDelegate.RaiseCanExecuteChanged();
        }
        
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                RaisePropertyChanged(() => Name);
                SubmitDelegate.RaiseCanExecuteChanged();
            }
        }

        public string Email
        {
            get { return _email; }
            set
            {
                _email = value;
                RaisePropertyChanged(() => Email);
                SubmitDelegate.RaiseCanExecuteChanged();
            }
        }

        public bool UseDatabase
        {
            get { return _useDatabase; }
            set
            {
                _useDatabase = value;
                RaisePropertyChanged(() => UseDatabase);
            }
        }

        public DelegateCommand SubmitDelegate { get; set; }

        public bool IsGridCollapsed
        {
            get { return _isGridCollased; }
            set
            {
                _isGridCollased = value;
                RaisePropertyChanged(() => IsGridCollapsed);
            }
        }

        public bool IsSubmitScoreCollapsed
        {
            get { return _isSubmitScoreCollapsed; }
            set
            {
                _isSubmitScoreCollapsed = value;
                RaisePropertyChanged(() => IsSubmitScoreCollapsed);
            }
        }

        public ObservableCollection<UserScore> UserScores { get; set; }

        private void ColorGameServiceSaveCompleted(object sender, AsyncCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                MessageBox.Show(e.Error.Message);
                IsGridCollapsed = true;
                IsSubmitScoreCollapsed = false;
            }
            else
            {
                ServiceHelperProxy.GameScoreServiceClient.GetTopRankersWithCurrentUserAsync(_scoreModel);
            }
        }

        private void ColorGameServiceGetTopRankersWithCurrentUserCompleted(object sender,
            GetTopRankersWithCurrentUserCompletedEventArgs e)
        {
            if (e.Error == null || string.IsNullOrEmpty(e.Error.Message))
            {
                UserScores.Clear();
                foreach (UserScore userScore in e.Result)
                {
                    UserScores.Add(userScore);
                }

                IsGridCollapsed = false;
                IsSubmitScoreCollapsed = true;
            }
            else
            {
                MessageBox.Show(e.Error.Message);
                IsGridCollapsed = true;
                IsSubmitScoreCollapsed = false;
            }
        }


        private bool CanExecute()
        {
            return !string.IsNullOrEmpty(Name) && !string.IsNullOrEmpty(Email);
        }

        private void OnSubmitDelegate()
        {
            _scoreModel.Email = Email;
            _scoreModel.Name = Name;

            if (UseDatabase)
            {
                SaveModel(_scoreModel);
            }
            else
            {
                var userScores = new List<UserScore>();
                userScores.Add(new UserScore {Email = "max@hotmail.com", Name = "Max", Score = 2});
                userScores.Add(new UserScore {Email = "charlie@yahoo.com", Name = "Charlie", Score = 5});
                userScores.Add(new UserScore {Email = "gomes@gomes.com", Name = "Gomes", Score = 4});
                userScores.Add(new UserScore {Email = "eric@butterfield.com", Name = "Eric", Score = 2});
                userScores.Add(new UserScore {Email = "steve@jobs.com", Name = "Steve", Score = -3});
                userScores.Add(_scoreModel);
                userScores.OrderByDescending(r => r.Score).ToList();
                int counter = 1;
                userScores.ForEach(r => r.Rank = counter++);
                foreach (UserScore item in userScores)
                {
                    UserScores.Add(item);
                }

                IsGridCollapsed = false;
                IsSubmitScoreCollapsed = true;
            }
        }
        
        public void SaveModel(UserScore userScoreModel)
        {
            ServiceHelperProxy.GameScoreServiceClient.SaveAsync(userScoreModel);
        }
    }
}