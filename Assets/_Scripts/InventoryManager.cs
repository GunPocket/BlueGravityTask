using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour {
    public List<ClothingItem> ShoppingCart = new List<ClothingItem>();
    public List<ClothingItem> Inventory = new List<ClothingItem>();

    public void AddItem(ClothingItem item) {
        ShoppingCart.Add(item);
    }

    public void RemoveItem(ClothingItem item) {
        ShoppingCart.Remove(item);
    }

    public void ShowInventory() {
        if (ShoppingCart.Count > 0) {
            print("Shopping Cart:");
            foreach (var item in ShoppingCart) {
                Debug.Log($"Item: {item.Name}, Type: {item.Type}, Value: ${item.Value}");
            }
            ShowPricesCombined();
        } else {
            print("Shopping cart is empty");
        }


        if (Inventory.Count > 0) {
            print("Inventory:");
            foreach (var item in Inventory) {
                Debug.Log($"Item: {item.Name}, Type: {item.Type}");
            }
        } else {
            print("Inventory is empty");
        }
    }

    private void ShowPricesCombined() {
        float price = 0;

        foreach (var item in ShoppingCart) {
            price += item.Value;
        }
        print($"Total price on shopping cart: ${price}");
    }

    public bool CheckItemIsHere(ClothingItem item) {
        if (ShoppingCart.Count > 0) {
            if (!ShoppingCart.Contains(item)) return false;
            return true;
        } else {
            return false;
        }
    }

    public void AddItemsToCart(List<ClothingItem> items) {
        foreach (var item in items) {
            ShoppingCart.Add(item);
        }
    }
}
