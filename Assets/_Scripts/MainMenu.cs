using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {
    [Header("UI Buttons")]
    [SerializeField] private Button playButton;
    [SerializeField] private Button controlsButton;
    [SerializeField] private Button exitButton;

    private void Start() {
        playButton.onClick.AddListener(PlayGame);
        controlsButton.onClick.AddListener(ShowControls);
        exitButton.onClick.AddListener(ExitGame);
    }

    private void PlayGame() {
        SceneManager.LoadScene("Store");
    }

    private void ShowControls() {
        SceneManager.LoadScene("ControlsScene");
    }

    private void ExitGame() {
        Application.Quit();
    }
}
