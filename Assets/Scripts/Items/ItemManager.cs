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
            switch (i) {
                case 0:
                    card.SetUp(new Item_DamageUp(2), cardHolders);
                    break;
                case 1:
                    card.SetUp(new Item_SequenceReducer(2), cardHolders);
                    break;
                case 2:
                    card.SetUp(new Item_SequenceRepeater(5), cardHolders);
                    break;
            }
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
}
