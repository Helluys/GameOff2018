using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_FullSequenceRepeater : Item {

    public Item_FullSequenceRepeater()
    {
        type = ItemType.FullRepeater;
        ItemManager.Instance.SetSprite(this);
    }
    
    public override void OnUse(Player player)
    {
        player.sequenceManager.sequence.RepeatSequence();
    }
    
}
