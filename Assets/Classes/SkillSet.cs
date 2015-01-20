using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SkillSet {

    public SkillSet() {
        
    }

    public Fireball castFireball(Player player) {
        Fireball fireball = new Fireball(player);
        return fireball;
    }

    public Blink castBlink(Player player) {
        Blink blink = new Blink(player);
        return blink;
    }
}
