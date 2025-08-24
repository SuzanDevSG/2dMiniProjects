using System.Collections;
using TMPro;
using UnityEngine;
public class GameOverManager : MonoBehaviour
{
    public static GameOverManager Instance { get; private set;}
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private TMP_Text resultText;


    private void Start()
    {
        GridManager.Instance.OnGameCompleted += ShowResult;
    }
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        gameOverPanel.SetActive(false);
    }
    public void ShowResult(PlayerType currentPlayer)
    {
        StartCoroutine(PrepareResult(currentPlayer));
        
    }
    private IEnumerator PrepareResult(PlayerType currentPlayer)
    {
        yield return new WaitForSeconds(1f);
        // Show the game over panel and set the result text
        gameOverPanel.SetActive(true);
        AiController.Instance.StopAiCorutine();
        if (currentPlayer == PlayerType.None)
        {
            resultText.text = "Draw :(";
        }
        else
        {
            resultText.text = $"{currentPlayer} : Wins!";
        }
    }
}
