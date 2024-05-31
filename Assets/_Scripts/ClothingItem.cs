using UnityEngine;

[CreateAssetMenu(fileName = "New Clothe", menuName = "Clothe")]
public class ClothingItem : ScriptableObject {
    public string Name;
    public ItemType Type;
    public float Value;
    public Sprite Image;

    public enum ItemType {
        Pants,
        Shirt,
        Shoes,
        Hat
    }
}
