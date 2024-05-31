using UnityEngine;

public class InteractableObject : MonoBehaviour {
    [SerializeField] private GameObject interactSprite;
    [SerializeField] private ClothingItem clothingItem;

    public ClothingItem ClothingItem { get { return clothingItem; } }

    [TextArea(1, 5)][SerializeField] private string message;

    public string Message { get { return message; } }

    private void Start() {
        interactSprite.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        interactSprite.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D collision) {
        interactSprite.SetActive(false);
    }
}
