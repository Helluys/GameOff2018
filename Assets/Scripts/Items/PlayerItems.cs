using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerItems{

    [SerializeField] private int itemAmount = 2;
    private Item[] items;
    private Player player;

    private PickUpItem overItem;

    public void OnStart(Player player)
    {
        items = new Item[itemAmount];
        for (int i = 0; i < items.Length; i++)
            items[i] = new Item();
        this.player = player;
    }

    public void OnUpdate()
    {
        if (overItem == null)
        {
            if (InputManager.Instance.IsKeyDown(InputType.Item1))
                UseItem(0);
            if (InputManager.Instance.IsKeyDown(InputType.Item2))
                UseItem(1);
        }
        else
        {
            if (InputManager.Instance.IsKeyDown(InputType.Item1))
                SwitchItem(0);
            if (InputManager.Instance.IsKeyDown(InputType.Item2))
                SwitchItem(1);
        }
    }

    public void SetOverItem(PickUpItem pickUp)
    {
        overItem = pickUp;
    }

    public void SwitchItem(int index)
    {
        Item temp = items[index];
        SetItem(overItem.GetItem(), index);
        overItem.SetUp(temp);
    }

    public void SetItem(Item item,int index)
    {
        if (index < 0 || index > items.Length - 1)
            return;
        items[index] = item;
        player.hud.SetItemDisplay(item,index);
    }

    public void UseItem(int index)
    {
        if (index < 0 || index > items.Length - 1)
            return;

        items[index].OnUse(player);
        items[index] = new Item();
        player.hud.SetItemDisplay(items[index],index);
    }

    public Item[] GetItems()
    {
        return items;
    }
}
