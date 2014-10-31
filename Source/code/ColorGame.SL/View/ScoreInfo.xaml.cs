using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
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
            this.DataContext = null;
            this.Close();
        }
    }
}

