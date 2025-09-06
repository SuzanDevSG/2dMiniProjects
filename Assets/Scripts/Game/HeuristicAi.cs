using UnityEngine;
public class HeuristicAi : MonoBehaviour
{
    public static HeuristicAi Instance { get; private set; }

    private CellController[,] gridCells;
    private int gridSize;
    private int winCount;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    public void SetValues(int size, int count, CellController[,] cells)
    {
        gridSize = size;
        winCount = count;
        gridCells = cells;
    }
    public int CellEvaluation(CellController cell)
    {
        int row = cell.row;
        int col = cell.col;

        cell.SetTemporaryMark(PlayerType.O);
        int aiRow = CheckRowStreak(PlayerType.O, row, col);
        int aiCol = CheckColStreak(PlayerType.O, row, col);
        int aiDiag = CheckDiagonalStreak(PlayerType.O, row, col);
        int aiAnti = CheckAntiDiagonalStreak(PlayerType.O, row, col);
        cell.ClearMark();

        cell.SetTemporaryMark(PlayerType.X);
        int opRow = CheckRowStreak(PlayerType.X, row, col);
        int opCol = CheckColStreak(PlayerType.X, row, col);
        int opDiag = CheckDiagonalStreak(PlayerType.X, row, col);
        int opAnti = CheckAntiDiagonalStreak(PlayerType.X, row, col);
        cell.ClearMark();

        int aiScore = Mathf.Max(aiRow, aiCol, aiDiag, aiAnti);
        int opScore = Mathf.Max(opRow, opCol, opDiag, opAnti);
        int score = aiScore - opScore;

        if (IsCenter(row, col)) score += 10; // Strongly prefer center
        if (IsCorner(row, col)) score += 1;
        return score;
    }
    private bool IsCenter(int row, int col)
    {
        if (gridSize % 2 == 1)
        {
            return gridSize / 2 == row && gridSize / 2 == col;
        }
        else
        {
            int mid = gridSize / 2;
            return (mid == row || mid - 1 == row) && (mid == col || mid - 1 == col);
        }
    }
    private bool IsCorner(int row, int col)
    {
        return (row == 0 || row == gridSize - 1) && (col == 0 || col == gridSize - 1);
    }

    private int CheckAntiDiagonalStreak(PlayerType player, int row, int col)
    {
        int streak = 1;

        for (int i = 1; (row - i >= 0 && col + i < gridSize); i++)
        {
            if (gridCells[row - i, col + i].IsMarkedBy(player))
            {
                streak++;
            }
            else break;
        }

        for (int i = 1; (row + i < gridSize && col - i >= 0); i++)
        {
            if (gridCells[row + i, col - i].IsMarkedBy(player))
            {
                streak++;
            }
            else break;
        }
        if (streak >= winCount) return 10;
        if (streak == winCount - 1) return 5;
        if (streak == winCount - 2) return 1;
        return 0;
    }

    private int CheckDiagonalStreak(PlayerType player, int row, int col)
    {
        int streak = 1;

        // forward streak
        for (int i = 1; (row + i < gridSize && col + i < gridSize); i++)
        {
            if (gridCells[row + i, col + i].IsMarkedBy(player))
            {
                streak++;
            }
            else break;
        }
        // backward streak
        for (int i = 1; (col - i >= 0 && row - i >= 0); i++)
        {
            if (gridCells[row - i, col - i].IsMarkedBy(player))
            {
                streak++;
            }
            else break;
        }
        if (streak >= winCount) return 10;
        if (streak == winCount - 1) return 5;
        if (streak == winCount - 2) return 1;
        return 0;
    }

    private int CheckRowStreak(PlayerType player, int row, int col)
    {
        int streak = 1;
        for (int c = col - 1; c >= 0; c--)
        {
            if (gridCells[row, c].IsMarkedBy(player))
            {
                streak++;
            }
            else break;
        }
        for (int c = col + 1; c < gridSize; c++)
        {
            if (gridCells[row, c].IsMarkedBy(player))
            {
                streak++;
            }
            else break;
        }
        if (streak >= winCount) return 10;
        if (streak == winCount - 1) return 5;
        if (streak == winCount - 2) return 1;
        return 0;
    }

    private int CheckColStreak(PlayerType player, int row, int col)
    {
        int streak = 1;
        for (int r = row - 1; r >= 0; r--)
        {
            if (gridCells[r, col].IsMarkedBy(player))
            {
                streak++;
            }
            else break;
        }
        for (int r = row + 1; r < gridSize; r++)
        {
            if (gridCells[r, col].IsMarkedBy(player))
            {
                streak++;
            }
            else break;
        }
        if (streak >= winCount) return 10;
        if (streak == winCount - 1) return 5;
        if (streak == winCount - 2) return 1;
        return 0;

    }

    public int BoardEvaluation()
    {
        int totalScore = 0;
        foreach (var cell in gridCells)
        {
            if (!cell.IsMarked)
            {
                totalScore += CellEvaluation(cell);
            }
        }
        return totalScore;
    }
}