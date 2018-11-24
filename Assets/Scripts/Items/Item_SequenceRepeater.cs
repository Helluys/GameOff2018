using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_SequenceRepeater : Item {

    private int repeatAmount;

    public Item_SequenceRepeater(ItemStrength strength)
    {
        type = ItemType.Repeater;
        this.strength = strength;

        switch (strength)
        {
            case ItemStrength.Weak:
                repeatAmount = 2;
                break;
            case ItemStrength.Medium:
                repeatAmount = 4;
                break;
            case ItemStrength.Strong:
                repeatAmount = 6;
                break;
        }

        ItemManager.Instance.SetUpItem(this);
    }

    public override string GetInfo()
    {
        return string.Format("Repeat the last input {0} times", repeatAmount);
    }

    public override void OnUse(Player player)
    {
        base.OnUse(player);
        player.sequenceManager.sequence.Repeat(repeatAmount);
    }
}
