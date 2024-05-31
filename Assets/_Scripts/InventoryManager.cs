using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour {
    public List<ClothingItem> inventory = new List<ClothingItem>();

    public void AddItem(ClothingItem item) {
        inventory.Add(item);
    }

    public void RemoveItem(ClothingItem item) {
        inventory.Remove(item);
    }

    public void ShowInventory() {
        if (inventory.Count > 0) {
            print("Inventory:");
            foreach (var item in inventory) {
                Debug.Log($"Item: {item.Name}, Type: {item.Type}, Value: ${item.Value}");
            }
            ShowPricesCombined();
        } else { 
            print("Inventory is empty");
        }
        print("");
    }

    private void ShowPricesCombined() {
        float price = 0;

        foreach (var item in inventory) {
            price += item.Value;
        }
        print($"Total price on inventory: ${price}");
    }
}
