using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_SequenceReducer : Item {

    private int reduceAmount;

    public Item_SequenceReducer(int reduceAmount)
    {
        type = ItemType.Reducer;
        this.reduceAmount = reduceAmount;
    }

    public override void OnUse(Player player)
    {
        player.sequenceManager.sequence.UnComplexify(reduceAmount);
    }
}
