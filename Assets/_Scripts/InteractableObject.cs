using UnityEngine;

public class InteractableObject : MonoBehaviour {
    [SerializeField] private GameObject interactSprite;
    [SerializeField] private ClothingItem clothingItem;

    [Tooltip("This bool is only Serailized so I can debug")]
    [SerializeField] private bool itemHere = true;

    public ClothingItem ClothingItem { get { return clothingItem; } }
    public bool ItemHere { get { return itemHere; } }

    private void Start() {
        HideInteractSprite();
    }

    public void ShowInteractSprite() {
        interactSprite.SetActive(true);
    }

    public void HideInteractSprite() {
        interactSprite.SetActive(false);
    }

    public void Interact() {
        if (ClothingItem != null) {
            CollectItem();
        } else {
            ReturnItem();
        }
    }

    public void CollectItem() {
        itemHere = false;
    }

    public void ReturnItem() {
        itemHere = true;
    }
}
