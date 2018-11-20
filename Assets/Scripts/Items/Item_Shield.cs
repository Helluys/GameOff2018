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
    }

    public override void OnUse(Player player)
    {
        player.combat.InstantiateShield(radius,duration);
    }
}
