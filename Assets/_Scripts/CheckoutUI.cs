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
        public List<Button> imageAreas;
    }

    public void Awake() {
        exitButton.onClick.AddListener(ExitCheckout);
        ShowCheckoutUI();
    }

    public void ShowCheckoutUI() {
        UpdateMoneyText();
        UpdateInventoryButtons(playerController.InventoryManager.Inventory);
        UpdateCartButtons(playerController.InventoryManager.ShoppingCart);

        gameObject.SetActive(true);
    }

    public void HideCheckoutUI() {
        gameObject.SetActive(false);
    }

    private void UpdateMoneyText() {
        playerController.UpdateMoney($"Money: ${playerController.PlayerMoney:F2}");
    }

    private void UpdateInventoryButtons(List<ClothingItem> inventory) {
        ClearAllImageAreas(inventoryImageAreas);

        foreach (var item in inventory) {
            UpdateClothingImage(inventoryImageAreas, item, () => SellItem(item));
        }
    }

    private void UpdateCartButtons(List<ClothingItem> shoppingCart) {
        ClearAllImageAreas(cartImageAreas);

        foreach (var item in shoppingCart) {
            UpdateClothingImage(cartImageAreas, item, () => BuyItem(item));
        }
    }

    private void ClearAllImageAreas(List<ShoppingCartOption> options) {
        foreach (var option in options) {
            ClearImageAreas(option.imageAreas);
        }
    }

    private void ClearImageAreas(List<Button> imageAreas) {
        foreach (var imageArea in imageAreas) {
            imageArea.gameObject.SetActive(false);
        }
    }

    private void UpdateClothingImage(List<ShoppingCartOption> options, ClothingItem item, UnityEngine.Events.UnityAction action) {
        foreach (var option in options) {
            if (option.type == item.Type) {
                for (int i = 0; i < option.imageAreas.Count; i++) {
                    if (!option.imageAreas[i].gameObject.activeSelf) {
                        option.imageAreas[i].gameObject.SetActive(true);
                        option.imageAreas[i].GetComponent<Image>().sprite = item.Image;
                        option.imageAreas[i].onClick.RemoveAllListeners();
                        option.imageAreas[i].onClick.AddListener(action);
                        break;
                    }
                }
            }
        }
    }

    private void SellItem(ClothingItem item) {
        if (playerController.CurrentlyWearing.Contains(item)) {
            Debug.Log("Cannot sell an item that you are currently wearing!");
            return;
        }

        int sellValue = Mathf.FloorToInt(item.Value * 0.1f);
        playerController.PlayerMoney += sellValue;
        UpdateMoneyText();

        if (playerController.InventoryManager.Inventory.Contains(item)) {
            playerController.InventoryManager.RemoveItemFromInventory(item);
            UpdateInventoryButtons(playerController.InventoryManager.Inventory);
        } else if (playerController.InventoryManager.ShoppingCart.Contains(item)) {
            playerController.InventoryManager.RemoveItemFromCart(item);
            UpdateCartButtons(playerController.InventoryManager.ShoppingCart);
        }
    }

    private void BuyItem(ClothingItem item) {
        if (playerController.PlayerMoney >= item.Value) {
            playerController.PlayerMoney -= item.Value;
            UpdateMoneyText();
            playerController.InventoryManager.RemoveItemFromCart(item);
            playerController.InventoryManager.AddItemToInventory(item);
            UpdateCartButtons(playerController.InventoryManager.ShoppingCart);
            UpdateInventoryButtons(playerController.InventoryManager.Inventory);
        }
    }

    private void ExitCheckout() {
        HideCheckoutUI();
        playerController.SetWalkingState();
    }
}
