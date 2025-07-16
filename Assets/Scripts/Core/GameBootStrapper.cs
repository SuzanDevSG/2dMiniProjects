using UnityEngine;
using UnityEngine.SceneManagement;
public class GameBootStrapper : MonoBehaviour
{
    public GameObject settingManagerPrefab;
    public GameObject audioManagerPrefab;
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        if (SettingsManager.Instance == null)
        {
            Instantiate(settingManagerPrefab);
        }
        if (AudioManager.Instance == null)
        {
            Instantiate(audioManagerPrefab);
        }
        if (SceneManager.GetActiveScene().name == "BootStrap")
        {
            SceneManager.LoadScene("Settings");
        }
    }
}
