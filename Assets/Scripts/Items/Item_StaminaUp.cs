using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_StaminaUp : Item {


    private float healAmount;

    public Item_StaminaUp(ItemStrength strength)
    {
        type = ItemType.StaminaUp;
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
        return string.Format("Restore 1/{0} of Timothee's stamina", healAmount);
    }

    public override void OnUse(Player player)
    {
        base.OnUse(player);
        player.StaminaRegen(player.sharedStatistics.maxStamina / healAmount);
    }
}
