﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemUICardHolder : MonoBehaviour {

    public ItemUICard card;
    public RectTransform rect { get; private set; }

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
    }

    public void SetCard(ItemUICard card,Vector3 position,bool sound = false)
    {
        if (this.card != null)
        {
            this.card.GoToPosition(position);
            this.card.isDragable = true;
        }

        this.card = card;
        this.card.isDragable = false;
        if (sound)
            SoundController.Instance.PlaySound(SoundName.UIButton4);
        
    }
}
