using System;
using tic_tac_toe_ui.Core;

namespace tic_tac_toe_ui.CMD.UI
{
    public class ConsoleWindow
    {
        private TicTacToe game;

        public ConsoleWindow()
        {
            game = new TicTacToe();
            game.Updated += (s, n) => DrawGameBoard();
        }

        public void Run()
        {
            ShowWelcomeDialog();
            DrawGameBoard();
            StartGameLoop();
            ShowGameOverDialog();
        }

        private void ShowWelcomeDialog()
        {
            Console.WriteLine("Добро пожаловать в игру \"Крестики-нолики\".");
            Console.WriteLine("Для игры с ботом введите: BOT, для игры с другом: FRIEND");

            const string BOT = "BOT";
            const string FRIEND = "FRIEND";

            string command = string.Empty;
            while (command != BOT && command != FRIEND)
            {
                command = Console.ReadLine();
                game.Restart(command == BOT ? GameMode.AI : GameMode.TwoPlayer);
            }
        }

        private void StartGameLoop()
        {
            while (!game.IsGameOver())
            {
                game.Update(GetCellNumberFromInput());
            }
        }

        private int GetCellNumberFromInput()
        {
            int cellNumber;
            while (!int.TryParse(Console.ReadLine(), out cellNumber));
            return cellNumber;
        }

        private void DrawGameBoard()
        {
            const int rowCount = 3;
            const int columnCount = 3;

            Console.Clear();

            for (int i = 0; i < columnCount; i++)
            {
                bool isFirst = true;
                for (int j = 1; j <= rowCount; j++)
                {
                    if (!isFirst)
                    {
                        Console.Write('|');
                    }
                    int cellNumber = i * columnCount + j;
                    var cellState = game.GetCell(cellNumber);
                    switch (cellState)
                    {
                        case CellState.NoFill:
                            Console.Write(cellNumber);
                            break;
                        case CellState.FillX:
                            Console.Write('x');
                            break;
                        case CellState.FillO:
                            Console.Write('o');
                            break;
                    }
                    isFirst = false;
                }
                Console.WriteLine();
            }
        }
      
        private void ShowGameOverDialog()
        {
            string winnerMark = game.GetWinner();
            string message = winnerMark == "DRAW" ? "DRAW!" :
                $"GAME_OVER. {winnerMark} -- win!";
            Console.WriteLine(message);
        }
    }
}
