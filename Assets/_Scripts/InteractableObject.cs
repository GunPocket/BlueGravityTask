using UnityEngine;

public class InteractableObject : MonoBehaviour {
    [SerializeField] private GameObject interactSprite;
    [SerializeField] private ClothingItem clothingItem;

    public ClothingItem ClothingItem { get { return clothingItem; } }

    [TextArea(1, 5)][SerializeField] private string message;

    public string Message { get { return message; } }

    private void Start() {
        HideSprite();
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        ShowSprite();
    }

    private void OnTriggerExit2D(Collider2D collision) {
        HideSprite();
    }

    public void ShowSprite() {
        interactSprite.SetActive(true);
    }

    public void HideSprite() {
        interactSprite.SetActive(false);
    }
}
