using UnityEngine;
using System.Collections;

public abstract class Item {
    public abstract string name { get; }
    public abstract int price { get; }
    public abstract void Apply(Player player);
}
