using UnityEngine;

[System.Serializable]
public class ItemDataSave
{
    public string itemName;
    public string sprite;
    public string description;
}
public abstract class ItemData : ScriptableObject
{
    public string itemName;
    public Sprite itemIcon;

    public string description;
}
