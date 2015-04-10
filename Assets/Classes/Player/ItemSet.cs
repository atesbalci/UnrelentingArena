using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ItemSet {
    public const int CAP = 6;

    public List<Item> potentialItems;
    private List<Item> items;

    public ItemSet() {
        items = new List<Item>(CAP);
        potentialItems = new List<Item>();
        potentialItems.Add(new HealthUpgrade());
    }

    public void Apply(Player player) {
        foreach (Item item in items)
            item.Apply(player);
    }

    public bool Has(Item item) {
        return items.Contains(item);
    }

    public void Get(int no) {
        items.Add(potentialItems[no]);
    }
}
