using UnityEngine;
using System.Collections;

public class HealthUpgrade : Item {
    public override string name { get { return "Health Upgrade"; } }
    public override int price { get { return 100; } }

    public override void Apply(Player player) {
        player.statSet.maxHealth += 50;
        player.health = player.statSet.maxHealth;
    }
}
