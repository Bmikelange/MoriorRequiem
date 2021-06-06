using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceSlot {
    public Dice dice = null;
    public int cooldown = 0;

    public DiceSlot(Dice _dice) {
        dice = _dice;
        cooldown = 0;
    }

    public DiceSlot(DiceSlot other) {
        dice = new Dice(other.dice);
        cooldown = other.cooldown;
    }

    public void applyCooldown() {
        cooldown = dice.cooldown;
    }
}
