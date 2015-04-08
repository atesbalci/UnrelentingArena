using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ItemSet {
    public LinkedList<Item> items;

    public ItemSet() {
        items = new LinkedList<Item>();
    }

    public void Apply(Player player) {
        foreach (Item item in items)
            item.Apply(player);
    }
}
