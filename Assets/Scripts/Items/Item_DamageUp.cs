﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_DamageUp : Item {

    private float damageMultiplier;

    public Item_DamageUp(float damageMultiplier)
    {
        type = ItemType.DamageUp;
        this.damageMultiplier = damageMultiplier;
        ItemManager.Instance.SetSprite(this);
    }

    public override string GetInfo()
    {
        return string.Format("Multiply damage by {0} for the current sequence", damageMultiplier);
    }

    public override void OnUse(Player player)
    {
        player.combat.DamageUp(damageMultiplier);
    }
}