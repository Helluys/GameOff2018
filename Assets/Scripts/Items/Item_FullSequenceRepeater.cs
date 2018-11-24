using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_FullSequenceRepeater : Item {

    public Item_FullSequenceRepeater()
    {
        strength = ItemStrength.Strong;
        type = ItemType.FullRepeater;
        ItemManager.Instance.SetUpItem(this);
    }

    public override string GetInfo()
    {
        return "Repeat current sequence";
    }

    public override void OnUse(Player player)
    {
        base.OnUse(player);
        player.sequenceManager.sequence.RepeatSequence();
    }
}
