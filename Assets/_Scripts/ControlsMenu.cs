using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ControlsMenu : MonoBehaviour {
    [Header("UI Buttons")]
    [SerializeField] private Button backButton;

    private void Start() {
        backButton.onClick.AddListener(BackToMainMenu);
    }

    private void BackToMainMenu() {
        SceneManager.LoadScene("MainMenuScene");
    }
}
