using System;
using System.Linq;

namespace tic_tac_toe_ui.Core
{
    public enum GameMode
    {
        TwoPlayer,
        AI,
    }

    public enum ActivePlayer
    {
        First,
        Second,
    }

    public enum CellState
    {
        NoFill,
        FillX,
        FillO,
    }

    public delegate void RenderHandler(CellState cellState, int cellNumber);

    public class TicTacToe
    {
        public event RenderHandler Updated;

        private ActivePlayer activePlayer;

        private GameMode gameMode = GameMode.TwoPlayer;

        private Random cellNumberGenerator = new Random();

        private const int ROW_COUNT = 3;

        private const int COLUMN_COUNT = 3;

        private CellState[] gameBoard;

        public CellState GetCell(int cellNumber)
        {
            return gameBoard[cellNumber - 1];
        }

        public string GetWinner() // TODO
        {
            if (!IsGameOver()) {
                return "No one";
            }
            return TryGetWinner(out var mark) ? mark.ToString() : "DRAW";
        }

        public void Restart(GameMode gameMode = GameMode.TwoPlayer)
        {
            this.gameMode = gameMode;
            activePlayer = ActivePlayer.First;
            gameBoard = new CellState[ROW_COUNT * COLUMN_COUNT];
        }

        public void Update(int cellNumber)
        {
            if (!IsCellValid(cellNumber)) {
                return;
            }
            switch (activePlayer)
            {
                case ActivePlayer.First:
                    gameBoard[cellNumber - 1] = CellState.FillX;
                    Updated?.Invoke(CellState.FillX, cellNumber);
                    activePlayer = ActivePlayer.Second;
                    if (gameMode == GameMode.AI && !IsGameOver()) {
                        Update(GenerateCellNumberForDummyAI());
                    }
                    break;
                case ActivePlayer.Second:
                    gameBoard[cellNumber - 1] = CellState.FillO;
                    Updated?.Invoke(CellState.FillO, cellNumber);
                    activePlayer = ActivePlayer.First;
                    break;
            }
        }

        private bool IsCellValid(int num)
        {
            if (num < 1 || num > gameBoard.Length)
            {
                return false;
            }
            return gameBoard[num - 1] == CellState.NoFill;
        }

        public bool IsGameOver()
        {
            return IsDraw() || TryGetWinner(out var _);
        }

        private bool IsDraw()
        {
            return gameBoard.All(c => c != CellState.NoFill);
        }

        private bool TryGetWinner(out char winnerMark)
        {
            winnerMark =
                CheckLines(CellState.FillX) || CheckDiagonals(CellState.FillX) ? 'x' :
                CheckLines(CellState.FillO) || CheckDiagonals(CellState.FillO) ? 'o' :
                '0';

            return winnerMark != '0';
        }

        private bool CheckLines(CellState mark)
        {
            for (int i = 0; i < ROW_COUNT; i++)
            {
                bool vertical = true;
                bool horizontal = true;
                for (int j = 0; j < COLUMN_COUNT; j++)
                {
                    vertical = vertical &&
                        gameBoard[ConvertToOneDimension(i, j)] == mark;

                    horizontal = horizontal &&
                        gameBoard[ConvertToOneDimension(j, i)] == mark;
                }
                if (vertical || horizontal)
                {
                    return true;
                }
            }

            return false;
        }

        private int ConvertToOneDimension(int x, int y)
        {
            return COLUMN_COUNT * y + x;
        }

        private bool CheckDiagonals(CellState mark)
        {
            bool toRightDown = true;
            bool toRightUp = true;

            for (int i = 0; i < COLUMN_COUNT; i++)
            {
                toRightDown = toRightDown && gameBoard[ConvertToOneDimension(i, i)] == mark;
                toRightUp = toRightUp &&
                    gameBoard[ConvertToOneDimension(i, COLUMN_COUNT - i - 1)] == mark;
            }

            return toRightUp || toRightDown;
        }

        private int GenerateCellNumberForDummyAI()
        {
            int result = -1;
            while (!IsCellValid(result))
            {
                result = cellNumberGenerator.Next(1, gameBoard.Length + 1);
            }
            return result;
        }
    }
}
