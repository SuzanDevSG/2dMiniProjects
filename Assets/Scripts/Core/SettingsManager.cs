using System;
using UnityEngine;
public enum GameMode { PvP, PvC, RapidFire, FrozenDefense}
public class SettingsManager : MonoBehaviour
{
    public static SettingsManager Instance { get; private set; }
    private int gridSize = 3;
    private GameMode currentGameMode = GameMode.PvP;

    public int GridSize { get { return gridSize; } }
    public GameMode CurrentGameMode { get { return currentGameMode; } }

    public event Action<int> OnGridSizeChanged;
    public event Action<GameMode> OnGameModeChanged;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void SetGridSize(int size)
    {
        gridSize = size;
        OnGridSizeChanged.Invoke(gridSize);
    }

    public void SetGameMode(GameMode gameMode)
    {
        currentGameMode = gameMode;
        OnGameModeChanged?.Invoke(currentGameMode);
    }
}
