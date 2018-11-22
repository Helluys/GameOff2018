using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_SequenceRepeater : Item {

    private int repeatAmount;

    public Item_SequenceRepeater(int repeatAmount)
    {
        type = ItemType.Repeater;
        this.repeatAmount = repeatAmount;
        ItemManager.Instance.SetSprite(this);
    }

    public override string GetInfo()
    {
        return string.Format("Repeat the last input {0} times", repeatAmount);
    }

    public override void OnUse(Player player)
    {
        player.sequenceManager.sequence.Repeat(repeatAmount);
    }
}
