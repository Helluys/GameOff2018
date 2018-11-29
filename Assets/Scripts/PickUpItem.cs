using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PickUpItem : MonoBehaviour {

    [SerializeField] private Image display;
    [SerializeField] private GameObject display3D;
    [SerializeField] private float animationSpeed = 1;

    private Item item;
    private Player player;

    private void Update()
    {
        display3D.transform.eulerAngles += (Vector3.up+Vector3.forward) * Time.deltaTime * animationSpeed;
    }

    public void SetUp(Item item)
    {
        this.item = item;
        player = GameManager.instance.GetPlayer();
        display.sprite = item.sprite;
        display.color = item.color;
    }

    public Item GetItem()
    {
        return item;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag != "Player")
            return;

        player.items.SetOverItem(this);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag != "Player")
            return;

        player.items.SetOverItem(null);
    }

    private void OnTriggerStay(Collider other)
    {
        Item[] playerItems = player.items.GetItems();
        if (playerItems[0].type == ItemType.Undefined)
        {
            player.items.SetItem(item, 0);
            Destroy(gameObject);
            player.items.SetOverItem(null);
        }
        else if (playerItems[1].type == ItemType.Undefined)
        {
            player.items.SetItem(item, 1);
            Destroy(gameObject);
            player.items.SetOverItem(null);
        }
    }
}
