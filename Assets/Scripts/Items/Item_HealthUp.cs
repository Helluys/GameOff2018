using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_HealthUp : Item {

    private float healAmount;

    public Item_HealthUp(ItemStrength strength)
    {
        type = ItemType.HealthUp;
        this.strength = strength;

        switch (strength)
        {
            case ItemStrength.Weak:
                healAmount = 4;
                break;
            case ItemStrength.Medium:
                healAmount = 3;
                break;
            case ItemStrength.Strong:
                healAmount = 2;
                break;
        }

        ItemManager.Instance.SetUpItem(this);
    }

    public override string GetInfo()
    {
        return string.Format("Restore 1/{0} of Timothee's health", healAmount);
    }

    public override void OnUse(Player player)
    {
        base.OnUse(player);
        player.Heal(player.sharedStatistics.maxHealth / healAmount);
    }
}
