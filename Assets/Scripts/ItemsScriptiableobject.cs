using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Item")]
public class ItemsScriptiableObject : ScriptableObject
{
    public string itemName;
    public Sprite itemImage;
    public CharacterLayer characterLayer;
    public ItemCategory itemCategory;
}
