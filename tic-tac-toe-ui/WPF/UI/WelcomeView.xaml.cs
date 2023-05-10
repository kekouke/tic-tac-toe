using System.Windows;
using System.Windows.Controls;
using tic_tac_toe_ui.Core;

namespace tic_tac_toe_ui.WPF.UI
{
    /// <summary>
    /// Логика взаимодействия для WelcomeView.xaml
    /// </summary>
    public partial class WelcomeView : Page
    {

        public WelcomeView()
        {
            InitializeComponent();
        }

        private void OnSelectBotMode(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new GameView(GameMode.AI));
        }

        private void OnSelectFriendMode(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new GameView(GameMode.TwoPlayer));
        }
    }
}
