using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Shield : Item
{
    private float radius;
    private float duration;

    public Item_Shield(ItemStrength strength)
    {
        this.strength = strength;
        this.type = ItemType.Shield;

        switch (strength)
        {
            case ItemStrength.Weak:
                radius = 10;
                duration = 2;
                break;
            case ItemStrength.Medium:
                radius = 15;
                duration = 2;
                break;
            case ItemStrength.Strong:
                radius = 15;
                duration = 3;
                break;
        }
        
        ItemManager.Instance.SetUpItem(this);
    }

    public override string GetInfo()
    {
        return string.Format("Create a shield of radius {0} during {1} seconds",radius,duration);
    }

    public override void OnUse(Player player)
    {
        base.OnUse(player);
        player.combat.InstantiateShield(radius,duration);
    }
    
}
