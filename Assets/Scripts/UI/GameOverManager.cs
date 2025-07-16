using TMPro;
using UnityEngine;
public class GameOverManager : MonoBehaviour
{
    public static GameOverManager Instance { get; private set;}
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private TMP_Text resultText;

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
    public void ShowResult(string result)
    {
        resultText.text = result;
        gameOverPanel.SetActive(true);
        AiController.Instance.StopAiCorutine();
    }
}
