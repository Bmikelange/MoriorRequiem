using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity {

    public Enemy(string _name, int _life, List<DiceSlot> _slots) : base(_name, _life, _slots) { }

    public override void action(int selfId, List<Entity> entities) {
        if (!canPlay()) {
            endTurn();
            return;
        }

        List<DiceSlot> availableDices = new List<DiceSlot>();
        foreach (var slot in slots) {
            if (slot.cooldown == 0) {
                availableDices.Add(slot);
            }
        }
        if (availableDices.Count == 0) {
            endTurn();
            return;
        }

        DiceSlot choosenSlot = availableDices[Random.Range(0, availableDices.Count)];

        Dice choosenDice = choosenSlot.dice;
        choosenSlot.applyCooldown();
        endTurn(new ActionTurn(choosenSlot.dice, selfId));
    }
}
