using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using static ClothingItem;

public class InventoryUI : MonoBehaviour {
    [SerializeField] private List<ButtonOption> clothingOptions;

    [SerializeField] private PlayerController playerController;

    [System.Serializable]
    public struct ButtonOption {
        public ItemType type;
        public List<Button> buttons;
    }

    public void UpdateClothingItemButtons(List<ClothingItem> clothingItems) {
        foreach (var option in clothingOptions) {
            var filteredItems = clothingItems.Where(item => item.Type == option.type).ToList();

            for (int i = 0; i < option.buttons.Count; i++) {
                if (i < filteredItems.Count) {
                    var button = option.buttons[i];
                    button.gameObject.SetActive(true);
                    button.GetComponent<Image>().sprite = filteredItems[i].Image;
                    button.onClick.RemoveAllListeners();
                    int index = i;
                    button.onClick.AddListener(() => OnClothingItemButtonClick(filteredItems[index]));
                } else {
                    option.buttons[i].gameObject.SetActive(false);
                }
            }
        }
    }

    private void OnClothingItemButtonClick(ClothingItem item) {
        playerController.EquipItem(item);
    }
}

