using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerItems{

    [SerializeField] private int itemAmount = 2;
    [SerializeField] private KeyCode[] keys = new KeyCode[2] { KeyCode.A, KeyCode.E };
    private Item[] items;
    private Player player;

    public void OnStart(Player player)
    {
        items = new Item[itemAmount];
        for (int i = 0; i < items.Length; i++)
            items[i] = new Item();
        this.player = player;
    }

    public void OnUpdate()
    {
        if (Input.GetKeyDown(keys[0]))
            UseItem(0);
        if (Input.GetKeyDown(keys[1]))
            UseItem(1);
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
