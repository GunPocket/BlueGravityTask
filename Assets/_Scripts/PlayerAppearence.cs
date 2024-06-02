using System.Collections.Generic;
using UnityEngine;

public class PlayerAppearance : MonoBehaviour {
    [Header("Clothes Renderers")]
    [SerializeField] private SpriteRenderer hatRenderer;
    [SerializeField] private SpriteRenderer shirtRenderer;
    [SerializeField] private SpriteRenderer pantsRenderer;
    [SerializeField] private SpriteRenderer shoesRenderer;

    [Header("Player Reference")]
    [SerializeField] private PlayerController playerController;

    public void UpdateAppearance(List<ClothingItem> equippedItems) {
        ClearRenderers();

        foreach (var item in equippedItems) {
            switch (item.Type) {
                case ClothingItem.ItemType.Hat:
                    UpdateRenderer(hatRenderer, item.IdleSprite);
                    break;
                case ClothingItem.ItemType.Shirt:
                    UpdateRenderer(shirtRenderer, item.IdleSprite);
                    break;
                case ClothingItem.ItemType.Pants:
                    UpdateRenderer(pantsRenderer, item.IdleSprite);
                    break;
                case ClothingItem.ItemType.Shoes:
                    UpdateRenderer(shoesRenderer, item.IdleSprite);
                    break;
            }
        }
    }

    private void ClearRenderers() {
        hatRenderer.sprite = null;
        shirtRenderer.sprite = null;
        pantsRenderer.sprite = null;
        shoesRenderer.sprite = null;
    }

    private void UpdateRenderer(SpriteRenderer renderer, Sprite sprite) {
        if (renderer != null && sprite != null) {
            renderer.sprite = sprite;
        }
    }
}
