using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour {
    public List<ClothingItem> ShoppingCart = new List<ClothingItem>();
    public List<ClothingItem> Inventory = new List<ClothingItem>();

    [SerializeField] private GameObject inventoryCanvas;
    public InventoryUI InventoryUI;
    [SerializeField] private ShoppingCartUI shoppingCartUI;
    [SerializeField] private PlayerController playerController;

    private void Start() {
        inventoryCanvas.SetActive(false);
    }

    public void ShowInventoryUI() {
        UpdateInventoryUI();
        EquipItemsFromInventory();
        inventoryCanvas.SetActive(true);
    }

    private void EquipItemsFromInventory() {
        foreach (var item in Inventory) {
            playerController.EquipItem(item);
        }
    }

    public void HideInventoryUI() {
        inventoryCanvas.SetActive(false);
    }

    private void UpdateInventoryUI() {
        UpdateClothingItemButtons();
        UpdateShoppingCartUI();
    }

    private void UpdateClothingItemButtons() {
        InventoryUI.UpdateClothingItemButtons(Inventory);
    }

    private void UpdateShoppingCartUI() {
        shoppingCartUI.UpdateClothingImages(ShoppingCart);
    }

    public void AddItemToInventory(ClothingItem item) {
        Inventory.Add(item);
    }

    public void RemoveItemFromInventory(ClothingItem item) {
        Inventory.Remove(item);
        UpdateInventoryUI();
    }

    public void AddItemToCart(ClothingItem item) {
        ShoppingCart.Add(item);
        UpdateShoppingCartUI();
    }

    public void RemoveItemFromCart(ClothingItem item) {
        ShoppingCart.Remove(item);
        UpdateShoppingCartUI();
    }

    private ClothingItem GetEquippedItem(ClothingItem.ItemType itemType) {
        return Inventory.Find(item => item.Type == itemType);
    }
}
