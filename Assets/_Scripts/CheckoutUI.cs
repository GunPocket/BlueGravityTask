using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheckoutUI : MonoBehaviour {
    [SerializeField] private List<ShoppingCartOption> inventoryImageAreas;
    [SerializeField] private List<ShoppingCartOption> cartImageAreas;
    [SerializeField] private Button exitButton;
    [SerializeField] private PlayerController playerController;

    [System.Serializable]
    public struct ShoppingCartOption {
        public ClothingItem.ItemType type;
        public List<Image> imageAreas;
    }

    private void Awake() {
        exitButton.onClick.AddListener(ExitCheckout);
        ShowCheckoutUI();
    }

    public void ShowCheckoutUI() {
        UpdateMoneyText();
        UpdateUI(playerController.InventoryManager.Inventory, inventoryImageAreas, SellItem);
        UpdateUI(playerController.InventoryManager.ShoppingCart, cartImageAreas, BuyItem);
        gameObject.SetActive(true);
    }

    public void HideCheckoutUI() {
        gameObject.SetActive(false);
    }

    private void UpdateMoneyText() {
        playerController.UpdateMoney($"Money: ${playerController.PlayerMoney:F2}");
    }

    private void UpdateUI(List<ClothingItem> items, List<ShoppingCartOption> areas, System.Action<ClothingItem> action) {
        ClearAllImageAreas(areas);
        foreach (var item in items) {
            UpdateClothingImage(areas, item, action);
        }
    }

    private void ClearAllImageAreas(List<ShoppingCartOption> options) {
        foreach (var option in options) {
            ClearImageAreas(option.imageAreas);
        }
    }

    private void ClearImageAreas(List<Image> imageAreas) {
        foreach (var imageArea in imageAreas) {
            imageArea.gameObject.SetActive(false);
        }
    }

    private void UpdateClothingImage(List<ShoppingCartOption> options, ClothingItem item, System.Action<ClothingItem> action) {
        foreach (var option in options) {
            if (option.type == item.Type) {
                foreach (var imageArea in option.imageAreas) {
                    if (!imageArea.gameObject.activeSelf) {
                        imageArea.gameObject.SetActive(true);
                        imageArea.sprite = item.Image;
                        imageArea.GetComponent<Button>().onClick.RemoveAllListeners();
                        imageArea.GetComponent<Button>().onClick.AddListener(() => action.Invoke(item));
                        break;
                    }
                }
            }
        }
    }

    private void SellItem(ClothingItem item) {
        if (playerController.CurrentlyWearing.Contains(item)) {
            return;
        }

        int sellValue = Mathf.FloorToInt(item.Value * 0.1f);
        playerController.PlayerMoney += sellValue;
        UpdateMoneyText();

        if (playerController.InventoryManager.Inventory.Contains(item)) {
            playerController.InventoryManager.RemoveItemFromInventory(item);
            UpdateUI(playerController.InventoryManager.Inventory, inventoryImageAreas, SellItem);
        } else if (playerController.InventoryManager.ShoppingCart.Contains(item)) {
            playerController.InventoryManager.RemoveItemFromCart(item);
            UpdateUI(playerController.InventoryManager.ShoppingCart, cartImageAreas, BuyItem);
        }
    }

    private void BuyItem(ClothingItem item) {
        if (playerController.PlayerMoney >= item.Value) {
            playerController.PlayerMoney -= item.Value;
            UpdateMoneyText();

            playerController.InventoryManager.RemoveItemFromCart(item);
            playerController.InventoryManager.AddItemToInventory(item);

            UpdateUI(playerController.InventoryManager.ShoppingCart, cartImageAreas, BuyItem);
            UpdateUI(playerController.InventoryManager.Inventory, inventoryImageAreas, SellItem);
        }
    }

    private void ExitCheckout() {
        HideCheckoutUI();
        playerController.SetWalkingState();
    }
}
