using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using Unity.Mathematics;
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

    public CellController GetBestMove(List<CellController> cells)
    {
        CellController bestCell = null;
        int bestScore = int.MinValue;

        foreach (CellController cell in cells)
        {
            int score = CellEvaluation(cell);
            Debug.Log($"cell[{cell.row} , {cell.col}] : {score}");
            if (score > bestScore)
            {
                bestScore = score;
                bestCell = cell;
            }
        }
        return bestCell;
    }

    private int CellEvaluation(CellController cell)
    {
        int score = 0;

        int row = cell.row;
        int col = cell.col;

        score += CheckStreak(PlayerType.O, row, col) * 25;
        score += CheckStreak(PlayerType.X, row, col) * 20;

        score += DetectFork(PlayerType.O, row, col);
        score += DetectFork(PlayerType.X, row, col);



        if (IsCenter(row, col))
        {
            score += 20;
        }
        if (IsCorner(row, col))
        {
            score += 15;
        }
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
    private int CheckStreak(PlayerType player, int row, int col)
    {
        int score = 0;

        int rowScore = CheckRowStreak(player, row, col);

        Debug.Log("RowScore : " + rowScore);

        int colScore = CheckColStreak(player, row, col);
        Debug.Log("ColScore : " + colScore);

        int diagScore = CheckDiagonalStreak(player, row, col);
        Debug.Log("DiagScore : " + diagScore);

        int antiDragScore = CheckAntiDiagonalStreak(player, row, col);
        Debug.Log("AntiDragScore : " + antiDragScore);

        score += rowScore + colScore + diagScore + antiDragScore;
        //score += CheckDirection(player, row, col, 1, 0);
        //score += CheckDirection(player, row, col, -1, 0);
        //score += CheckDirection(player, row, col, 0, 1);
        //score += CheckDirection(player, row, col, 0, -1);
        //score += CheckDirection(player, row, col, 1, 1);
        //score += CheckDirection(player, row, col, -1, -1);
        //score += CheckDirection(player, row, col, -1, +1);
        //score += CheckDirection(player, row, col, +1, -1);

        return score;
    }

    private int CheckAntiDiagonalStreak(PlayerType player, int row, int col)
    {
        int streak = 1;

        for (int i = 1; (row - i >= 0 && col + i < gridSize); i++)
        {
            if (gridCells[row - 1, col + 1].IsMarkedBy(player))
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
        if (streak >= 3) return 6;
        if (streak == 2) return 2;
        if (streak == 1) return 1;
        return 0;
    }

    private int CheckDiagonalStreak(PlayerType player, int row, int col)
    {
        int streak = 1;

        // forward streak
        for (int i = 1; (row + i < gridSize && col + i < gridSize); i++)
        {
            if (gridCells[row + 1, col + i].IsMarkedBy(player))
            {
                streak++;
            }
            else break;
        }
        // backward streak
        for (int i = 1; (col - i >= 0 && row + i >= 0); i++)
        {
            if (gridCells[row - i, col - i].IsMarkedBy(player))
            {
                streak++;
            }
            else break;
        }
        if (streak >= 3) return 6;
        if (streak == 2) return 2;
        if (streak == 1) return 1;
        return 0;
    }

    private int CheckColStreak(PlayerType player, int row, int col)
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
        if (streak >= 3) return 6;
        if (streak == 2) return 2;
        if (streak == 1) return 1;
        return 0;
    }

    private int CheckRowStreak(PlayerType player, int row, int col)
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
        if (streak >= 3) return 6;
        if (streak == 2) return 2;
        if (streak == 1) return 1;
        return 0;

    }

    private int DetectFork(PlayerType player, int row, int col)
    {
        int threats = 0;

        if ((CheckDirection(player, row, col, 0, 1) + CheckDirection(player, row, col, 0, -1) >= 2))
        {
            threats++;
        }
        if ((CheckDirection(player, row, col, 1, 0) + CheckDirection(player, row, col, -1, 0)) >= 2)
        {
            threats++;
        }
        if ((CheckDirection(player, row, col, 1, 1) + CheckDirection(player, row, col, -1, -1) >= 2))
        {
            threats++;
        }
        if ((CheckDirection(player, row, col, 1, -1) + CheckDirection(player, row, col, -1, 1) >= 2))
        {
            threats++;
        }

        if (threats >= 2)
        {
            return 100;
        }
        return 0;
    }
    private int CheckDirection(PlayerType player, int row, int col, int rowStep, int colStep)
    {
        int streak = 0;

        for (int i = 1; i < winCount; i++)
        {
            int r = row + i * rowStep;
            int c = col + i * colStep;

            if (r < 0 || c < 0 || r >= gridSize || c >= gridSize) break;

            if (gridCells[r, c].IsMarkedBy(player))
            {
                streak++;
            }
            //else
            //{
            //    break;
            //}
        }
        if (streak >= 3) return 150;
        if (streak == 2) return 50;
        if (streak == 1) return 10;
        return 0;
    }
}