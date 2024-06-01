using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static ClothingItem;

public class InventoryUI : MonoBehaviour {
    [SerializeField] private TMP_Text[] currentlyWearingTexts;
    [SerializeField] private List<ButtonOption> clothingOptions;

    [System.Serializable]
    public struct ButtonOption {
        public ItemType type;
        public List<Button> buttons;
    }

    public TMP_Text[] CurrentlyWearingTexts { get { return currentlyWearingTexts; } }

    public void UpdateCurrentlyWearingText(int sectionIndex, string itemName) {
        currentlyWearingTexts[sectionIndex].text = $"Currently wearing: {itemName}";
    }

    public void UpdateClothingItemButtons(List<ClothingItem> clothingItems) {
        foreach (var option in clothingOptions) {
            // Filtrar os itens de acordo com o tipo da opção
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
        // Implement logic to equip the selected item and update currently wearing text
        Debug.Log($"Equipping item: {item.Name}");
    }
}
