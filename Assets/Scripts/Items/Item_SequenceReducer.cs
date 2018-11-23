using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_SequenceReducer : Item {

    private int reduceAmount;

    public Item_SequenceReducer(ItemStrength strength)
    {
        type = ItemType.Reducer;
        this.strength = strength;
        switch (strength)
        {
            case ItemStrength.Weak:
                reduceAmount = 2;
                break;
            case ItemStrength.Medium:
                reduceAmount = 4;
                break;
            case ItemStrength.Strong:
                reduceAmount = 6;
                break;
        }

        ItemManager.Instance.SetUpItem(this);
    }

    public override string GetInfo()
    {
        return string.Format("Reduce the current sequence of {0} inputs", reduceAmount);
    }

    public override void OnUse(Player player)
    {
        base.OnUse(player);
        player.sequenceManager.sequence.UnComplexify(reduceAmount);
    }
    
}
