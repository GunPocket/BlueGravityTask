using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour {
    public List<ClothingItem> ShoppingCart = new List<ClothingItem>();
    public List<ClothingItem> Inventory = new List<ClothingItem>();

    [SerializeField] private GameObject inventoryCanvas;
    [SerializeField] private InventoryUI inventoryUI;
    [SerializeField] private ShoppingCartUI shoppingCartUI;

    private void Start() {
        inventoryCanvas.SetActive(false);
    }

    public void ShowInventoryUI() {
        UpdateInventoryUI();
        inventoryCanvas.SetActive(true);
    }

    public void HideInventoryUI() {
        inventoryCanvas.SetActive(false);
    }

    private void UpdateInventoryUI() {
        UpdateCurrentlyWearingText();
        UpdateClothingItemButtons();
        UpdateShoppingCartUI();
    }

    private void UpdateCurrentlyWearingText() {
        for (int i = 0; i < inventoryUI.CurrentlyWearingTexts.Length; i++) {
            inventoryUI.UpdateCurrentlyWearingText(i, "Default");
        }
    }

    private void UpdateClothingItemButtons() {
        inventoryUI.UpdateClothingItemButtons(Inventory);
    }

    private void UpdateShoppingCartUI() {
        shoppingCartUI.UpdateClothingImages(ShoppingCart);
    }

    public void EquipItem(ClothingItem item) {
        Debug.Log($"Equipping item: {item.Name}");
    }

    public void UnequipItem(ClothingItem item) {
        Debug.Log($"Unequipping item: {item.Name}");
    }

    public void AddItemToInventory(ClothingItem item) {
        Inventory.Add(item);
    }

    public void RemoveItemFromInventory(ClothingItem item) {
        Inventory.Remove(item);
    }

    public void AddItemToCart(ClothingItem item) {
        ShoppingCart.Add(item);
        UpdateShoppingCartUI();
    }

    public void RemoveItemFromCart(ClothingItem item) {
        ShoppingCart.Add(item);
        UpdateShoppingCartUI();
    }
}
