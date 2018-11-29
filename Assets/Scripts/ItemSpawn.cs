using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawn : MonoBehaviour {

    public bool isEmpty { get { return item == null; } }
    public Vector3 position { get { return transform.position; }}
    private PickUpItem item;

    public void SetItem(PickUpItem item)
    {
        this.item = item;
    }
}
