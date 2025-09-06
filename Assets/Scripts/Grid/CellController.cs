using UnityEngine;
using UnityEngine.UI;
public class CellController : MonoBehaviour
{
    private Button cellButton;
    [SerializeField] private GameObject marker;
    [SerializeField] private Sprite XSprite;
    [SerializeField] private Sprite OSprite;
    [SerializeField] private ParticleSystem particleSystem1;
    [SerializeField] private ParticleSystem particleSystem2;

    public bool IsMarked = false;
    public PlayerType MarkedBy { get; private set; }


    public int row { get; private set; }
    public int col { get; private set; }


    public void InitializeRowCol(int row, int col)
    {
        this.row = row;
        this.col = col;
    }
    private void Start()
    {
        cellButton = GetComponent<Button>();
        marker.SetActive(false);

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

        TurnManager.Instance.DecreaseTurnCount();
        TurnManager.Instance.ChangeTurn();
    }
    public void CellClicked()
    {
        // Prevent player input if AI is thinking
        if (AiController.IsAiThinking) return;
        // check if it is already clicked
        if (IsMarked) return;

        // cell clicked change the properties

        PlayerType currentPlayer = TurnManager.Instance.CurrentPlayer;
        MarkedBy = currentPlayer;

        // enable the Image component to show the mark
        // trigger the animation
        marker.SetActive(true);
        marker.GetComponent<Image>().sprite = (currentPlayer == PlayerType.X) ? XSprite : OSprite;
        particleSystem1.Play();
        particleSystem2.Play();



        cellButton.enabled = false;
        IsMarked = true;
        TurnManager.Instance.DecreaseTurnCount();

        if (WinChecker.Instance.CheckWinningState(currentPlayer, row, col))
        {
            GridManager.Instance.RaiseOnGameComplete(currentPlayer);
        }

        else if (TurnManager.Instance.turnCounter == 0)
        {
            GridManager.Instance.RaiseOnGameComplete(PlayerType.None);
        }
        else
        {
            TurnManager.Instance.ChangeTurn();
        }
    }

    public void SetTemporaryMark(PlayerType currentPlayer)
    {
        MarkedBy = currentPlayer;
        IsMarked = true;
    }

    public void ClearMark()
    {
        MarkedBy = PlayerType.None;
        IsMarked = false;
    }

    

    public bool IsMarkedBy(PlayerType player)
    {
        return IsMarked == true && MarkedBy == player;
    }
}
