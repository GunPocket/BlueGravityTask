using UnityEngine;

[CreateAssetMenu(fileName = "New Clothe", menuName = "Clothe")]
public class ClothingItem : ScriptableObject {
    public enum ItemType { Hat, Shirt, Pants, Shoes}
    public string Name;
    public ItemType Type;
    public float Value;
    public Sprite Image;
    public Sprite IdleSprite;
    public Sprite ClotSeleSprite;
    public bool IsDefault = false;

    [TextArea(1, 5)] public string Description;   
}
