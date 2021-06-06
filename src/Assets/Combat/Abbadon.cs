using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Abbadon : Enemy {
    public Abbadon(string _name, int _life, List<DiceSlot> _slots) : base(_name, _life, _slots) { }

    int phase = 0;

    private void SetPhase() {
        phase = Mathf.Max(phase, 3 - (int)((float)(life) / (float)lifeMax * 3f));
    }
    public override void action(int selfId, List<Entity> entities) {
        if (!canPlay()) {
            endTurn();
            return;
        }

        SetPhase();

        List<DiceSlot> availableDices = new List<DiceSlot>();
        for (int i = 0; i < slots.Count; ++i) {

            if (slots[i].cooldown == 0 && i < phase + 1) {
                availableDices.Add(slots[i]);
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
