using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Undefined,
    Reducer,
    Repeater,
    FullRepeater,
    DamageUp,
    Shield
}

[System.Serializable]
public class Item{
    public ItemType type = ItemType.Undefined;
    public Sprite sprite;
    public virtual void OnUse(Player player) { }
    public virtual string GetInfo() { return string.Empty; }
    public  Item()
    {
        if(ItemManager.Instance != null)
        ItemManager.Instance.SetSprite(this);
    }
}
