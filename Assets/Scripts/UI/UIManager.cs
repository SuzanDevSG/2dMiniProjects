using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Button restartButton;
    [SerializeField] private Button MainMenuButton;

    private void Start()
    {
        restartButton.onClick.AddListener(() => LoadScene(SceneManager.GetActiveScene().buildIndex));
        MainMenuButton.onClick.AddListener(() => LoadScene(2));
    }
    private void OnDestroy()
    {
        restartButton.onClick.RemoveAllListeners();
    }

    private void LoadScene(int SceneIndex)
    {
        SceneManager.LoadScene(SceneIndex);
    }
}
