using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemManager : SingletonBehaviour<ItemManager> {

    [System.Serializable]
    public struct ItemSpriteDictionnaryElement
    {
        public ItemType type;
        public Sprite sprite;
    }

    [SerializeField] private ItemSpriteDictionnaryElement[] InspectorDictionnary;
    private Dictionary<ItemType, Sprite> itemsSpritesDictionnary;

    [SerializeField] private ItemUICardHolder[] cardHolders;
    [SerializeField] private RectTransform[] cardStands;
    [SerializeField] private ItemUICard itemCard;
    [SerializeField] private GameObject itemManagementPanel;
    [SerializeField] private Text itemInfo;

    Player player { get { return GameManager.instance.GetPlayer(); } }

    private List<ItemUICard> cards;

    private void Start()
    {
        itemsSpritesDictionnary = new Dictionary<ItemType, Sprite>();
        cards = new List<ItemUICard>();
        for (int i = 0; i < InspectorDictionnary.Length; i++)
        {
            itemsSpritesDictionnary.Add(InspectorDictionnary[i].type, InspectorDictionnary[i].sprite);
        }
    }

    public void SetSprite(Item item)
    {
        Sprite sprite;
        if (itemsSpritesDictionnary.TryGetValue(item.type, out sprite))
        {
            item.sprite = sprite;
        }
    }

    public void InitCardDisplay()
    {
        DeleteCards();
        Player player = GameManager.instance.GetPlayer();
        Item[] items = player.items.GetItems();
        for(int i = 0; i < items.Length; i++)
        {
            if (items[i].type == ItemType.Undefined)
                continue;

            ItemUICard card = Instantiate(itemCard, cardHolders[i].rect.position, Quaternion.identity);
            card.transform.parent = cardHolders[i].transform.parent;
            card.SetUp(items[i],cardHolders);
            cardHolders[i].SetCard(card, Vector3.zero);
            card.isDragable = false;
            cards.Add(card);
        }

        for (int i = 0; i < cardStands.Length; i++)
        {
            ItemUICard card = Instantiate(itemCard, cardStands[i].position, Quaternion.identity);
            card.transform.parent = cardStands[i].transform.parent;
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

    private void DeleteCards()
    {
        for(int i = 0; i < cards.Count; i++)
        {
            Destroy(cards[i].gameObject);
        }
        cards.Clear();
    }

    private Item GetRandomItem()
    {
        int random = Random.Range(0, 5);
        switch (random)
        {
            case 0:
                return new Item_DamageUp(1.5f);
            case 1:
                return new Item_FullSequenceRepeater();
            case 2:
                return new Item_SequenceRepeater(3);
            case 3:
                return new Item_Shield(15, 3);
            case 4:
                return new Item_DamageUp(1.5f);
            default:
                return new Item();
        }
    }

    public void SetInfo(string info)
    {
        itemInfo.text = info;
    }
}
