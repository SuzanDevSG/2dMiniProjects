using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks; // Add for Task
using System;

public class AiController : MonoBehaviour
{
    public static AiController Instance { get; private set; }
    public static bool IsAiThinking { get; private set; } // Add flag
    private Coroutine AiPlay;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void AiTurn()
    {
        // Start async AI move
        PlayAsync();
    }

    private async void PlayAsync()
    {
        List<CellController> unmarkedCells = GridManager.Instance.GetAllUnMarkedCells();
        if (unmarkedCells.Count == 0) return;
        Debug.Log("Getting Best Move");

        IsAiThinking = true; // Block player input
        await Task.Delay(500); // Wait for 0.5s

        // Run minimax in background
        CellController chosenCell = await Task.Run(() => MiniMaxAI.Instance.GetBestMove(unmarkedCells));

        IsAiThinking = false; // Allow player input
        // Apply move on main thread
        if (chosenCell != null)
        {
            chosenCell.CellClicked();
        }
    }
}
