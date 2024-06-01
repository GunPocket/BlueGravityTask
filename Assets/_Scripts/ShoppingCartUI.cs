using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static ClothingItem;

public class ShoppingCartUI : MonoBehaviour {
    [SerializeField] private TMP_Text clothingValueAllText;
    [SerializeField] private List<ShoppingCartOption> clothingImageAreas;

    [System.Serializable]
    public struct ShoppingCartOption {
        public ItemType type;
        public List<Image> imageAreas;
    }

    public void UpdateClothingValueAllText(float totalValue) {
        clothingValueAllText.text = $"Total Value: ${totalValue:F2}";
    }

    public void UpdateClothingImages(List<ClothingItem> shoppingCart) {
        ClearAllImageAreas();

        var groupedItems = GroupItemsByType(shoppingCart);

        foreach (var option in clothingImageAreas) {
            if (groupedItems.ContainsKey(option.type)) {
                var item = groupedItems[option.type];
                for (int i = 0; i < option.imageAreas.Count; i++) {
                    if (i < item.Count) {
                        option.imageAreas[i].gameObject.SetActive(true);
                        option.imageAreas[i].sprite = item[i].Image;
                    } else {
                        option.imageAreas[i].gameObject.SetActive(false);
                    }
                }
            }
        }

        UpdateClothingValueAllText(CalculateTotalValue(shoppingCart));
    }

    private Dictionary<ItemType, List<ClothingItem>> GroupItemsByType(List<ClothingItem> shoppingCart) {
        var groupedItems = new Dictionary<ItemType, List<ClothingItem>>();

        foreach (var item in shoppingCart) {
            if (!groupedItems.ContainsKey(item.Type)) {
                groupedItems[item.Type] = new List<ClothingItem>();
            }
            groupedItems[item.Type].Add(item);
        }

        return groupedItems;
    }

    private void ClearAllImageAreas() {
        foreach (var option in clothingImageAreas) {
            ClearImageAreas(option.imageAreas);
        }
    }

    private void ClearImageAreas(List<Image> imageAreas) {
        foreach (var imageArea in imageAreas) {
            imageArea.sprite = null;
        }
    }

    private void UpdateClothingImage(ItemType itemType, ClothingItem item) {
        foreach (var option in clothingImageAreas) {
            if (option.type == itemType) {
                if (item != null) {
                    option.imageAreas[0].sprite = item.Image;
                }
                break;
            }
        }
    }

    private float CalculateTotalValue(List<ClothingItem> items) {
        float totalValue = 0;
        foreach (var item in items) {
            totalValue += item.Value;
        }
        return totalValue;
    }
}
