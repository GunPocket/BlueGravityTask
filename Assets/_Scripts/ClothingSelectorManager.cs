using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class ClothingSelectionManager : MonoBehaviour {
    public enum ClothingType { Hat, Shirt, Pants, Shoes }

    [System.Serializable]
    public struct ClothingOption {
        public ClothingType ClotheType;
        public List<ClothingItem> Items;
    }

    [Header("UI Elements")]
    public GameObject ClothingButtonPrefab;
    public Transform Content;
    public Image HatImage;
    public Image ShirtImage;
    public Image PantsImage;
    public Image ShoeImage;
    public TMP_Text CurrentPriceText;
    public Button AddToCartButton;

    [Header("Clothing Options")]
    public List<ClothingOption> ClothingOptions;

    private Dictionary<ClothingType, ClothingItem> selectedClothing;
    private InventoryManager inventoryManager;

    private void Start() {
        selectedClothing = new Dictionary<ClothingType, ClothingItem>();
        inventoryManager = FindObjectOfType<InventoryManager>();

        AddToCartButton.onClick.AddListener(AddToCart);
    }

    public void ShowClothingOptions(ClothingType type) {
        ClearClothingButtons();

        ClothingOption option = ClothingOptions.Find(opt => opt.ClotheType == type);
        foreach (var item in option.Items) {
            GameObject buttonObj = Instantiate(ClothingButtonPrefab, Content);
            Button button = buttonObj.GetComponent<Button>();
            Text buttonText = buttonObj.GetComponentInChildren<Text>();
            buttonText.text = item.Name;

            button.onClick.AddListener(() => OnClothingOptionSelected(type, item));
        }
    }

    private void ClearClothingButtons() {
        foreach (Transform child in Content) {
            Destroy(child.gameObject);
        }
    }

    private void OnClothingOptionSelected(ClothingType type, ClothingItem item) {
        selectedClothing[type] = item;
        UpdateCharacterImage();
        UpdateCurrentPrice();
    }

    private void UpdateCharacterImage() {
        // Implement logic later
    }

    private void UpdateCurrentPrice() {
        float totalPrice = 0;

        foreach (var item in selectedClothing.Values) {
            totalPrice += item.Value;
        }

        CurrentPriceText.text = $"Total: ${totalPrice}";
    }

    private void AddToCart() {
        foreach (var item in selectedClothing.Values) {
            if (!item.IsDefault) {
                inventoryManager.AddItem(item);
            }
        }
    }
}
