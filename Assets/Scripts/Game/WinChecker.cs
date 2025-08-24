using UnityEngine;
public class WinChecker : MonoBehaviour
{
    public static WinChecker Instance { get; private set; }
    private CellController[,] gridCells;
    private int gridSize, winCount;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        Instance = this;
    }

    public int GetWinCount(int size)
    {
        if (size <= 3) return size;
        if (size <= 6) return 4;
        else return 5;
    }
    public void SetGridCells(CellController[,] grid, int gridSize)
    {
        gridCells = grid;
        this.gridSize = gridSize;
        winCount = GetWinCount(gridSize);
    }
    public bool CheckWinningState(PlayerType player, int row, int col)
    {
        //Debug.Log("checking winstate");
        bool HasWon = CheckRow(player, row) || CheckCol(player, col) || CheckRightDiagonal(player, row, col) || CheckleftDiagonal(player, row, col);
        return HasWon;
    }

    private bool CheckRow(PlayerType player, int row)
    {
        int count = 0;
        for (int col = 0; col < gridSize; col++)
        {
            if (gridCells[row, col].IsMarkedBy(player))
            {
                count++;
                if (count >= winCount)
                    return true;
            }
            else
            {
                count = 0;

            }
        }
        return false;
    }
    private bool CheckCol(PlayerType player, int col)
    {
        int count = 0;
        for (int row = 0; row < gridSize; row++)
        {
            if (gridCells[row, col].IsMarkedBy(player))
            {
                count++;
                if (count >= winCount)
                    return true;
            }
            else
            {
                count = 0;
            }
        }
        return false;
    }
    private bool CheckRightDiagonal(PlayerType player, int r, int c)
    {
        int offset = Mathf.Min(r, c);
        int row = r - offset;
        int col = c - offset;
        int count = 0;
        for (int i = 0; (row + i < gridSize && col + i < gridSize); i++)
        {
            //Debug.Log($"checking {row + i}, {col + i}");

            if (gridCells[row + i, col + i].IsMarkedBy(player))
                count++;
            else
            {
                count = 0;
            }
            if (count >= winCount)
            {
                return true;
            }
        }
        return false;
    }
    private bool CheckleftDiagonal(PlayerType player, int r, int c)
    {
        int offset = Mathf.Min(r, gridSize - 1 - c);
        int row = r - offset;
        int col = c + offset;
        int count = 0;
        for (int i = 0; (row + i < gridSize && col - i >= 0); i++)
        {
            if (gridCells[row + i, col - i].IsMarkedBy(player))
                count++;
            else
            {
                count = 0;
            }
            if (count >= winCount)
            {
                return true;
            }
        }
        return false;
    }

    public bool CanWinInLine(PlayerType player, int startRow, int startCol, int stepRow, int stepCol, int length)
    {
        int streak = 0;
        int r = startRow, c = startCol;

        for (int i = 0; i < length; i++, r += stepRow, c += stepCol)
        {
            // skip out-of-bounds
            if (r < 0 || r >= gridSize || c < 0 || c >= gridSize)
                return false;

            if (gridCells[r, c].IsMarkedBy(player) || !gridCells[r, c].IsMarked) // treat empty as potential
            {
                streak++;
            }
            else return false;
        }
        return streak == length;
    }

    // THe Below methods checks all the cells for any possibility of row,col,diagonal match

    //private bool CheckRow(PlayerType player)
    //{
    //    for (int row = 0; row < gridSize; row++)
    //    {
    //        count = 0;
    //        for (int col = 0; col < gridSize; col++)
    //        {
    //            if (gridCells[row, col].IsMarkedBy(player))
    //            {
    //                count++;
    //                if (count >= winCount)
    //                    return true;
    //            }
    //            else
    //            {
    //                count = 0;
    //            }
    //        }
    //    }
    //    return false;
    //}
    //private bool CheckCol(PlayerType player)
    //{
    //    for (int col = 0; col < gridSize; col++)
    //    {
    //        count = 0;
    //        for (int row = 0; row < gridSize; row++)
    //        {
    //            if (gridCells[row, col].IsMarkedBy(player))
    //            {
    //                count++;
    //                if (count >= winCount)
    //                    return true;
    //            }
    //            else
    //            {
    //                count = 0;
    //            }
    //        }
    //    }
    //    return false;
    //}
    //private bool CheckRightDiagonal(PlayerType player)
    //{
    //    for (int row = 0; row <= gridSize - winCount; row++)
    //    {
    //        for (int col = 0; col <= gridSize - winCount; col++)
    //        {
    //            count = 0;
    //            for (int offset = 0; offset < winCount; offset++)
    //            {

    //                if (gridCells[row + offset, col + offset].IsMarkedBy(player))
    //                    count++;
    //                else break;
    //            }
    //            if (count >= winCount)
    //            {
    //                return true;
    //            }
    //            //debugs
    //            //Debug.Log($"Checking  diagonal from ({row}, {col})");
    //            //count = 0;
    //            //for (int offset = 0; offset < winCount; offset++)
    //            //{

    //            //    int r = row + offset;
    //            //    int c = col + offset;

    //            //    bool match = gridCells[r, c].IsMarkedBy(player);
    //            //    Debug.Log($"Cell[{r}, {c}] = {(match ? "Y" : "N")} for {player}");

    //            //    if (match)
    //            //        count++;
    //            //    else
    //            //        break;
    //            //}

    //            //if (count >= winCount)
    //            //{
    //            //    Debug.Log($" Diagonal WIN for {player} starting at ({row}, {col})");

    //            //}

    //        }
    //    }
    //    return false;
    //}
    //private bool CheckleftDiagonal(PlayerType player)
    //{
    //    for (int row = 0; row <= gridSize - winCount; row++)
    //    {
    //        for (int col = winCount - 1; col < gridSize - winCount; col++)
    //        {
    //            count = 0;
    //            for (int offset = 0; offset < winCount; offset++)
    //            {
    //                if (gridCells[row + offset, col - offset].IsMarkedBy(player))
    //                {
    //                    count++;
    //                    if (count >= winCount)
    //                        return true;
    //                }
    //            }

    //            Debug.Log($"Checking  diagonal from ({row}, {col})");
    //            count = 0;
    //            for (int offset = 0; offset < winCount; offset++)
    //            {

    //                int r = row + offset;
    //                int c = col - offset;

    //                bool match = gridCells[r, c].IsMarkedBy(player);
    //                Debug.Log($"Cell[{r}, {c}] = {(match ? "Y" : "N")} for {player}");

    //                if (match)
    //                    count++;
    //                else
    //                    break;
    //            }

    //            if (count >= winCount)
    //            {
    //                Debug.Log($" Diagonal WIN for {player} starting at ({row}, {col})");
    //            }
    //        }
    //    }      
    //    return false;
    //}
}
