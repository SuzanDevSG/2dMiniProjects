using System.Collections.Generic;
using UnityEngine;
using System.Text; // For board state hashing
public class MiniMaxAI : MonoBehaviour
{
    public static MiniMaxAI Instance { get; private set; }
    private CellController[,] gridCells;
    private PlayerType aiPlayer = PlayerType.O;
    private PlayerType opponent = PlayerType.X;

    [SerializeField] private int depthLimit = 4; 

    // Transposition table for caching board states
    private Dictionary<string, int> transpositionTable = new();

    private void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
    }

    public void SetValues(CellController[,] cells)
    {
        gridCells = cells;
    }

    // Iterative Deepening
    public CellController GetBestMove(List<CellController> unMarkedCells)
    {

        int bestScore = int.MinValue;
        CellController bestMove = null;
        for (int currentDepth = 1; currentDepth <= depthLimit; currentDepth++)
        {
            foreach (CellController cell in unMarkedCells)
            {
                cell.SetTemporaryMark(aiPlayer);
                var nextAvailableCells = new List<CellController>(unMarkedCells);
                nextAvailableCells.Remove(cell);
                int score = AlphaBetaMiniMax(0, aiPlayer, nextAvailableCells, cell, int.MinValue, int.MaxValue, currentDepth);
                cell.ClearMark();
                // Prefer moves that block opponent win/fork
                if (score > bestScore || bestMove == null)
                {
                    bestScore = score;
                    bestMove = cell;
                }
            }
        }
        return bestMove;
    }

    private int AlphaBetaMiniMax(int depth, PlayerType currentPlayer, List<CellController> availableCells, CellController lastCell, int alpha, int beta, int maxDepth)
    {
        // Board state key for caching
        string boardKey = GetBoardKey();
        if (transpositionTable.TryGetValue(boardKey, out int cachedScore))
        {
            return cachedScore;
        }

        if (WinChecker.Instance.CheckWinningState(currentPlayer, lastCell.row, lastCell.col))
        {
            int winScore = currentPlayer == aiPlayer ? 100 - depth : -100 + depth;
            transpositionTable[boardKey] = winScore;
            return winScore;
        }

        if (IsDraw() || availableCells.Count == 0)
        {
            transpositionTable[boardKey] = 0;
            return 0;
        }

        if (depth >= maxDepth)
        {
            int score = HeuristicAi.Instance.BoardEvaluation();
            transpositionTable[boardKey] = score;
            return score;
        }

        currentPlayer = (currentPlayer == aiPlayer) ? opponent : aiPlayer;
        int bestScore = (currentPlayer == aiPlayer) ? int.MinValue : int.MaxValue;
        availableCells.Sort((a, b) =>
            HeuristicAi.Instance.CellEvaluation(b).CompareTo(HeuristicAi.Instance.CellEvaluation(a)));

        foreach (CellController cell in availableCells)
        {
            cell.SetTemporaryMark(currentPlayer);
            var nextAvailableCells = new List<CellController>(availableCells);
            nextAvailableCells.Remove(cell);
            int score = AlphaBetaMiniMax(depth + 1, currentPlayer, nextAvailableCells, cell, alpha, beta, maxDepth);
            cell.ClearMark();
            if (currentPlayer == aiPlayer)
            {
                bestScore = Mathf.Max(bestScore, score);
                alpha = Mathf.Max(alpha, score);
            }
            else
            {
                bestScore = Mathf.Min(bestScore, score);
                beta = Mathf.Min(beta, score);
            }
            if (beta <= alpha)
            {
                break; // Alpha-beta pruning
            }
        }
        transpositionTable[boardKey] = bestScore;
        return bestScore;
    }

    // Board state as string for caching
    private string GetBoardKey()
    {
        StringBuilder sb = new StringBuilder();
        foreach (var cell in gridCells)
        {
            sb.Append(cell.MarkedBy);
        }
        return sb.ToString();
    }

    private bool IsDraw()
    {
        foreach (CellController cell in gridCells)
        {
            if (!cell.IsMarked) return false;
        }
        return true;
    }
}