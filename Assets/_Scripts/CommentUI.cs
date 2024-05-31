using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CommentUI : MonoBehaviour {
    [SerializeField] private TMP_Text clothingName;
    [SerializeField] private Image clothingSprite;
    [SerializeField] private TMP_Text messageText;

    public void ShowUi(string message, ClothingItem item) {
        gameObject.SetActive(true);
        
        clothingName.text = $"{item.Name} (${item.Value})";
        clothingSprite.sprite = item.Image;
        messageText.text = message;
    }

    public void HideUi() {
        gameObject.SetActive(false);
    }

}
