using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PickUpItem : MonoBehaviour {

    [SerializeField] private Image display;
    [SerializeField] private GameObject display3D;
    [SerializeField] private float animationSpeed = 1;
    [SerializeField] private ParticleSystem particles;
    [SerializeField] private AudioClip itemAcquired;

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
        if (other.gameObject.tag != "Player")
            return;

        Item[] playerItems = player.items.GetItems();
        if (playerItems[0].type == ItemType.Undefined)
        {
            player.items.SetItem(item, 0);
            player.items.SetOverItem(null);
            DestroyPickUp();
        }
        else if (playerItems[1].type == ItemType.Undefined)
        {
            player.items.SetItem(item, 1);
            player.items.SetOverItem(null);
            DestroyPickUp();
        }
    }

    private void DestroyPickUp()
    {
        SoundController.Instance.Say(itemAcquired);
        particles.Stop();
        particles.gameObject.transform.parent = transform.parent;
        Destroy(particles.gameObject, 2.0f);
        Destroy(gameObject);
    }
}
