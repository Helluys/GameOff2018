using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_FullSequenceRepeater : Item {

    public Item_FullSequenceRepeater()
    {
        type = ItemType.FullRepeater;
    }
    public override void OnUse(Player player)
    {
        player.sequenceManager.sequence.RepeatSequence();
    }
}
