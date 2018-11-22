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

public class Item{
    public ItemType type = ItemType.Undefined;
    public Sprite sprite;
    public virtual void OnUse(Player player) { }
    public  Item()
    {
        ItemManager.Instance.SetSprite(this);
    }
}
