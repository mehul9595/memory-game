using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ColorGame.SL.ViewModel;

namespace ColorGame.SL.View
{
    public partial class ColorGame : UserControl
    {
        private CardGameViewModel _cardGameViewModel;

        public ColorGame()
        {
            InitializeComponent();
            DataContext = CardGameViewModel;
        }

        public CardGameViewModel CardGameViewModel
        {
            get { return _cardGameViewModel ?? (_cardGameViewModel = new CardGameViewModel()); }
        }

        private void CardsListBoxKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                var lastCard = ((ListBox)sender).SelectedItem as CardViewModel;
                var cardVm = (DataContext as CardGameViewModel);

                if (cardVm == null || lastCard == null || rectOverlay.Visibility == Visibility.Visible || lastCard.Solved)
                    return;

                cardVm.ChangeCardSelection(lastCard);
                e.Handled = true;
            }
        }
    }
}