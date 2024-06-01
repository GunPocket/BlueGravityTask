using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class ClothingSelectionManager : MonoBehaviour {
    public enum ClothingType { Hat, Shirt, Pants, Shoes }

    [Header("Player Reference")]
    [Tooltip("Reference to the player's Transform.")]
    [SerializeField] private GameObject playerTransform;

    [System.Serializable]
    public struct ClothingOption {
        public ClothingType type;
        public List<ClothingItem> items;
    }

    [Header("UI Texts")]
    [Tooltip("Reference to the item price on the cart.")]
    [SerializeField] private TMP_Text currentPriceText;
    [Tooltip("Reference to the cart items.")]
    [SerializeField] private TMP_Text cartItemsText;
    [Tooltip("Reference to the clothe name.")]
    [SerializeField] private TMP_Text clotheNameText;
    [Tooltip("Reference to the clothe description.")]
    [SerializeField] private TMP_Text clotheDescriptionText;
    [Tooltip("Reference to the clothe value.")]
    [SerializeField] private TMP_Text clotheValueText;
    [SerializeField] private GameObject clotheInfo;

    [Header("UI Buttons")]
    [Tooltip("Reference to the 'Add to Cart' button.")]
    [SerializeField] private Button addToCartButton;
    [Tooltip("Reference to the 'Exit' button.")]
    [SerializeField] private Button exitButton;
    [Tooltip("List of buttons for clothing types.")]
    [SerializeField] private List<Button> typeOfClotheBtn;
    [Tooltip("List of buttons for clothing options.")]
    [SerializeField] private List<Button> clothesOptionBtns;

    [Header("Clothing Options")]
    [Tooltip("List of available clothing options.")]
    [SerializeField] private List<ClothingOption> clothingOptions;

    private List<ClothingItem> cartItems = new List<ClothingItem>();
    private Dictionary<ClothingType, List<ClothingItem>> availableClothing = new Dictionary<ClothingType, List<ClothingItem>>();
    private Dictionary<ClothingType, ClothingItem> selectedClothing = new Dictionary<ClothingType, ClothingItem>();

    private void Start() {
        addToCartButton.onClick.AddListener(AddToCart);
        exitButton.onClick.AddListener(Exit);

        for (int i = 0; i < typeOfClotheBtn.Count; i++) {
            int index = i;
            typeOfClotheBtn[i].onClick.AddListener(() => ShowClothingOptions((ClothingType)index));
        }

        UpdateCharacterImage();
        UpdateCurrentPrice();
    }

    public void ShowClothingOptions(ClothingType type) {
        ClearClothingButtons();


        if (!availableClothing.ContainsKey(type)) {
            availableClothing[type] = new List<ClothingItem>();
            ClothingOption option = clothingOptions.Find(opt => opt.type == type);
            availableClothing[type].AddRange(option.items);
        }

        List<ClothingItem> items = availableClothing[type];
        for (int i = 0; i < items.Count; i++) {
            clothesOptionBtns[i].gameObject.SetActive(true);
            clothesOptionBtns[i].GetComponent<Image>().sprite = items[i].Image;
            int index = i;
            clothesOptionBtns[i].onClick.AddListener(() => OnClothingOptionSelected(type, items[index]));
        }

        for (int i = items.Count; i < clothesOptionBtns.Count; i++) {
            clothesOptionBtns[i].gameObject.SetActive(false);
        }
    }

    private void OnClothingOptionSelected(ClothingType type, ClothingItem item) {
        clotheInfo.SetActive(true);
        if (!selectedClothing.ContainsValue(item)) {
            selectedClothing[type] = item;
            UpdateClothingDetails(item);
        }
    }

    private void UpdateClothingDetails(ClothingItem item) {
        clotheNameText.text = item.Name;
        clotheDescriptionText.text = item.Description;
        clotheValueText.text = $"Value: ${item.Value}";
    }

    private void UpdateCharacterImage() {
        // Implement logic later
    }

    private void UpdateCurrentPrice() {
        float totalPrice = 0;

        foreach (var item in cartItems) {
            totalPrice += item.Value;
        }

        currentPriceText.text = $"Total: ${totalPrice}";
    }

    private void AddToCart() {
        foreach (var item in selectedClothing.Values) {
            if (!item.IsDefault && !cartItems.Contains(item)) {
                cartItems.Add(item);
            }
        }

        selectedClothing.Clear();
        UpdateCartItemsText();
        UpdateCurrentPrice();
    }

    private void UpdateCartItemsText() {
        string cartText = "Cart Items:\n";
        foreach (var item in cartItems) {
            cartText += $"{item.Name}\n";
        }
        cartItemsText.text = cartText;
    }

    private void ClearClothingButtons() {
        foreach (Button button in clothesOptionBtns) {
            button.gameObject.SetActive(false);
            button.onClick.RemoveAllListeners();
        }
    }

    private void Exit() {
        playerTransform.GetComponent<InventoryManager>().AddItemsToCart(cartItems);

        gameObject.SetActive(false);
        cartItems.Clear();
        selectedClothing.Clear();
        UpdateCartItemsText();
        UpdateCurrentPrice();

        playerTransform.SetActive(true);
    }
}
