﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemManager : SingletonBehaviour<ItemManager> {

    [System.Serializable]
    public struct ItemSpriteDictionnaryElement
    {
        public ItemType type;
        public Sprite sprite;
        public AudioClip sound;
    }
    [Header("Reference")]
    [SerializeField] private ItemUICardHolder[] cardHolders;
    [SerializeField] private RectTransform[] cardStands;
    [SerializeField] private ItemUICard itemCard;
    [SerializeField] private PickUpItem pickUpItem;
    [SerializeField] private GameObject itemManagementPanel;
    [SerializeField] private Text itemInfo;

    [Header("Parameters")]
    [SerializeField] private ItemSpriteDictionnaryElement[] InspectorDictionnary;
    [SerializeField] private Color weakColor;
    [SerializeField] private Color mediumColor;
    [SerializeField] private Color strongColor;

    [Header("Item spawn")]
    [SerializeField] private float spawnPeriod = 30;
    
    private Dictionary<ItemType, Sprite> itemsSpritesDictionnary;
    private Dictionary<ItemType, AudioClip> itemsSoundsDictionnary;
    Player player { get { return GameManager.instance.GetPlayer(); } }
    private List<ItemUICard> cards;
    private ItemSpawn[] spawnablePosition;
    private List<ItemSpawn> emptySpawn;

    public override void Awake()
    {
        base.Awake();
        itemsSpritesDictionnary = new Dictionary<ItemType, Sprite>();
        itemsSoundsDictionnary = new Dictionary<ItemType, AudioClip>();
        cards = new List<ItemUICard>();
        emptySpawn = new List<ItemSpawn>();
        for (int i = 0; i < InspectorDictionnary.Length; i++)
        {
            itemsSpritesDictionnary.Add(InspectorDictionnary[i].type, InspectorDictionnary[i].sprite);
            itemsSoundsDictionnary.Add(InspectorDictionnary[i].type, InspectorDictionnary[i].sound);
        }
    }

    private void Start()
    {
        spawnablePosition = GameObject.FindObjectsOfType<ItemSpawn>();
        StartCoroutine(EItemSpawn());
    }

    private void SpawnRandomItem()
    {
        if (spawnablePosition.Length < 1)
            return;

        emptySpawn.Clear();
        foreach(ItemSpawn spawn in spawnablePosition)
        {
            if (spawn.isEmpty)
                emptySpawn.Add(spawn);
        }

        if (emptySpawn.Count < 1)
            return;
        
        int rand = Random.Range(0, emptySpawn.Count);
        emptySpawn[rand].SetItem(CreatePickUpItem(GetRandomItem(), emptySpawn[rand].position));
    }

    IEnumerator EItemSpawn()
    {
        yield return new WaitForSeconds(spawnPeriod);
        SpawnRandomItem();
        if (spawnablePosition.Length < 1)
            yield break;
        StartCoroutine(EItemSpawn());
    }

    public PickUpItem CreatePickUpItem(Item item, Vector3 position)
    {
        PickUpItem pickUp = Instantiate(pickUpItem, position, Quaternion.identity);
        pickUp.SetUp(item);
        return pickUp;
    }

    public void SetUpItem(Item item)
    {
        Sprite sprite;
        if (itemsSpritesDictionnary.TryGetValue(item.type, out sprite))
        {
            item.sprite = sprite;
            item.color = GetColor(item.strength);
        }
        AudioClip sound;
        if (itemsSoundsDictionnary.TryGetValue(item.type, out sound))
        {
            item.sound = sound;
        }
    }

    public void InitCardDisplay()
    {
        DeleteCards();
        Player player = GameManager.instance.GetPlayer();
        Item[] items = player.items.GetItems();
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i].type == ItemType.Undefined)
                continue;

            ItemUICard card = Instantiate(itemCard, cardHolders[i].rect.position,Quaternion.identity);
            card.transform.parent = cardHolders[i].transform.parent;
            card.gameObject.GetComponent<RectTransform>().localScale = Vector3.one;
            card.SetUp(items[i],cardHolders);
            cardHolders[i].SetCard(card, Vector3.zero);
            card.isDragable = false;
            cards.Add(card);
        }

        for (int i = 0; i < cardStands.Length; i++)
        {
            ItemUICard card = Instantiate(itemCard, cardStands[i].position,Quaternion.identity);
            card.transform.parent = cardStands[i].transform.parent;
            card.gameObject.GetComponent<RectTransform>().localScale = Vector3.one;
            card.SetUp(GetRandomItem(),cardHolders);
            cards.Add(card);
        }
    }

    public void ApplyItemChanges()
    {
        if(cardHolders[0].card != null)
            player.items.SetItem(cardHolders[0].card.item, 0);
        if (cardHolders[1].card != null)
            player.items.SetItem(cardHolders[1].card.item, 1);    
    }

    public List<Item> RetrieveSelectedItems() {

        Item item1 = new Item();
        Item item2 = new Item();

        if (cardHolders[0].card != null)
           item1 = cardHolders[0].card.item;
        if (cardHolders[1].card != null)
           item2 = cardHolders[1].card.item;

        return new List<Item>() {
            item1,item2
        };
    }

    private void DeleteCards()
    {
        for(int i = 0; i < cards.Count; i++)
        {
            Destroy(cards[i].gameObject);
        }
        cards.Clear();
    }

    public Item GetRandomItem()
    {
        int random = Random.Range(0, InspectorDictionnary.Length-1);
        switch (random)
        {
            case 0:
                return new Item_DamageUp(GetRandomItemStrength());
            case 1:
                return new Item_FullSequenceRepeater();
            case 2:
                return new Item_SequenceRepeater(GetRandomItemStrength());
            case 3:
                return new Item_Shield(GetRandomItemStrength());
            case 4:
                return new Item_SequenceReducer(GetRandomItemStrength());
            case 5:
                return new Item_HealthUp(GetRandomItemStrength());
            case 6:
                return new Item_StaminaUp(GetRandomItemStrength());
            
            default:
                return new Item();
        }
    }

    public Color GetColor (ItemStrength strength)
    {
        switch (strength)
        {
            case ItemStrength.Weak:
                return weakColor;
            case ItemStrength.Medium:
                return mediumColor;
            case ItemStrength.Strong:
                return strongColor;
            default:
                return weakColor;
        }
    }

    private ItemStrength GetRandomItemStrength()
    {
        return (ItemStrength) Random.Range(0, 3);
    }

    public void SetInfo(string info)
    {
        itemInfo.text = info;
    }
}
