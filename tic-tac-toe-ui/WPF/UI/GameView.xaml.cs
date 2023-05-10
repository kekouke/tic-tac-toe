using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Input;
using System.Windows.Media.Animation;
using tic_tac_toe_ui.Core;
using tic_tac_toe_ui.WPF.Shapes;
using tic_tac_toe_ui.WPF.Animation;


namespace tic_tac_toe_ui.WPF.UI
{
    /// <summary>
    /// Логика взаимодействия для GameView.xaml
    /// </summary>
    public partial class GameView : Page
    {
        private Canvas gameBoard;
        
        private TicTacToe game;
        
        private AnimatorList animators;

        private readonly double GRID_SIZE = 330;
        private readonly double CELL_SIZE = 110;
        private readonly int ROW_COUNT = 3;
        private readonly int COLUMN_COUNT = 3;

        public GameView(GameMode gameMode)
        {
            InitializeComponent();

            game = new TicTacToe();
            animators = new AnimatorList();
            gameBoard = new Canvas() {
                Width = GRID_SIZE,
                Height = GRID_SIZE,
                Background = Brushes.Transparent,
            };

            gameBoard.MouseLeftButtonDown += OnMouseLeftButtonDown;
            Root.Children.Add(gameBoard);

            game.Updated += DrawFigure;
            game.Restart(gameMode);

            DrawBoard();
        }

        private void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (IsAnimationRunning()) {
                return;
            }
            Update(e);
        }

        private bool IsAnimationRunning()
        {
            return !animators.IsEmpty;
        }

        private void Update(MouseButtonEventArgs e)
        {
            var point = e.GetPosition(gameBoard);
            int x = (int)(point.X / CELL_SIZE);
            int y = (int)(point.Y / CELL_SIZE);
            game.Update(y * COLUMN_COUNT + x + 1);
            if (game.IsGameOver() && IsAnimationRunning()) {
                animators.AnimationsCompleted += (s, e) => ShowGameOverDialog();
            }
        }

        private void ShowGameOverDialog()
        {
            string winnerMark = game.GetWinner();
            string message = winnerMark == "DRAW" ? "DRAW!" :
                $"GAME_OVER. {winnerMark} -- win!";
            if (MessageBox.Show(message) == MessageBoxResult.OK)
            {
                NavigationService.Navigate(new WelcomeView());
            }
        }

        private void DrawFigure(CellState cellState, int cellNumber)
        {
            int x = (cellNumber - 1) % ROW_COUNT;
            int y = (cellNumber - 1) / COLUMN_COUNT;
            var center = new Point(CELL_SIZE * (0.5 + x), CELL_SIZE * (0.5 + y));
            Shape figure;
            DependencyProperty animatedProperty;
            AnimationTimeline animation;
            switch (cellState)
            {
                case CellState.FillX:
                    figure = FigureProvider.CreateCross(center);
                    animatedProperty = Cross.StrokeProgressProperty;
                    animation = new DoubleAnimation(0, 1, TimeSpan.FromSeconds(.4)); 
                    break;
                case CellState.FillO:
                    figure = FigureProvider.CreateCircle(center);
                    animatedProperty = Circle.AngleProperty;
                    animation = new DoubleAnimation(0, 360, TimeSpan.FromSeconds(.4));
                    break;
                default:
                    return;
            }
            gameBoard.Children.Add(figure);
            animators.Enqueue(new Animator(figure, animatedProperty, animation));
        }

        private void DrawBoard()
        {
            for (int i = 1; i <= 2; i++)
            {
                gameBoard.Children.Add(
                    new Line()
                    {
                        X1 = i * CELL_SIZE,
                        Y1 = 0,
                        X2 = i * CELL_SIZE,
                        Y2 = gameBoard.Height,
                        Stroke = Brushes.Black,
                        StrokeThickness = 3,
                    }
                );
                gameBoard.Children.Add(
                    new Line()
                    {
                        X1 = 0,
                        Y1 = i * CELL_SIZE,
                        X2 = gameBoard.Width,
                        Y2 = i * CELL_SIZE,
                        Stroke = Brushes.Black,
                        StrokeThickness = 3,
                    }
                );
            }
        }
    }
}
