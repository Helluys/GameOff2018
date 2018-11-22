using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Shield : Item
{
    private float radius;
    private float duration;

    public Item_Shield(float radius,float duration)
    {
        this.type = ItemType.Shield;
        this.radius = radius;
        this.duration = duration;
        ItemManager.Instance.SetSprite(this);
    }

    public override string GetInfo()
    {
        return string.Format("Create a shield of radius {0} during {1} seconds",radius,duration);
    }

    public override void OnUse(Player player)
    {
        player.combat.InstantiateShield(radius,duration);
    }
    
}
