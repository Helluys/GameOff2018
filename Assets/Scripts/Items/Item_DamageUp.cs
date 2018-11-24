using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_DamageUp : Item {

    private float damageMultiplier;

    public Item_DamageUp(ItemStrength strength)
    {
        type = ItemType.DamageUp;
        this.strength = strength;

        switch (strength)
        {
            case ItemStrength.Weak:
                damageMultiplier = 1.2f;
                break;
            case ItemStrength.Medium:
                damageMultiplier = 1.5f;
                break;
            case ItemStrength.Strong:
                damageMultiplier = 2.0f;
                break;
        }

        
        ItemManager.Instance.SetUpItem(this);
    }

    public override string GetInfo()
    {
        return string.Format("Multiply damage by {0} for the current sequence", damageMultiplier);
    }

    public override void OnUse(Player player)
    {
        base.OnUse(player);
        player.combat.DamageUp(damageMultiplier);
    }
}
