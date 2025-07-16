using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class CellController : MonoBehaviour
{
    [SerializeField] private Button cellButton;
    [SerializeField] private TMP_Text cellText;
    public bool IsMarked = false;
    private PlayerType MarkedBy;

    public int row { get; private set; }
    public int col { get; private set; }
    public void InitializeRowCol(int row, int col)
    {
        this.row = row;
        this.col = col;
    }
    private void Start()
    {
        cellButton.onClick.AddListener(CellClicked);
    }
    private void OnDestroy()
    {
        cellButton.onClick.RemoveListener(CellClicked);
    }
    public void AiMove()
    {
        if (IsMarked) return; 

        IsMarked = true;
        MarkedBy = PlayerType.O;
        cellText.text = MarkedBy.ToString();

        TurnManager.Instance.DecreaseTurnCount();
        TurnManager.Instance.ChangeTurn();
    }
    public void CellClicked()
    {
        //Debug.Log($"button ({row} and {col} clicked ");
        // check if it is already clicked
        if (IsMarked) return;
        int gridSize = SettingsManager.Instance.GridSize;

        // cell clicked change the properties
        float cellSize = transform.GetComponent<RectTransform>().rect.width;
        cellText.fontSize = cellSize;

        PlayerType currentPlayer = TurnManager.Instance.CurrentPlayer;
        MarkedBy = currentPlayer;

        transform.GetComponent<Image>().enabled = false;
        cellText.text = currentPlayer.ToString();

        cellButton.enabled = false;
        IsMarked = true;
        TurnManager.Instance.DecreaseTurnCount();

        if (WinChecker.Instance.CheckWinningState(currentPlayer, row, col))
        {
            GameOverManager.Instance.ShowResult($" PLayer {currentPlayer} : Wins :) ");
        }

        else if (TurnManager.Instance.turnCounter == 0)
        {
            GameOverManager.Instance.ShowResult($" Draw :( ");
        }
        else
        {
            TurnManager.Instance.ChangeTurn();
        }

    }
    public bool IsMarkedBy(PlayerType player)
    {
        return IsMarked == true && MarkedBy == player;
    }
}
