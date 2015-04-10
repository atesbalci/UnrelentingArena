using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ItemSet {
    public const int CAP = 6;

    public LinkedList<Item> potentialItems;
    public List<Item> items;

    public ItemSet() {
        items = new List<Item>(CAP);
        potentialItems = new LinkedList<Item>();
        potentialItems.AddLast(new LinkedListNode<Item>(new HealthUpgrade()));
    }

    public void Apply(Player player) {
        foreach (Item item in items)
            item.Apply(player);
    }
}
