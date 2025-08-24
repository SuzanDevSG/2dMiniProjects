using System.Collections.Generic;
using UnityEngine;
public class MiniMaxAI : MonoBehaviour
{
    public static MiniMaxAI Instance { get; private set; }
    private CellController[,] gridCells;
    private PlayerType aiPlayer = PlayerType.O;
    private PlayerType opponent = PlayerType.X;

    [SerializeField] private int depthLimit = 2; 

    
    public MinimaxTreeNode LastTreeRoot { get; private set; }

    private void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
    }

    public void SetValues(CellController[,] cells)
    {
        gridCells = cells;
    }
    public CellController GetBestMove(List<CellController> unMarkedCells)
    {
        int bestScore = int.MinValue;
        CellController bestMove = null;

        var root = new MinimaxTreeNode("Root", 0, 0);
        MinimaxTreeNode.LastTreeRoot = root;

        foreach (CellController cell in unMarkedCells)
        {
            cell.SetTemporaryMark(aiPlayer);

            var nextAvailableCells = new List<CellController>(unMarkedCells);
            nextAvailableCells.Remove(cell);

            int score = AlphaBetaMiniMax(0, aiPlayer, nextAvailableCells, cell, int.MinValue, int.MaxValue, "", root);
            cell.ClearMark();

            if (score > bestScore)
            {
                bestScore = score;
                bestMove = cell;
            }
        }

        return bestMove;
    }

    private int AlphaBetaMiniMax(int depth, PlayerType currentPlayer, List<CellController> availableCells, CellController lastCell, int alpha, int beta, string logPrefix, MinimaxTreeNode parentNode)
    {
        // Create a node for this call
        string label = lastCell != null ? $"{currentPlayer} ({lastCell.row},{lastCell.col})" : "Start";
        var node = new MinimaxTreeNode(label, depth, 0, parentNode);
        parentNode.Children.Add(node);

        // Check if the last move resulted in a win
        if (WinChecker.Instance.CheckWinningState(currentPlayer, lastCell.row, lastCell.col))
        {
            int winScore = currentPlayer == aiPlayer ? 100 - depth : -100 + depth;
            node.Score = winScore; 
            return winScore;
        }

        if (IsDraw() || availableCells.Count == 0)
        {
            node.Score = 0; 
            return 0;
        }

        if (depth >= depthLimit)
        {
            int score = HeuristicAi.Instance.BoardEvaluation();
            node.Score = score; 
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

            int score = AlphaBetaMiniMax(depth + 1, currentPlayer, nextAvailableCells, cell, alpha, beta, logPrefix + " ", node);
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
        node.Score = bestScore;
        return bestScore;
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