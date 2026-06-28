using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Item")]
public class ItemsScriptableObject : ScriptableObject
{
    public string itemName;
    public string assetname;
    public Sprite itemImage;
    public CharacterLayer characterLayer;
}
