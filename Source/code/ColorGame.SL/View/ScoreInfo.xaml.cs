using System.Windows;
using System.Windows.Controls;
using ColorGame.SL.ViewModel;

namespace ColorGame.SL.View
{
    public partial class ScoreInfo : ChildWindow
    {
        public ScoreInfo()
        {
            InitializeComponent();
        }

        public ScoreInfo(int score)
        {
            InitializeComponent();
            var scoreInfoViewModel = new ScoreInfoViewModel(score);

            DataContext = scoreInfoViewModel;
        }

        private void BtnCloseClick(object sender, RoutedEventArgs e)
        {
            DataContext = null;
            Close();
        }
    }
}