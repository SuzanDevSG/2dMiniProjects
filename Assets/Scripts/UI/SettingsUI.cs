using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingsUI : MonoBehaviour
{
    [SerializeField] private Slider gridSizeSlider;
    [SerializeField] private TMP_Dropdown gameModeSelector;
    [SerializeField] private TMP_Text gridLabel;
    [SerializeField] private Button playButton;

    private void Start()
    {
        gridSizeSlider.onValueChanged.AddListener(HandleGridSize);
        gameModeSelector.onValueChanged.AddListener(HandleGameMode);
        playButton.onClick.AddListener(PlayGame);
        HandleGrideSizeLabel(SettingsManager.Instance.GridSize);
    }

    private void PlayGame()
    {
        SceneManager.LoadScene("Game");
    }

    private void OnEnable()
    {
        if (SettingsManager.Instance != null)
        {
            SettingsManager.Instance.OnGridSizeChanged += HandleGrideSizeLabel;
        }
    }
    private void OnDisable()
    {
        if (SettingsManager.Instance != null)
        {
            SettingsManager.Instance.OnGridSizeChanged -= HandleGrideSizeLabel;
        }
    }
    private void HandleGrideSizeLabel(int size)
    {
        gridLabel.text = $"Grid : {size} x {size}";
    }

    private void OnDestroy()
    {
        gridSizeSlider.onValueChanged.RemoveAllListeners();
        gameModeSelector.onValueChanged.RemoveAllListeners();
        playButton.onClick.RemoveAllListeners();

    }
    private void HandleGameMode(int gameModeIndex)
    {
        SettingsManager.Instance.SetGameMode((GameMode)gameModeIndex);
    }
    private void HandleGridSize(float size)
    {
        int gridSize = Mathf.RoundToInt(size);
        SettingsManager.Instance.SetGridSize(gridSize);
    }
}
