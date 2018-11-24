using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Undefined = -1,
    Reducer = 0,
    Repeater = 1,
    FullRepeater = 2,
    DamageUp = 3,
    Shield = 4,
    HealthUp = 5,
    StaminaUp = 6
}

public enum ItemStrength
{
    Weak = 0,
    Medium = 1,
    Strong = 2
}

public class Item{

    public ItemType type = ItemType.Undefined;
    public ItemStrength strength = ItemStrength.Weak;
    public Sprite sprite;
    public Color color;
    public AudioClip sound;
    public virtual void OnUse(Player player)
    {
        if (type == ItemType.Undefined)
            return;

        SoundController.Instance.Say(sound);
    }

    public virtual string GetInfo() { return string.Empty; }

    public  Item()
    {
        if(ItemManager.Instance != null)
        ItemManager.Instance.SetUpItem(this);
    }
}
