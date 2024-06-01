using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheckoutUI : MonoBehaviour {
    [SerializeField] private GameObject checkoutCanvas;
    [SerializeField] private Text moneyText;
    [SerializeField] private Button[][] inventoryButtons;
    [SerializeField] private Button[][] cartButtons;

    private float playerMoney;

    public void ShowCheckoutUI(int money, List<ClothingItem> inventory, List<ClothingItem> shoppingCart) {
        playerMoney = money;
        moneyText.text = $"Money: ${playerMoney}";

        UpdateInventoryButtons(inventory);
        UpdateCartButtons(shoppingCart);

        checkoutCanvas.SetActive(true);
    }

    public void HideCheckoutUI() {
        checkoutCanvas.SetActive(false);
    }

    private void UpdateInventoryButtons(List<ClothingItem> inventory) {
        for (int i = 0; i < inventoryButtons.Length; i++) {
            for (int j = 0; j < inventoryButtons[i].Length; j++) {
                int index = i * inventoryButtons[i].Length + j;
                if (index < inventory.Count) {
                    inventoryButtons[i][j].gameObject.SetActive(true);
                    inventoryButtons[i][j].GetComponent<Image>().sprite = inventory[index].Image;
                    int itemIndex = index;
                    inventoryButtons[i][j].onClick.RemoveAllListeners();
                    inventoryButtons[i][j].onClick.AddListener(() => SellItem(inventory[itemIndex]));
                } else {
                    inventoryButtons[i][j].gameObject.SetActive(false);
                }
            }
        }
    }

    private void UpdateCartButtons(List<ClothingItem> shoppingCart) {
        for (int i = 0; i < cartButtons.Length; i++) {
            for (int j = 0; j < cartButtons[i].Length; j++) {
                int index = i * cartButtons[i].Length + j;
                if (index < shoppingCart.Count) {
                    cartButtons[i][j].gameObject.SetActive(true);
                    cartButtons[i][j].GetComponent<Image>().sprite = shoppingCart[index].Image;
                    int itemIndex = index;
                    cartButtons[i][j].onClick.RemoveAllListeners();
                    cartButtons[i][j].onClick.AddListener(() => BuyItem(shoppingCart[itemIndex]));
                } else {
                    cartButtons[i][j].gameObject.SetActive(false);
                }
            }
        }
    }

    private void SellItem(ClothingItem item) {
        playerMoney += Mathf.FloorToInt(item.Value * 0.1f);
        moneyText.text = $"Money: ${playerMoney}";
        // Implement logic to remove item from inventory
    }

    private void BuyItem(ClothingItem item) {
        if (playerMoney >= item.Value) {
            playerMoney -= item.Value;
            moneyText.text = $"Money: ${playerMoney}";
            // Implement logic to remove item from shopping cart
        } else {
            Debug.Log("Not enough money to buy this item!");
        }
    }
}
