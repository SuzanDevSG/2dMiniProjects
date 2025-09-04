using UnityEngine;
public enum PlayerType { None, X, O }
public class TurnManager : MonoBehaviour
{
    public static TurnManager Instance { get; private set; }
    public PlayerType CurrentPlayer { get; private set; } = PlayerType.X;
    [SerializeField] public int turnCounter { get; private set; }
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    public void SetGridSize(int gridSize)
    {
        turnCounter = gridSize * gridSize;
    }
    public void DecreaseTurnCount()
    {
        turnCounter--;
    }
    public void ChangeTurn()
    {
        
        CurrentPlayer = (CurrentPlayer == PlayerType.X) ? PlayerType.O : PlayerType.X;

        if (SettingsManager.Instance.CurrentGameMode == GameMode.PvC && CurrentPlayer == PlayerType.O)
        {
            AiController.Instance.AiTurn();
        }
    }
}