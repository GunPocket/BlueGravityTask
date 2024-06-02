using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ExitGameInteractable : MonoBehaviour {
    [Header("UI Reference")]
    [SerializeField] private Button yesButton;
    [SerializeField] private Button noButton;

    [Header("Player Reference")]
    [SerializeField] private PlayerController playerController;

    private void Start() {
        gameObject.SetActive(false);

        yesButton.onClick.AddListener(() => ConfirmExit());
        noButton.onClick.AddListener(() => CancelExit());
    }

    public void ConfirmExit() {
        playerController.ExitGame();
    }

    public void CancelExit() {
        gameObject.SetActive(false);
        playerController.SetWalkingState();
    }
}
